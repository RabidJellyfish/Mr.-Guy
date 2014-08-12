using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace MrGuy.Objects
{
	class EmptyObject : GameObject
	{
		public EmptyObject(World w, int index, Vector2 position)
			: base()
		{
			this.Position = position;
			this.Index = index;
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
		{
		}
	}
}
