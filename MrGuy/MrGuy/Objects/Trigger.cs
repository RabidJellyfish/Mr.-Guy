using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuy.Objects
{
	public class Trigger
	{
		public static List<string> ActiveTriggers = new List<string>();

		public Rectangle Bounds { get; set; }
		public string Name { get; set; }
		public int[] ObjectID { get; set; }
		public int WhenTrigger { get; set; }

		private bool containsObj;

		public Trigger(Rectangle bounds, string name, int[] objects, int when)
		{
			this.Bounds = bounds;
			this.Name = name;
			this.ObjectID = objects;
			this.WhenTrigger = when;
			containsObj = false;
		}

		public void Update(List<GameObject> objects)
		{
			foreach (int i in ObjectID)
			{
				GameObject obj = GameObject.GetObjectFromIndex(i, objects);
				if (Bounds.Contains((int)obj.Position.X, (int)obj.Position.Y))
				{
					if (WhenTrigger == 0)
						ActiveTriggers.Add(Name);
					else if (WhenTrigger == 1 && !containsObj)
						ActiveTriggers.Add(Name);
					containsObj = true;
				}
				else
				{
					if (WhenTrigger == -1 && containsObj)
						ActiveTriggers.Add(Name);
					containsObj = false;
				}
			}
		}
	}
}
