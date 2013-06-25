using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuy
{
	public class Tile
	{
		public string texture;
		public int X { get; set; }
		public int Y { get; set; }
		public float Rotation { get; set; }
		public Vector2 Scale { get; set; }
		public float Layer { get; set; }
		public SpriteEffects Effect { get; set; }

		public int CompareTo(Tile other)
		{
			return this.Layer.CompareTo(other.Layer);
		}

		public Tile(string tex, Vector2 pos, Vector2 scale, float rotation, float layer, SpriteEffects effect) 
		{
			this.texture = tex;
			this.X = (int)pos.X;
			this.Y = (int)pos.Y;
			this.Scale = scale;
			this.Rotation = rotation;
			this.Layer = layer;
			this.Effect = effect;
		}

		public void Draw(SpriteBatch sb, Dictionary<string, Texture2D> textures, Camera cam)
		{
			sb.Draw(textures[texture], new Vector2(X, Y), null, Color.White, this.Rotation, new Vector2(textures[texture].Width / 2, textures[texture].Height / 2), this.Scale, this.Effect, this.Layer);
		}
	}
}
