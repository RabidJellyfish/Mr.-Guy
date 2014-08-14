using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;

using MrGuy.Scripts;

namespace MrGuy.Objects
{
	public abstract class GameObject
	{
		public int Index { get; set; }

		public virtual Vector2 Position { get; set; }

		public List<Script> Scripts { get; set; }

		protected World world;

		public GameObject()
		{
			Scripts = new List<Script>();
		}

		public virtual void Update(List<GameObject> otherObjects, GameTime gameTime)
		{
			foreach (Script s in Scripts)
				s.Update(gameTime);
		}

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
