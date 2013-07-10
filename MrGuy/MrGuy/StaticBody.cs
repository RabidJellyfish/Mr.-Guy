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

using Krypton;

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
		public void CreateBody(World w, KryptonEngine lightEngine)
		{
			Vertices vs = new Vertices();
			Vector2[] points = new Vector2[this.vertices.Count];
			for (int i = 0; i < vertices.Count; i++)
			{
				vs.Add(vertices[i] * MainGame.PIXEL_TO_METER);
				points[i] = vertices[i];
			}
			List<Vertices> list = FarseerPhysics.Common.Decomposition.EarclipDecomposer.ConvexPartition(vs);
			poly = BodyFactory.CreateCompoundPolygon(w, list, 1.0f);
			poly.BodyType = BodyType.Static;

			foreach (Vertices v in list)
			{
				Vector2[] p = new Vector2[v.Count];
				for (int i = 0; i < p.Length; i++)
					p[i] = v[p.Length - i - 1] * MainGame.METER_TO_PIXEL;
				lightEngine.Hulls.Add(ShadowHull.CreateConvex(ref p));
			}
		}

		public void DebugDraw(SpriteBatch sb)
		{
			for (int i = 0; i < vertices.Count - 1; i++)
				MainGame.DrawLine(sb, MainGame.blank, 1, Color.Red, vertices[i], vertices[i + 1]);
			MainGame.DrawLine(sb, MainGame.blank, 1, Color.Red, vertices[vertices.Count - 1], vertices[0]);
		}
	}
}
