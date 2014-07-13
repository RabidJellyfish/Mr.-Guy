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
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

namespace MrGuy.Objects
{
	class Guy : GameObject
	{
		// Physics bodies
		private World world;
		private Body torso, legs;
		private RevoluteJoint axis;

		// Control values
		private const float JUMP_VELOCITY = -7f;
		private Vector2 JUMP_FORCE = -Vector2.UnitY * 2f;
		private const float SWIM_VELOCITY = -2f;
		private const float MAX_AIRSPEED = 2f;
		private const float AIR_FORCE = 5f;
		private const float MOTOR_SPEED = 2.5f * MathHelper.TwoPi;

		private Vector2 left_airForce = -AIR_FORCE * Vector2.UnitX;
		private Vector2 left_waterForce = -AIR_FORCE * 1.5f * Vector2.UnitX;
		private Vector2 right_airForce = AIR_FORCE * Vector2.UnitX;
		private Vector2 right_waterForce = AIR_FORCE * 1.5f * Vector2.UnitX;

		private bool onGround;
		private bool inWater, swimming;
		private bool facingLeft;
		private bool startJump, holdJump;

		// Drawing
		protected AnimatedTexture texIdle, texRun, texJump, texRoll, texWaterIdle, texSwim;
		protected AnimatedTexture currentTexture;

		/// <summary>
		/// Gets and sets the position of MrGuy (in pixel coordinates)
		/// </summary>
		public override Vector2 Position
		{
			get { return torso.Position * MainGame.METER_TO_PIXEL; }
			set
			{
				torso.Position = value * MainGame.PIXEL_TO_METER;
				legs.Position = torso.Position + Vector2.UnitY * 40 * MainGame.PIXEL_TO_METER;
			}
		}

		// Actions
		public bool MovingLeft { get; set; }
		public bool MovingRight { get; set; }
		public bool Jumping { get; set; }
		private bool _crouching;
		public bool Crouching 
		{
			get { return _crouching; }
			set
			{
				if (!_crouching && value)
					legs.Rotation = 0f;
				_crouching = value;
				torso.CollidesWith = _crouching ? Category.None : Category.All;
			}
		}

		#region Creation

		public Guy(World world, float x, float y, Texture2D texture)
		{
			CreateBody(world, x, y);

			onGround = false;
			inWater = false;
			facingLeft = false;
			startJump = holdJump = false;

			texIdle = new AnimatedTexture(texture, 24, 0, 0, 120, 140);
			texRun = new AnimatedTexture(texture, 19, 0, 144, 120, 140);
			texJump = new AnimatedTexture(texture, 9, 19 * 120, 144, 120, 140, 1, false, false);
			texRoll = new AnimatedTexture(texture, 1, 4 * 120, 288, 120, 140);
			texWaterIdle = new AnimatedTexture(texture, 14, 5 * 120, 288, 120, 140, 2, true, false);
			texSwim = new AnimatedTexture(texture, 17, 19 * 120, 288, 120, 144);

			currentTexture = texIdle;
		}

		private void CreateBody(World w, float x, float y)
		{
			world = w;

			torso = BodyFactory.CreateRectangle(w, 60 * MainGame.PIXEL_TO_METER, 80 * MainGame.PIXEL_TO_METER, 1);
			torso.BodyType = BodyType.Dynamic;
			torso.UserData = this;
			legs = BodyFactory.CreateCircle(w, 32 * MainGame.PIXEL_TO_METER, 1);
			legs.BodyType = BodyType.Dynamic;
			legs.Friction = 5.0f;
			legs.UserData = this;

			this.Position = new Vector2(x, y);

			JointFactory.CreateFixedAngleJoint(w, torso);
			axis = JointFactory.CreateRevoluteJoint(w, torso, legs, Vector2.Zero);
			axis.CollideConnected = false;
			axis.MotorEnabled = true;
			axis.MotorSpeed = 0.0f;
			axis.MaxMotorTorque = 10.0f;
		}

		#endregion

		#region Updating

		public override void Update(List<GameObject> otherObjects)
		{
			UpdateMovement();
			CheckOnGround();
			UpdateJumping();
			UpdateTexture();
		}

		private void UpdateTexture()
		{
			if (swimming && !Crouching)
				currentTexture = texSwim;
			currentTexture.Update();
			if (swimming && currentTexture.Frame == texSwim.Length - 1)
			{
				texSwim.Frame = 0;
				swimming = false;
			}

			if (!onGround && !Crouching && !inWater)
			{
				if (currentTexture != texJump)
					currentTexture = texJump;
				int frame = (int)(torso.LinearVelocity.Y * MainGame.METER_TO_PIXEL) / 16 + 3;
				if (frame < 0) frame = 0;
				else if (frame > 8) frame = 8;
				texJump.Frame = frame;
			}
		}

