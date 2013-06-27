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
		public string[] ParameterValues;
		public string[] ParameterNames;

		public string ValueFromName(string name)
		{
			for (int i = 0; i < ParameterNames.Length; i++)
				if (ParameterNames[i] == name)
					return ParameterValues[i];
			return null;
		}

		public void Draw(SpriteBatch sb, Camera camera)
		{
			sb.Draw(Editor.ObjectTextures[this.Texture],
					camera.GlobalToCameraPos((int)Position.X, (int)Position.Y),
					null,
					Color.White, 
					ParameterNames.Contains("Rotation") ? float.Parse(ValueFromName("Rotation")) : 0f,
					new Vector2(Editor.ObjectTextures[this.Texture].Width / 2, Editor.ObjectTextures[this.Texture].Height / 2),
					camera.TotalScale * ((ParameterNames.Contains("Width") ? int.Parse(ValueFromName("Width")) / Editor.ObjectTextures[this.Texture].Width : 1f) * Vector2.UnitX +
										 (ParameterNames.Contains("Height") ? int.Parse(ValueFromName("Height")) / Editor.ObjectTextures[this.Texture].Height : 1f) * Vector2.UnitY),
					SpriteEffects.None,
					0.5555556f);
		}
	}
}
