using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuy.Objects
{
	class CameraBox : GameObject
	{
		private Rectangle area;

		public override Vector2 Position { get; set; }
		public float Priority { get; set; }

		public CameraBox(Vector2 target, Rectangle area, float priority)
		{
			this.Position = target;
			this.area = area;
			this.Priority = priority;
		}

		public override void Update(List<GameObject> otherObjects)
		{
			if (Priority > 1f)
				Priority = 1f;
			else if (Priority < 0f)
				Priority = 0f;
		}

		public bool ContainsObject(GameObject obj)
		{
			return area.Contains(new Point((int)obj.Position.X, (int)obj.Position.Y));
		}

		public override void Draw(SpriteBatch sb)
		{
		}
	}
}
