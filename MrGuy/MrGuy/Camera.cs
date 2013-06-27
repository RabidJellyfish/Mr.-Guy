using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuy
{
	public class Camera
	{
		private Vector2 pos;
		public Vector2 Position { get { return this.pos; } set { pos = value; } }
		public int X { get { return (int)Position.X; } set { pos.X = value; } }
		public int Y { get { return (int)Position.Y; } set { pos.Y = value; } }
		private Vector2 bounds;

		public Camera(Vector2 pos, Vector2 bounds)
		{
			this.pos = pos;
			this.bounds = bounds;
		}

		public void Update()
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
