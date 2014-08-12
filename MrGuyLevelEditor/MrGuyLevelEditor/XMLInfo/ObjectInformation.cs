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
		public List<ScriptInformation> Scripts;

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

		public string[] GetExtraParameterNames()
		{
			List<string> p = new List<string>();
			for (int i = 0; i < ParameterNames.Length; i++)
			{
				if (ParameterNames[i] != "FacingLeft" && ParameterNames[i] != "Width" && ParameterNames[i] != "Height" && ParameterNames[i] != "Rotation" && ParameterNames[i] != "Scale" &&
						ParameterNames[i] != "Rotation" && ParameterNames[i] != "Radius" && ParameterNames[i] != "Position2")
					p.Add(ParameterNames[i]);
			}
			return p.ToArray();
		}

		public bool HasExtraParameterNames()
		{
			for (int i = 0; i < ParameterNames.Length; i++)
			{
				if (ParameterNames[i] != "FacingLeft" && ParameterNames[i] != "Width" && ParameterNames[i] != "Height" && ParameterNames[i] != "Rotation" && ParameterNames[i] != "Scale" &&
						ParameterNames[i] != "Rotation" && ParameterNames[i] != "Radius" && ParameterNames[i] != "Position2")
					return true;
			}
			return false;
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
					ParameterNames != null && ParameterNames.Contains("Rotation") ? float.Parse(ValueFromName("Rotation")) : 0f,
					new Vector2(Editor.ObjectTextures[this.Texture].Width / 2, Editor.ObjectTextures[this.Texture].Height / 2),
					camera.TotalScale * ((ParameterNames != null && ParameterNames.Contains("Width") ? float.Parse(ValueFromName("Width")) / Editor.ObjectTextures[this.Texture].Width : 1f) * Vector2.UnitX +
										 (ParameterNames != null && ParameterNames.Contains("Height") ? float.Parse(ValueFromName("Height")) / Editor.ObjectTextures[this.Texture].Height : 1f) * Vector2.UnitY),
					SpriteEffects.None,
					0.5555556f);
		}
	}
}
