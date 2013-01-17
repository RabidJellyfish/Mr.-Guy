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

using MrGuyLevelEditor.Components;

namespace MrGuyLevelEditor
{
	public class EditorGUI : Microsoft.Xna.Framework.Game
	{
		SpriteBatch spriteBatch;

		SideBar sideBar;	
		int prevScrollTotal;

		private Texture2D selectedTexture;
		private float selTexRotation, selTexScale;
		private SpriteEffects selTexEffect;
		private float layer;

		private bool xPressed;

		public EditorGUI()
		{
			Global.Graphics = new GraphicsDeviceManager(this);
			Global.Graphics.PreferredBackBufferWidth = 1280;
			Global.Graphics.PreferredBackBufferHeight = 720;
			IsMouseVisible = true;
			prevScrollTotal = 0;
			selTexRotation = 0.0f;
			selTexScale = 1.0f;
			selTexEffect = SpriteEffects.None;
			layer = 0.0f;
			xPressed = false;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Global.Font = Content.Load<SpriteFont>("mainfont");
			Global.BlankTexture = new Texture2D(Global.Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			Global.BlankTexture.SetData(new[] { Color.White });
			Global.TestTexture = Content.Load<Texture2D>("tiles/tileset test");

			sideBar = new SideBar();
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();
			Global.DScroll = Mouse.GetState().ScrollWheelValue - prevScrollTotal;

			sideBar.Update();
			if (sideBar.PageIndex == 0)
			{
			}
			else if (sideBar.PageIndex == 1)
			{
				if (sideBar.SelectedButton != null)
				{
					selectedTexture = sideBar.SelectedButton.ForeTexture;

					if (Keyboard.GetState().IsKeyDown(Keys.A))
						selTexRotation -= 0.05f;
					else if (Keyboard.GetState().IsKeyDown(Keys.D))
						selTexRotation += 0.05f;

					if (Keyboard.GetState().IsKeyDown(Keys.W))
						selTexScale += 0.05f;
					else if (Keyboard.GetState().IsKeyDown(Keys.S))
						selTexScale -= 0.05f;

					if (Keyboard.GetState().IsKeyDown(Keys.X))
					{
						if (!xPressed)
						{
							xPressed = true;
							selTexEffect = (selTexEffect == SpriteEffects.None ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
						}
					}
					else
						xPressed = false;
				}
			}
			else if (sideBar.PageIndex == 2)
			{
			}

			prevScrollTotal = Mouse.GetState().ScrollWheelValue;
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			// Draw debug stuff
//			spriteBatch.DrawString(Global.Font, "(" + Mouse.GetState().X.ToString() + ", " + Mouse.GetState().Y.ToString() + ")", Vector2.Zero, Color.Black);
//			spriteBatch.DrawString(Global.Font, Global.DScroll.ToString(), Vector2.Zero, Color.Black);

			if (selectedTexture != null)
			{
				MouseState state = Mouse.GetState();
				if (state.X > SideBar.WIDTH)
					spriteBatch.Draw(selectedTexture, new Vector2(state.X, state.Y), null, new Color(255, 255, 255, 125), selTexRotation,
									 new Vector2(selectedTexture.Width / 2, selectedTexture.Height / 2), selTexScale, selTexEffect, layer);
			}

			// Draw components
			sideBar.Draw(spriteBatch);

			// Draw tiles
			// Draw objects
			// Draw collision map
			// Make sure you include scale values and shit

			spriteBatch.End();

			base.Draw(gameTime);
		}

		public static void DrawLine(SpriteBatch sb, Vector2 point1, Vector2 point2, Color c)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);
			sb.Draw(Global.BlankTexture, point1, null, c, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
		}
	}
}
