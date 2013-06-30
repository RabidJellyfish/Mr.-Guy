using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using MrGuy.Screens;

namespace MrGuy
{
	public class MainGame : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public const float METER_TO_PIXEL = 140f;
		public const float PIXEL_TO_METER = 1 / 140f;
		public static float RESOLUTION_SCALE = 1f;
		public const float MAX_RES_X = 1920;
		public const float MAX_RES_Y = 1080;

		public static Texture2D blank;
		public static Texture2D texPlayer, texBox;

		GameScreen currentScreen;
		private bool escapePressed;

		public MainGame()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = (int)(MAX_RES_X * RESOLUTION_SCALE);
			graphics.PreferredBackBufferHeight = (int)(MAX_RES_Y * RESOLUTION_SCALE);
			graphics.ApplyChanges();
			escapePressed = true;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			blank.SetData(new[] { Color.White });
			texBox = Content.Load<Texture2D>("objects/box");
			texPlayer = Content.Load<Texture2D>("mrguy");

			currentScreen = new GameWorld(this, "test");
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				if (!escapePressed)
				{
					escapePressed = true;
					currentScreen = currentScreen == null ? null : currentScreen.Exit();
				}
			}
			else
				escapePressed = false;

			currentScreen = currentScreen == null ? null : currentScreen.Update(gameTime);
			if (currentScreen == null)
				this.Exit();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Transform based on camera and resolution
			Camera cam = currentScreen.GetCamera();
			Matrix transformMatrix;
			if (cam != null)
				transformMatrix = Matrix.CreateTranslation(new Vector3(-cam.Position, 0));
			else
				transformMatrix = Matrix.Identity;
			transformMatrix *= Matrix.CreateScale(RESOLUTION_SCALE);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, transformMatrix);
			currentScreen.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}

		public static void DrawLine(SpriteBatch sb, Texture2D b, float width, Color color, Vector2 point1, Vector2 point2)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);
			sb.Draw(b, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
		}
	}
}
