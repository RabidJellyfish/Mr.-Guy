using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MrGuyLevelEditor.XMLInfo
{
	public class TriggerInformation
	{
		public Rectangle Bounds;
		public string Name;
		public int[] ObjID;
		public int WhenTrigger;

		public TriggerInformation()
		{
		}

		public TriggerInformation(int Left, int Top, int Right, int Bottom, string Name, int[] objID, int when)
		{
			this.Bounds = new Rectangle(Left, Top, Right - Left, Bottom - Top);
			this.Name = Name;
			this.ObjID = objID;
			this.WhenTrigger = when;
		}

		public void Draw(SpriteBatch sb, Camera camera, MouseState state)
		{
			Vector2 topLeft = camera.GlobalToCameraPos(Bounds.Left, Bounds.Top);
			Vector2 dimensions = camera.GlobalToCameraPos(Bounds.Right, Bounds.Bottom);
			Rectangle r = new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)dimensions.X - (int)topLeft.X, (int)dimensions.Y - (int)topLeft.Y);
			Editor.DrawRectangleOutline(sb, r, Color.GreenYellow);
			Editor.DrawRectangleOutline(sb, new Rectangle(r.Left - 2, r.Top - 2, 4, 4), Color.GreenYellow);
			Editor.DrawRectangleOutline(sb, new Rectangle(r.Right - 2, r.Top - 2, 4, 4), Color.GreenYellow);
			Editor.DrawRectangleOutline(sb, new Rectangle(r.Left - 2, r.Bottom - 2, 4, 4), Color.GreenYellow);
			Editor.DrawRectangleOutline(sb, new Rectangle(r.Right - 2, r.Bottom - 2, 4, 4), Color.GreenYellow);
			if (Bounds.Contains((int)camera.CameraToGlobalPos(state.X, state.Y).X, (int)camera.CameraToGlobalPos(state.X, state.Y).Y))
				sb.DrawString(Editor.Font, Name, topLeft + Vector2.One, Color.Black);
		}
	}
}
