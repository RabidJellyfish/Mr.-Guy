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
		private bool onGround, facingLeft, pressW, holdW;
		private Vector2 jumpForce, rightAirForce, leftAirForce, prevVelocity, start, end, normal, p;
		private AnimatedTexture texIdle, texRun, texJump;
		private AnimatedTexture currentTexture;

		public Vector2 Position { get { return torso.Position; } }

		public Player(World world, float x, float y, Texture2D texture)
		{
			torso = BodyFactory.CreateRectangle(world, 60 * MainGame.PIXEL_TO_METER, 80 * MainGame.PIXEL_TO_METER, 1);
			torso.Position = new Vector2(x * MainGame.PIXEL_TO_METER, y * MainGame.PIXEL_TO_METER);
			torso.BodyType = BodyType.Dynamic;
			torso.UserData = this;
			legs = BodyFactory.CreateCircle(world, 30 * MainGame.PIXEL_TO_METER, 1);
			legs.Position = torso.Position + new Vector2(0, 40 * MainGame.PIXEL_TO_METER);
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
			facingLeft = false;
			jumpForce = new Vector2(0, -1f);
			rightAirForce = new Vector2(5f, 0);
			leftAirForce = -1 * rightAirForce;
			prevVelocity = Vector2.Zero;
			normal = Vector2.Zero;
			pressW = false;
			holdW = false;

			texIdle = new AnimatedTexture(texture, 24, 0, 0, 120, 140);
			texRun = new AnimatedTexture(texture, 19, 0, 140, 120, 140);
			texJump = new AnimatedTexture(texture, 9, 19 * 120, 140, 120, 140, 1, false, false);
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
				int frame = (int)(torso.LinearVelocity.Y * MainGame.METER_TO_PIXEL) / 16 + 3;
				if (frame < 0) frame = 0;
				else if (frame > 8) frame = 8;
				texJump.Frame = frame;
			}

			// Walking
			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				facingLeft = true;
				axis.MotorSpeed = 2.5f * -MathHelper.TwoPi;
				if (onGround)
				{
					currentTexture = texRun;
				}
				if (!onGround && torso.LinearVelocity.X > -2)
					torso.ApplyForce(ref leftAirForce);
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				facingLeft = false;
				axis.MotorSpeed = 2.5f * MathHelper.TwoPi;
				if (onGround)
				{
					currentTexture = texRun;
				}
				if (!onGround && torso.LinearVelocity.X < 2)
					torso.ApplyForce(ref rightAirForce);
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
			start = legs.Position + new Vector2(0, 24 * MainGame.PIXEL_TO_METER);
			end = start + new Vector2(0, 24 * MainGame.PIXEL_TO_METER);
			for (int i = -30; i <= 30; i += 15)
			{
				start.X = legs.Position.X + i * MainGame.PIXEL_TO_METER;
				end.X = legs.Position.X + i * MainGame.PIXEL_TO_METER;
				w.RayCast((f, p, n, fr) =>
				{
					normal = n;
					if (f != null)
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
						if (Keyboard.GetState().IsKeyDown(Keys.A))
							torso.LinearVelocity -= 0.7f * Vector2.UnitX;
						if (Keyboard.GetState().IsKeyDown(Keys.D))
							torso.LinearVelocity += 0.7f * Vector2.UnitX;
						torso.LinearVelocity = new Vector2(torso.LinearVelocity.X, -8);
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
				Vector2 start = light.Position * MainGame.PIXEL_TO_METER;
				Vector2 end = torso.Position + Vector2.UnitY * i * MainGame.PIXEL_TO_METER - start;
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
			currentTexture.Draw(sb, torso.Position * MainGame.METER_TO_PIXEL, Color.White, torso.Rotation, new Vector2(60, 60), Vector2.One, facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

//			sb.Draw(Game1.texture, new Rectangle((int)(torso.Position.X * Game1.unitToPixel) - 16, (int)(torso.Position.Y * Game1.unitToPixel) - 16, 32, 46), Color.Green);

//			sb.Draw(MainGame.texBox, torso.Position * MainGame.METER_TO_PIXEL, null, Color.White, torso.Rotation, new Vector2(MainGame.texBox.Width / 2.0f, MainGame.texBox.Height / 2.0f), new Vector2(60f / MainGame.texBox.Width, 80f / MainGame.texBox.Height), SpriteEffects.None, 0);
//			sb.Draw(MainGame.texBox, legs.Position * MainGame.METER_TO_PIXEL, null, Color.White, legs.Rotation, new Vector2(MainGame.texBox.Width / 2.0f, MainGame.texBox.Height / 2.0f), new Vector2(60f / MainGame.texBox.Width, 60f / MainGame.texBox.Height), SpriteEffects.None, 0);

//			MainGame.DrawLine(sb, MainGame.blank, 1, Color.Black, start * MainGame.METER_TO_PIXEL, end * MainGame.METER_TO_PIXEL);

//			sb.DrawString(Game1.font, normal.ToString(), Vector2.Zero, Color.Black);
		}
	}
}
