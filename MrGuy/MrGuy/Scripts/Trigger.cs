using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MrGuy.Objects;

namespace MrGuy.Scripts
{
	public class Trigger
	{
		private static List<string> activeTriggers = new List<string>();

		public Rectangle Bounds { get; set; }
		public string Name { get; set; }
		public int[] ObjectID { get; set; }
		public int WhenTrigger { get; set; }

		private bool containsObj;

		public static bool IsTriggered(string name)
		{
			return activeTriggers.Contains(name);
		}

		public static void Clear()
		{
			activeTriggers.Clear();
		}

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
						activeTriggers.Add(Name);
					else if (WhenTrigger == 1 && !containsObj)
						activeTriggers.Add(Name);
					containsObj = true;
				}
				else
				{
					if (WhenTrigger == -1 && containsObj)
						activeTriggers.Add(Name);
					containsObj = false;
				}
			}
		}
	}
}
