using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuyLevelEditor
{
	public class ObjectInformation
	{
		public string Type;
		public string Texture;
		public Vector2 Position;
		public string[] Paramaters;

		public void Draw(SpriteBatch sb, Camera camera)
		{
			sb.Draw(Editor.ObjectTextures[this.Texture], camera.GlobalToCameraPos((int)Position.X, (int)Position.Y),
				 null, Color.White, 0f, new Vector2(Editor.ObjectTextures[this.Texture].Width / 2, Editor.ObjectTextures[this.Texture].Height / 2), camera.TotalScale, SpriteEffects.None, 0.5555556f);
		}
	}
}
