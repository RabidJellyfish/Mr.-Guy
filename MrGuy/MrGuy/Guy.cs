﻿using System;
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

namespace MrGuy
{
	class Guy
	{
		// Physics bodies
		private World world;
		private Body torso, legs;
		private RevoluteJoint axis;

		// Control values
		private const float JUMP_VELOCITY = -8f;
		private Vector2 JUMP_FORCE = -Vector2.UnitY * 1f;
		private const float MAX_AIRSPEED = 2f;
		private const float AIR_FORCE = 5f;
		private const float MOTOR_SPEED = 2.5f * MathHelper.TwoPi;

		private Vector2 left_airForce = -AIR_FORCE * Vector2.UnitX;
		private Vector2 right_airForce = AIR_FORCE * Vector2.UnitX;

		private bool onGround;
		private bool facingLeft;
		private bool startJump, holdJump;

		// Drawing
		private AnimatedTexture texIdle, texRun, texJump, texCrouch, texRoll;
		private AnimatedTexture currentTexture;

		/// <summary>
		/// Gets and sets the position of MrGuy (in pixel coordinates)
		/// </summary>
		public Vector2 Position
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
				_crouching = value;
				torso.CollidesWith = _crouching ? Category.None : Category.All;
			}
		}

		#region Creation

		public Guy(World world, float x, float y, Texture2D texture)
		{
			CreateBody(world, x, y);

			onGround = false;
			facingLeft = false;
			startJump = holdJump = false;

			SetUpTextures(texture);
		}

		private void CreateBody(World w, float x, float y)
		{
			world = w;

			torso = BodyFactory.CreateRectangle(w, 60 * MainGame.PIXEL_TO_METER, 80 * MainGame.PIXEL_TO_METER, 1);
			torso.BodyType = BodyType.Dynamic;
			torso.UserData = this;
			legs = BodyFactory.CreateCircle(w, 31 * MainGame.PIXEL_TO_METER, 1);
			legs.BodyType = BodyType.Dynamic;
			legs.Friction = 5.0f;
			legs.UserData = this;

			this.Position = new Vector2(x, y);

			JointFactory.CreateFixedAngleJoint(w, torso);
			axis = JointFactory.CreateRevoluteJoint(w, torso, legs, Vector2.Zero);
			axis.CollideConnected = false;
			axis.MotorEnabled = true;
			axis.MotorSpeed = 0.0f;
			axis.MotorTorque = 3.0f;
			axis.MaxMotorTorque = 10.0f;
		}

		private void SetUpTextures(Texture2D guyTexture)
		{
			texIdle = new AnimatedTexture(guyTexture, 24, 0, 0, 120, 140);
			texRun = new AnimatedTexture(guyTexture, 19, 0, 140, 120, 140);
			texJump = new AnimatedTexture(guyTexture, 9, 19 * 120, 140, 120, 140, 1, false, false);
			texCrouch = new AnimatedTexture(guyTexture, 1, 4 * 120, 280, 120, 140);
			texRoll = new AnimatedTexture(guyTexture, 1, 5 * 120, 280, 120, 140);
			
			currentTexture = texIdle;
		}

		#endregion

		#region Updating

		public virtual void Update()
		{
			UpdateTexture();
			UpdateMovement();
			CheckOnGround();
			UpdateJumping();
		}

		private void UpdateTexture()
		{
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
		}

		private void UpdateMovement()
		{
			if (MovingLeft)
			{
				facingLeft = true;
				axis.MotorSpeed = 2.5f * -MathHelper.TwoPi;
				if (onGround)
					if (Crouching)
						currentTexture = texRoll;
					else
						currentTexture = texRun;
				if (!onGround && torso.LinearVelocity.X > -2)
					torso.ApplyForce(ref left_airForce);
			}
			else if (MovingRight)
			{
				facingLeft = false;
				axis.MotorSpeed = 2.5f * MathHelper.TwoPi;
				if (onGround)
					if (Crouching)
						currentTexture = texRoll;
					else
						currentTexture = texRun;
				if (!onGround && torso.LinearVelocity.X < 2)
					torso.ApplyForce(ref right_airForce);
			}
			else
			{
				if (onGround)
					if (Crouching)
						currentTexture = texCrouch;
					else
						currentTexture = texIdle;
				axis.MotorSpeed = 0;
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
			Vector2 end = start + new Vector2(0, 24 * MainGame.PIXEL_TO_METER);
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
			if (Jumping)
			{
				if (!startJump)
				{
					if (onGround)
					{
						if (Keyboard.GetState().IsKeyDown(Keys.A))
							torso.LinearVelocity -= 0.7f * Vector2.UnitX;
						if (Keyboard.GetState().IsKeyDown(Keys.D))
							torso.LinearVelocity += 0.7f * Vector2.UnitX;
						torso.LinearVelocity = new Vector2(torso.LinearVelocity.X, JUMP_VELOCITY);
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
			if (holdJump && torso.LinearVelocity.Y < 0)
				torso.ApplyForce(JUMP_FORCE);
		}

		#endregion

		public virtual void Draw(SpriteBatch sb)
		{
			currentTexture.Draw(
				sb,
				Crouching && onGround ? legs.Position * MainGame.METER_TO_PIXEL : torso.Position * MainGame.METER_TO_PIXEL,
				Color.White,
				Crouching && (MovingLeft || MovingRight) && onGround? legs.Rotation : 0f,
				new Vector2(60, 70),
				Vector2.One,
				facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 
				0);
		}
	}
}
