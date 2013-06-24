using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;

using Krypton.Lights;

namespace MrGuy
{
	class Player
	{
		private Body torso, legs;
		private FarseerPhysics.Dynamics.Joints.RevoluteJoint axis;
		private bool onGround, pressW, holdW;
		private Vector2 jumpForce, rightAirSpeed, leftAirSpeed, prevVelocity, start, end, normal, p;
		private AnimatedTexture texIdle, texRun, texJump;
		private AnimatedTexture currentTexture;

		public Player(World world, float x, float y, Texture2D texture)
		{
			torso = BodyFactory.CreateRectangle(world, 32 * MainGame.PIXEL_TO_METER, 32 * MainGame.PIXEL_TO_METER, 1);
			torso.Position = new Vector2(x * MainGame.PIXEL_TO_METER, y * MainGame.PIXEL_TO_METER);
			torso.BodyType = BodyType.Dynamic;
			torso.UserData = this;
			legs = BodyFactory.CreateCircle(world, 16 * MainGame.PIXEL_TO_METER, 1);
			legs.Position = torso.Position + new Vector2(0, 16 * MainGame.PIXEL_TO_METER);
			legs.BodyType = BodyType.Dynamic;
			legs.Friction = 5.0f;
			legs.UserData = this;
			JointFactory.CreateFixedAngleJoint(world, torso);
			axis = JointFactory.CreateRevoluteJoint(world, torso, legs, Vector2.Zero);
			axis.CollideConnected = false;
			axis.MotorEnabled = true;
			axis.MotorSpeed = 0.0f;
			axis.MotorTorque = 3.0f;
			axis.MaxMotorTorque = 10.0f;
			onGround = false;
			jumpForce = new Vector2(0, -1f);
			rightAirSpeed = new Vector2(0.8f, 0);
			leftAirSpeed = -1 * rightAirSpeed;
			prevVelocity = Vector2.Zero;
			normal = Vector2.Zero;
			pressW = false;
			holdW = false;

			texIdle = new AnimatedTexture(texture, 19, 9 * 64, 0, 64, 64);
			texRun = new AnimatedTexture(texture, 19, 28 * 64, 0, 64, 64);
			texJump = new AnimatedTexture(texture, 9, 0, 0, 64, 64, 1, false, false);
			currentTexture = texIdle;
		}

		public void Update(World w)
		{
			// Animate texture
			currentTexture.Update();
			if (!onGround)
			{
				if (currentTexture != texJump)
					currentTexture = texJump;
				int frame = (int)(torso.LinearVelocity.Y * MainGame.METER_TO_PIXEL) / 8 + 3;
				if (frame < 0) frame = 0;
				else if (frame > 8) frame = 8;
				texJump.Frame = frame;
			}

			// Walking
			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				if (onGround)
				{
					axis.MotorSpeed = 3 * -MathHelper.TwoPi;
					currentTexture = texRun;
				}
				if (!onGround && torso.LinearVelocity.X > -1)
					torso.ApplyForce(ref leftAirSpeed);
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				if (onGround)
				{
					axis.MotorSpeed = 3 * MathHelper.TwoPi;
					currentTexture = texRun;
				}
				if (!onGround && torso.LinearVelocity.X < 1)
					torso.ApplyForce(ref rightAirSpeed);
			}
			else
			{
				if (onGround)
					currentTexture = texIdle;
				axis.MotorSpeed = 0;
			}

			if (!onGround)
				axis.MotorSpeed = 0;

			// Check if you're standing on something
			onGround = false;
			Vector2 start = legs.Position + new Vector2(0, 12 * MainGame.PIXEL_TO_METER);
			Vector2 end = start + new Vector2(0, 12 * MainGame.PIXEL_TO_METER);
			for (int i = -8; i <= 8; i += 8)
			{
				start.X = legs.Position.X + i * MainGame.PIXEL_TO_METER;
				end.X = legs.Position.X + i * MainGame.PIXEL_TO_METER;
				w.RayCast((f, p, n, fr) =>
				{
					normal = n;
					if (f != null && (string)f.UserData != "nonjumpable")
						onGround = true;
					else
						onGround = false;
					return 0;
				}, start, end);

				if (onGround)
					break;
			}

			// Jump
			if (Keyboard.GetState().IsKeyDown(Keys.W))
			{
				if (!pressW)
				{
					if (onGround)
					{
						torso.LinearVelocity = new Vector2(torso.LinearVelocity.X + 2 * normal.X, 6 * normal.Y);
						holdW = true;
					}
					pressW = true;
				}
			}
			else
			{
				pressW = false;
				holdW = false;
			}
			if (holdW && torso.LinearVelocity.Y < 0)
				torso.ApplyForce(jumpForce);

			prevVelocity = torso.LinearVelocity;
		}

		public bool InLight(World w, Light2D light)
		{
			List<float> fractions = new List<float>();
			for (float i = -24f; i <= 24f; i += 24f)
			{
				start = light.Position * MainGame.PIXEL_TO_METER;
				end = torso.Position + Vector2.UnitY * i * MainGame.PIXEL_TO_METER - start;
				end.Normalize();
				end *= (light.Range - 32) * MainGame.PIXEL_TO_METER;
				end += start;
				fractions.Clear();
				float foundFr = (end - start).Length();
				w.RayCast((f, p, n, fr) =>
					{
						this.p = p;
						if (f.Body.UserData == this)
							foundFr = fr;
						if (f.Body.UserData == "hasshadow" || f.UserData == "hasshadow" || f.Body.UserData == this)
						{
							fractions.Add(fr);
							return fr;
						}
						else
							return -1;
					}, start, end);
				fractions.Sort();
				if (fractions.Count > 0 && fractions[0] == foundFr)
					return true;
			}
			return false;
		}

		public void Draw(SpriteBatch sb)
		{
			currentTexture.Draw(sb, torso.Position * MainGame.METER_TO_PIXEL, Color.White, torso.Rotation, new Vector2(32, 32), Vector2.One, torso.LinearVelocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			MainGame.DrawLine(sb, MainGame.blank, 1, Color.Black, start * MainGame.METER_TO_PIXEL, p * MainGame.METER_TO_PIXEL);

//			sb.Draw(Game1.texture, new Rectangle((int)(torso.Position.X * Game1.unitToPixel) - 16, (int)(torso.Position.Y * Game1.unitToPixel) - 16, 32, 46), Color.Green);
			
//			sb.Draw(Game1.texture, torso.Position * Game1.unitToPixel, null, Color.White, torso.Rotation, new Vector2(Game1.texture.Width / 2.0f, Game1.texture.Height / 2.0f), new Vector2(32 / Game1.texture.Width, 32 / Game1.texture.Height), SpriteEffects.None, 0);
//			sb.Draw(Game1.texture, legs.Position * Game1.unitToPixel, null, Color.White, legs.Rotation, new Vector2(Game1.texture.Width / 2.0f, Game1.texture.Height / 2.0f), new Vector2(32 / Game1.texture.Width, 32 / Game1.texture.Height), SpriteEffects.None, 0);

//			sb.DrawString(Game1.font, normal.ToString(), Vector2.Zero, Color.Black);
		}
	}
}
