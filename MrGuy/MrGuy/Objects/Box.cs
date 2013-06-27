using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace MrGuy.Objects
{
	class Box : PhysicsObject
	{
		private Body box;
		private int width, height;

		private Texture2D texture;

		public Box(Vector2 pos, string width, string height)
		{
			this.Position = pos;
			this.width = int.Parse(width);
			this.height = int.Parse(height);
			this.texture = MainGame.texBox;
		}

		public override void Initialize(World w)
		{
			box = BodyFactory.CreateRectangle(w, width * MainGame.PIXEL_TO_METER, height * MainGame.PIXEL_TO_METER, 1.0f);
			box.Position = this.Position * MainGame.PIXEL_TO_METER;
			box.BodyType = BodyType.Dynamic;
		}

		public override void Update()
		{
			this.Position = box.Position * MainGame.METER_TO_PIXEL;
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, this.Position, null, Color.White, box.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(width / texture.Width, height / texture.Height), SpriteEffects.None, 1f);
		}
	}
}
