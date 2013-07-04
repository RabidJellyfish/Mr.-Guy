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
	class Box : GameObject
	{
		private Body box;
		private int width, height;

		private Texture2D texture;

		public override Vector2 Position
		{
			get { return box.Position * MainGame.METER_TO_PIXEL; }
			set { box.Position = value * MainGame.PIXEL_TO_METER; }
		}

		public Box(World w, int index, Vector2 pos, string width, string height)
		{
			this.Index = index;
			this.width = int.Parse(width);
			this.height = int.Parse(height);
			CreateBody(w, pos);

			this.texture = MainGame.texBox;
		}

		private void CreateBody(World w, Vector2 pos)
		{
			box = BodyFactory.CreateRectangle(w, width * MainGame.PIXEL_TO_METER, height * MainGame.PIXEL_TO_METER, 1.0f);
			box.BodyType = BodyType.Dynamic;
			this.Position = pos;
		}

		public override void Update(List<GameObject> otherObjects)
		{
			this.Position = box.Position * MainGame.METER_TO_PIXEL;
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, this.Position, null, Color.White, box.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(width / texture.Width, height / texture.Height), SpriteEffects.None, 1f);
		}
	}
}
