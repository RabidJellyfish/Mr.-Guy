using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace MrGuy
{
	public class StaticBody
	{
		public List<Vector2> vertices;
		Body poly;

		public StaticBody()
		{
			this.vertices = new List<Vector2>();
		}

		public StaticBody(List<Vector2> v)
		{
			this.vertices = v;
		}

		// Make sure this is called for each StaticBody after loading level data
		public void CreateBody(World w)
		{
			Vertices vs = new Vertices();
			foreach (Vector2 vec in vertices)
				vs.Add(vec * MainGame.PIXEL_TO_METER);
			List<Vertices> list = FarseerPhysics.Common.Decomposition.EarclipDecomposer.ConvexPartition(vs);
			poly = BodyFactory.CreateCompoundPolygon(w, list, 1.0f);
			poly.BodyType = BodyType.Static;
		}

		public void DebugDraw(SpriteBatch sb, Camera cam)
		{
			for (int i = 0; i < vertices.Count - 1; i++)
				MainGame.DrawLine(sb, MainGame.blank, 1, Color.Red, vertices[i], vertices[i + 1]);
			MainGame.DrawLine(sb, MainGame.blank, 1, Color.Red, vertices[vertices.Count - 1], vertices[0]);
		}
	}
}
