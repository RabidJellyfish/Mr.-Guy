using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuyLevelEditor
{
	public class ObjectInformation : IComparable<ObjectInformation>
	{
		public string Type { get; set; }
		public Texture2D texture;
		public int X { get; set; }
		public int Y { get; set; }
		public float Rotation { get; set; }
		public Vector2 Scale { get; set; }
		public float Layer { get; set; }
		public SpriteEffects Effect { get; set; }

		public int CompareTo(ObjectInformation other)
		{
			return this.Layer.CompareTo(other.Layer);
		}

		public static void AddObject(List<ObjectInformation> list, Texture2D tex, Vector2 pos, Vector2 scale, float rotation, float layer, SpriteEffects effect)
		{
			ObjectInformation i = new ObjectInformation();
			i.texture = tex;
			i.X = (int)pos.X; 
			i.Y = (int)pos.Y;
			i.Scale = scale;
			i.Rotation = rotation;
			i.Layer = layer;
			i.Effect = effect;

			// Add tile to list sorted by layer
			if (list.Count > 0)
			{
				int id;
				for (id = 0; id < list.Count; id++)
				{
					if (list[id].Layer > i.Layer)
						break;
				}
				if (id < list.Count)
					list.Insert(id, i);
				else
					list.Add(i);
			}
			else
				list.Add(i);
		}
	}
}
