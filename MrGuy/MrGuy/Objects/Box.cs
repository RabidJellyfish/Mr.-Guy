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
		public Body box;
		private float width, height, rotation;

		private Texture2D texture;

		public override Vector2 Position
		{
			get { return box.Position * MainGame.METER_TO_PIXEL; }
			set { box.Position = value * MainGame.PIXEL_TO_METER; }
		}

		public Box(World w, int index, Vector2 pos, string width, string height)
			: this(w, index, pos, width, height, "0")
		{
		}

		public Box(World w, int index, Vector2 pos, string width, string height, string rotation)
			: base()
		{
			this.world = w;
			this.Index = index;
			this.width = float.Parse(width);
			this.height = float.Parse(height);
			this.rotation = float.Parse(rotation);
			CreateBody(w, pos);

			this.texture = MainGame.texBox;
		}

		private void CreateBody(World w, Vector2 pos)
		{
			box = BodyFactory.CreateRectangle(w, width * MainGame.PIXEL_TO_METER, height * MainGame.PIXEL_TO_METER, 0.8f);
			box.Rotation = this.rotation;
			box.BodyType = BodyType.Dynamic;
			box.CollisionCategories = Category.Cat2;
			this.Position = pos;
		}

		public override void Update(List<GameObject> otherObjects, GameTime gameTime)
		{
			
			base.Update(otherObjects, gameTime);
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(texture, this.Position, null, Color.White, box.Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(width / texture.Width, height / texture.Height), SpriteEffects.None, 0.5f);
		}
	}
}
