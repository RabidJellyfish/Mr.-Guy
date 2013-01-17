using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuyLevelEditor
{
	public class TileInformation : IComparable<TileInformation>
	{
		public Rectangle srcRectangle;
		public int X { get; set; }
		public int Y { get; set; }
		public float Rotation { get; set; }
		public float Scale { get; set; }
		public float Layer { get; set; }
		public SpriteEffects Effect { get; set; }

		public int CompareTo(TileInformation other)
		{
			return this.Layer.CompareTo(other.Layer);
		}
	}
}
