using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuyLevelEditor.XMLInfo
{
	public class ObjectInformation
	{
		public string Type;
		public int Index;
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

		public void SetParameter(string name, object value)
		{
			for (int i = 0; i < ParameterNames.Length; i++)
			{
				if (ParameterNames[i] == name)
				{
					ParameterValues[i] = value.ToString();
					return;
				}
			}
		}

		public void Draw(SpriteBatch sb, Camera camera)
		{
			Draw(sb, camera, Color.White);
		}

		public void Draw(SpriteBatch sb, Camera camera, Color c)
		{
			sb.Draw(Editor.ObjectTextures[this.Texture],
					camera.GlobalToCameraPos((int)Position.X, (int)Position.Y),
					null,
					c, 
					ParameterNames.Contains("Rotation") ? float.Parse(ValueFromName("Rotation")) : 0f,
					new Vector2(Editor.ObjectTextures[this.Texture].Width / 2, Editor.ObjectTextures[this.Texture].Height / 2),
					camera.TotalScale * ((ParameterNames.Contains("Width") ? float.Parse(ValueFromName("Width")) / Editor.ObjectTextures[this.Texture].Width : 1f) * Vector2.UnitX +
										 (ParameterNames.Contains("Height") ? float.Parse(ValueFromName("Height")) / Editor.ObjectTextures[this.Texture].Height : 1f) * Vector2.UnitY),
					SpriteEffects.None,
					0.5555556f);
		}
	}
}
