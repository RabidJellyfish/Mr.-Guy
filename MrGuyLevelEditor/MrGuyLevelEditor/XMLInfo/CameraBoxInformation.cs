using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuyLevelEditor.XMLInfo
{
	public class CameraBoxInformation
	{
		public Vector2 Target;
		public Rectangle Bounds;
		public float Priority;

		public CameraBoxInformation()
		{

		}

		public CameraBoxInformation(int Left, int Top, int Right, int Bottom, Vector2 target, float Priority)
		{
			this.Target = target;
			this.Bounds = new Rectangle(Left, Top, Right - Left, Bottom - Top);
			this.Priority = Priority;
		}

		public void Draw(SpriteBatch sb, Camera camera)
		{
			Vector2 topLeft = camera.GlobalToCameraPos(Bounds.Left, Bounds.Top);
			Vector2 dimensions = camera.GlobalToCameraPos(Bounds.Right, Bounds.Bottom);
			Rectangle r = new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)dimensions.X - (int)topLeft.X, (int)dimensions.Y - (int)topLeft.Y);
			Editor.DrawRectangleOutline(sb, r, Color.Blue);
			Editor.DrawRectangleOutline(sb, new Rectangle(r.Left - 2, r.Top - 2, 4, 4), Color.Blue);
			Editor.DrawRectangleOutline(sb, new Rectangle(r.Right - 2, r.Top - 2, 4, 4), Color.Blue);
			Editor.DrawRectangleOutline(sb, new Rectangle(r.Left - 2, r.Bottom - 2, 4, 4), Color.Blue);
			Editor.DrawRectangleOutline(sb, new Rectangle(r.Right - 2, r.Bottom - 2, 4, 4), Color.Blue);
			Editor.DrawLine(sb, camera.GlobalToCameraPos(Target - Vector2.One * 2), camera.GlobalToCameraPos(Target + Vector2.One * 2), Color.Blue);
			Editor.DrawLine(sb, camera.GlobalToCameraPos(Target + Vector2.UnitX * 2 - Vector2.UnitY * 2), camera.GlobalToCameraPos(Target - Vector2.UnitX * 2 + Vector2.UnitY * 2), Color.Blue);
		}
	}
}
