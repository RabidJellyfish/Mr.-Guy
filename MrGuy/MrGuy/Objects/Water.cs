using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuy.Objects
{
	class Water : GameObject
	{
		private float width, height;

		private Texture2D texture;

		private Vector2 pos;
		public override Vector2 Position
		{
			get { return pos * MainGame.METER_TO_PIXEL; }
			set { pos = value * MainGame.PIXEL_TO_METER; }
		}

		public AABB Container { get; set; }

		public Water(World w, int index, Vector2 pos, string width, string height)
			: base()
		{
			this.world = w;
			this.Index = index;
			this.width = float.Parse(width);
			this.height = float.Parse(height);
			this.Position = pos;
			this.Container = new AABB((pos - new Vector2(this.width / 2, this.height / 2)) * MainGame.PIXEL_TO_METER, (pos + new Vector2(this.width / 2, this.height / 2)) * MainGame.PIXEL_TO_METER);
			BuoyancyController buoyancy = new BuoyancyController(Container, 1.0f, 0.3f, 0.3f, w.Gravity);
			buoyancy.AddDisabledCategory(Category.Cat1);
			w.AddController(buoyancy);
			this.texture = MainGame.blank;
		}

		public override void Update(List<GameObject> otherObjects, GameTime gameTime)
		{
			base.Update(otherObjects, gameTime);
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, this.Position, null, new Color(30, 30, 255, 150), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(width / texture.Width, height / texture.Height), SpriteEffects.None, 0f);
		}
	}
}
