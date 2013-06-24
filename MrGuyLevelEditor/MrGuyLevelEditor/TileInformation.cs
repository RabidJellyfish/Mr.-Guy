using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MrGuy;

namespace MrGuyLevelEditor
{
	public class TileInformation : IComparable<TileInformation>
	{
		public string texture;
		public int X { get; set; }
		public int Y { get; set; }
		public float Rotation { get; set; }
		public Vector2 Scale { get; set; }
		public float Layer { get; set; }
		public SpriteEffects Effect { get; set; }

		public int CompareTo(TileInformation other)
		{
			return this.Layer.CompareTo(other.Layer);
		}

		public static void AddTile(List<TileInformation> list, string tex, Vector2 pos, Vector2 scale, float rotation, float layer, SpriteEffects effect)
		{
			TileInformation i = new TileInformation();
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

		public static List<Tile> ConvertToTiles(List<TileInformation> list)
		{
			List<Tile> tiles = new List<Tile>();
			foreach (TileInformation info in list)
				tiles.Add(new Tile(info.texture, new Vector2(info.X, info.Y), info.Scale, info.Rotation, info.Layer, info.Effect));
			return tiles;
		}

		public void Draw(SpriteBatch sb, Camera camera)
		{
			sb.Draw(Editor.Textures[this.texture], camera.GlobalToCameraPos(this.X, this.Y),
				 null, Color.White, this.Rotation, new Vector2(Editor.Textures[this.texture].Width / 2, Editor.Textures[this.texture].Height / 2), this.Scale * camera.TotalScale, this.Effect, this.Layer);
		}
	}
}
