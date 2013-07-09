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
	public abstract class GameObject
	{
		public int Index { get; set; }

		public virtual Vector2 Position { get; set; }

		public abstract void Update(List<GameObject> otherObjects);

		public abstract void Draw(SpriteBatch sb);

		public static GameObject GetObjectFromIndex(int index, List<GameObject> objects)
		{
			var obj = from GameObject o in objects
					  where o.Index == index
					  select o;
			return obj.First();
		}
	}
}