		private void UpdateMovement()
		{
			if (Crouching)
				currentTexture = texRoll;

			if (MovingLeft)
			{
				facingLeft = true;
				axis.MotorEnabled = true;
				axis.MaxMotorTorque = Crouching ? 0.3f : 10.0f;
				axis.MotorSpeed = -MOTOR_SPEED;
				if (!Crouching)
				{
					if (onGround)
						currentTexture = texRun;
					else if (inWater)
						swimming = true;
				}
				if (!onGround && torso.LinearVelocity.X > -MAX_AIRSPEED * (inWater ? 1.5f : 1f))
					torso.ApplyForce(inWater ? left_waterForce : left_airForce);
			}
			else if (MovingRight)
			{
				facingLeft = false;
				axis.MotorEnabled = true;
				axis.MaxMotorTorque = Crouching ? 0.3f : 10.0f;
				axis.MotorSpeed = MOTOR_SPEED;
				if (!Crouching)
				{
					if (onGround)
						currentTexture = texRun;
					else if (inWater)
						swimming = true;
				}
				if (!onGround && torso.LinearVelocity.X < MAX_AIRSPEED * (inWater ? 1.5f : 1f))
					torso.ApplyForce(inWater ? right_waterForce : right_airForce);
			}
			else
			{
				if (onGround)
				{
					if (Crouching)
					{
						axis.MotorEnabled = false;
						axis.MaxMotorTorque = 0.0f;
					}
					else
					{
						currentTexture = texIdle;
						axis.MotorEnabled = true;
						axis.MaxMotorTorque = 10.0f;
						axis.MotorSpeed = 0;
					}
				}
				else
				{
					if (inWater && !Crouching)
					{
						currentTexture = texWaterIdle;
						if (Math.Abs(torso.LinearVelocity.X) > 0)
							torso.ApplyForce(-Math.Sign(torso.LinearVelocity.X) * Vector2.UnitX * 3);
					}
				}
			}

			if (!onGround)
				axis.MotorEnabled = false;
			else
				axis.MotorEnabled = true;
		}

		private void CheckOnGround()
		{
			onGround = false;
			Vector2 start = legs.Position + new Vector2(0, 24 * MainGame.PIXEL_TO_METER);
			Vector2 end = start + new Vector2(0, 12 * MainGame.PIXEL_TO_METER);
			for (int i = -30; i <= 30; i += 15)
			{
				start.X = legs.Position.X + i * MainGame.PIXEL_TO_METER;
				end.X = legs.Position.X + i * MainGame.PIXEL_TO_METER;
				world.RayCast((f, p, n, fr) =>
				{
					if (f != null)
						onGround = true;
					else
						onGround = false;
					return 0;
				}, start, end);

				if (onGround)
					break;
			}
		}

		private void UpdateJumping()
		{
			if (inWater)
			{
				if (torso.LinearVelocity.Y < (Crouching ? 5f : 2.5f))
					torso.ApplyForce(Vector2.UnitY * (Crouching ? -3f : -6f) * (torso.Mass + legs.Mass));
				else if (torso.LinearVelocity.Y > (Crouching ? 5f : 2.5f))
					torso.ApplyForce(Vector2.UnitY * -14f * (torso.Mass + legs.Mass));
				else
					torso.ApplyForce(Vector2.UnitY * -world.Gravity * (torso.Mass + legs.Mass));
			}

			if (Jumping)
			{
				if (!startJump)
				{
					if (onGround || inWater)
					{
						// Bunny hopping
						// Commented out because OP on upward slopes
//						if (!inWater)
//						{
//							if (Keyboard.GetState().IsKeyDown(Keys.A))
//								torso.LinearVelocity -= 0.7f * Vector2.UnitX;
//							if (Keyboard.GetState().IsKeyDown(Keys.D))
//								torso.LinearVelocity += 0.7f * Vector2.UnitX;
//						}

						if (!(inWater && Crouching))
							torso.LinearVelocity = new Vector2(torso.LinearVelocity.X, JUMP_VELOCITY * (inWater ? 0.5f : 1f));
						if (inWater)
							swimming = true;
						holdJump = true;
					}
					startJump = true;
				}
			}
			else
			{
				startJump = false;
				holdJump = false;
			}
			if (holdJump && torso.LinearVelocity.Y < 0 && !inWater)
				torso.ApplyForce(JUMP_FORCE);
		}

		#endregion

		public override void Draw(SpriteBatch sb)
		{
			currentTexture.Draw(
				sb,
				Crouching ? legs.Position * MainGame.METER_TO_PIXEL : torso.Position * MainGame.METER_TO_PIXEL,
				Color.White,
				Crouching ? legs.Rotation : 0f,
				new Vector2(60, 70),
				Vector2.One,
				facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
				0.5f);
		}
	}
}
