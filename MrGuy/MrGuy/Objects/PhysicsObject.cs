using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;

namespace MrGuy.Objects
{
	public abstract class PhysicsObject
	{
		public Vector2 Position { get; set; }

		public abstract void Initialize(World w);

		public abstract void Update();

		public abstract void Draw(SpriteBatch sb);
	}
}
