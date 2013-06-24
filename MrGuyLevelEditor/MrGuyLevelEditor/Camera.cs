using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuyLevelEditor
{
	public class Camera
	{
		private Vector2 center;

		/// <summary>
		/// The center X value of the camera
		/// </summary>
		public int X
		{
			get { return (int)center.X; }
			set { center.X = value; }
		}

		/// <summary>
		/// The center Y value of the camera
		/// </summary>
		public int Y
		{
			get { return (int)center.Y; }
			set { center.Y = value; }
		}

		public float TotalScale { get; set; }

		public Camera()
		{
			center = new Vector2(640, 360);
			TotalScale = 1.0f;
		}

		public Vector2 CameraToGlobalPos(Vector2 cameraPos)
		{
			int x = (int)((cameraPos.X - 640) / TotalScale + center.X);
			int y = (int)((cameraPos.Y - 360) / TotalScale + center.Y);
			return new Vector2(x, y);
		}
		public Vector2 CameraToGlobalPos(int x, int y)
		{
			return CameraToGlobalPos(new Vector2(x, y));
		}

		public Vector2 GlobalToCameraPos(Vector2 globalPos)
		{
			int x = (int)((globalPos.X - center.X) * TotalScale + 640);
			int y = (int)((globalPos.Y - center.Y) * TotalScale + 360);
			return new Vector2(x, y);
		}
		public Vector2 GlobalToCameraPos(int x, int y)
		{
			return GlobalToCameraPos(new Vector2(x, y));
		}

		public void Pan(int dX, int dY)
		{
			this.X -= (int)(dX / TotalScale);
			this.Y -= (int)(dY / TotalScale);
		}

		public void Zoom(float amount)
		{
			TotalScale += amount;
		}
	}
}
