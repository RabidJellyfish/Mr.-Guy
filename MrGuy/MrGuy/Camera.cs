using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MrGuy.Objects;

namespace MrGuy
{
	public class Camera
	{
		private Vector2 pos;
		public Vector2 Position { get { return this.pos; } set { pos = value; } }
		public int X { get { return (int)Position.X; } set { pos.X = value; } }
		public int Y { get { return (int)Position.Y; } set { pos.Y = value; } }

		private Vector2 bounds;

		public float MaxSpeed { get; set; }
		public PhysicsObject Target1 { get; set; }
		public PhysicsObject Target2 { get; set; }
		public float Target2Priority { get; set; }

		public Camera(Vector2 pos, Vector2 bounds, float maxSpeed, PhysicsObject target)
		{
			this.Target1 = target;
			this.Target2 = target;
			this.Target2Priority = 0f;
			this.MaxSpeed = maxSpeed;
			this.pos = pos;
			this.bounds = bounds;
		}

		public void Update()
		{
			this.pos = Target1.Position + (Target2.Position - Target1.Position) * Target2Priority - MainGame.MAX_RES_X / 2 * Vector2.UnitX - MainGame.MAX_RES_Y / 2 * Vector2.UnitY;
			StayInBounds();
		}

		private void StayInBounds()
		{
			if (this.pos.X < 0)
				this.pos.X = 0;
			else if (this.pos.X + MainGame.MAX_RES_X > bounds.X)
				this.pos.X = (bounds.X - MainGame.MAX_RES_X) > 0 ? bounds.X - MainGame.MAX_RES_X : 0;
			if (this.pos.Y < 0)
				this.pos.Y = 0;
			else if (this.pos.Y + MainGame.MAX_RES_Y > bounds.Y)
				this.pos.Y = (bounds.Y - MainGame.MAX_RES_Y) > 0 ? bounds.Y - MainGame.MAX_RES_Y : 0;
		}
	}
}
