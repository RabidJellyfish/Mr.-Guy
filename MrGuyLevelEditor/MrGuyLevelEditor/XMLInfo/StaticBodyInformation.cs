using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuyLevelEditor.XMLInfo
{
	public class StaticBodyInformation
	{
		List<Vector2> points;
		public List<Vector2> Points { get { return points; } }

		public StaticBodyInformation()
		{
			points = new List<Vector2>();
		}

		public StaticBodyInformation(Vector2 firstPoint)
		{
			points = new List<Vector2>();
			points.Add(firstPoint);
		}

		/// <summary>
		/// Adds a point to the structure
		/// </summary>
		/// <param name="newPoint">The point to add in global coordinates</param>
		/// <returns>True if a loop has been made</returns>
		public bool AddPoint(Vector2 newPoint)
		{
			bool contains = false;
			foreach (Vector2 v in points)
				if ((newPoint - v).Length() <= 5)
				{
					contains = true;
					break;
				}

			if (!contains)
				points.Add(newPoint);

			return contains && points.Count > 2;
		}

		public void RemovePoint(Vector2 point)
		{
			int index = points.IndexOf(point);
			List<Vector2> newPoints = new List<Vector2>();
			for (int i = index + 1; i < points.Count; i++)
				newPoints.Add(points[i]);
			for (int i = 0; i < index; i++)
				newPoints.Add(points[i]);
			points = newPoints;
		}

		public int Count()
		{
			return points.Count;
		}

		public Vector2 LastPoint()
		{
			return points[points.Count - 1];
		}

		public void Draw(SpriteBatch sb, Camera camera)
		{
			if (points.Count > 1)
			{
				for (int i = 0; i < points.Count - 1; i++)
				{
					Editor.DrawRectangleOutline(sb, new Rectangle((int)camera.GlobalToCameraPos(points[i]).X - 2, (int)camera.GlobalToCameraPos(points[i]).Y - 2, 4, 4), Color.Red);
					Editor.DrawLine(sb, camera.GlobalToCameraPos(points[i]), camera.GlobalToCameraPos(points[i + 1]), Color.Red);
				}
				Editor.DrawRectangleOutline(sb, new Rectangle((int)camera.GlobalToCameraPos(points[points.Count - 1]).X - 2, (int)camera.GlobalToCameraPos(points[points.Count - 1]).Y - 2, 4, 4), Color.Red);
				Editor.DrawLine(sb, camera.GlobalToCameraPos(points[points.Count - 1]), camera.GlobalToCameraPos(points[0]), Color.Red);
			}
		}
	}
}
