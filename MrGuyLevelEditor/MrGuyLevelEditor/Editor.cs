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

// left click - place things
// shift+left click - change level size
// a/d - rotate
// ctrl+a/d - snap rotate
// w/s - scale
// x - flip horizontally
// ctrl + scroll - zoom
// middle click - pan

namespace MrGuyLevelEditor
{
	public class Editor : Microsoft.Xna.Framework.Game
	{
		public static Texture2D BlankTexture; // White texture
		public static int DScroll; // Amount mouse has been scrolled
		public static GraphicsDeviceManager Graphics; // Stuff
		public static SpriteFont Font; // Font for everything
		
		SpriteBatch spriteBatch;
		private Rectangle levelSize; // When writing to XML file, subtract levelSize.X and levelSize.Y from all of the positions
		private bool changingBounds;
		Camera camera;

		SideBar sideBar;

		private Texture2D selectedTexture;
		private float selTexRotation, selTexScale;
		private SpriteEffects selTexEffect;
		private float layer;

		private bool xPressed;
		private bool rotOnceLeft, rotOnceRight;
		private bool mleftPressed;
		private int prevScrollTotal;
		private int prevMX, prevMY;

		public List<ObjectInformation> objectInfo; // Maybe put in ObjectInformation class

		public Editor()
		{
			Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = 1280;
			Graphics.PreferredBackBufferHeight = 720;

			levelSize = new Rectangle(0, 0, 1280, 720);
			camera = new Camera();

			IsMouseVisible = true;
			prevMX = 0;
			prevMY = 0;
			prevScrollTotal = 0;
			selTexRotation = 0.0f;
			selTexScale = 1.0f;
			selTexEffect = SpriteEffects.None;
			layer = 0.5f;
			xPressed = false;
			mleftPressed = false;
			rotOnceLeft = false;
			rotOnceRight = false;
			objectInfo = new List<ObjectInformation>();
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Font = Content.Load<SpriteFont>("mainfont");
			BlankTexture = new Texture2D(Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			BlankTexture.SetData(new[] { Color.White });

			sideBar = new SideBar(this);
		}

		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			// Set up side bar
			if (sideBar.PageIndex == 0)
			{
				// General functions
			}
			else if (sideBar.PageIndex == 1)
			{
				// Select tiles
				if (sideBar.SelectedButton != null)
					selectedTexture = sideBar.SelectedButton.ForeTexture;
			}
			else if (sideBar.PageIndex == 2)
			{
				// Select physics objects and stuff
			}

			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				if (!mleftPressed && Mouse.GetState().X > (sideBar.Hidden ? 32 : SideBar.WIDTH)) // If the mouse isn't over the panel
				{
					if (Keyboard.GetState().IsKeyUp(Keys.LeftShift)) // Make sure not changing bounds
					{
						if (sideBar.PageIndex == 1)
						{
							ObjectInformation.AddObject(objectInfo, "tile",
														selectedTexture,
														camera.CameraToGlobalPos(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)),
														selTexScale / camera.TotalScale,
														selTexRotation,
														layer,
														selTexEffect);
 
						}
						else if (sideBar.PageIndex == 2)
						{
							// add objects that do stuff instead of just sitting there like tiles
						}
					}
					else
					{
						// Change level size when holding shift
						if (!changingBounds)
						{
							changingBounds = true;
							Vector2 size = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
							levelSize.X = (int)size.X;
							levelSize.Y = (int)size.Y;
						}
						else
						{
							changingBounds = false;
							Vector2 size = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
							levelSize.Width = (int)size.X - levelSize.X;
							levelSize.Height = (int)size.Y - levelSize.Y;
						}
					}
				}
				mleftPressed = true;
			}
			else
				mleftPressed = false;

			sideBar.Update();

			// Rotate selection counterclockwise
			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
				{
					if (!rotOnceLeft)
					{
						selTexRotation = SnapAngle(true);
						rotOnceLeft = true;
					}
				}
				else 
					selTexRotation -= 0.05f;
			}
			// Rotate selection clockwise
			else if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				rotOnceLeft = false;
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
				{
					if (!rotOnceRight)
					{
						selTexRotation = SnapAngle(false);
						rotOnceRight = true;
					}
				}
				else
					selTexRotation += 0.05f;
			}
			else
			{
				rotOnceLeft = false;
				rotOnceRight = false;
			}
			while (selTexRotation < 0)
				selTexRotation += MathHelper.TwoPi;
			while (selTexRotation >= MathHelper.TwoPi)
				selTexRotation -= MathHelper.TwoPi;
			
			// Scale selection
			if (Keyboard.GetState().IsKeyDown(Keys.W))
				selTexScale += 0.05f;
			else if (Keyboard.GetState().IsKeyDown(Keys.S) && selTexScale > 0.05f)
				selTexScale -= 0.05f;

			if (selTexScale < 0.05f)
				selTexScale = 0.05f;

			// Flip selection
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

			// Change selected layer
			Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();
			if (pressedKeys.Count() > 0)
				if (pressedKeys[0] >= Keys.D0 && pressedKeys[0] <= Keys.D9)
					layer = float.Parse(Enum.GetName(typeof(Keys), pressedKeys[0]).Substring(1)) / 9.0f;

			// Zoom
			DScroll = Mouse.GetState().ScrollWheelValue - prevScrollTotal;
			if (DScroll != 0 && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
				camera.Zoom(0.05f * Math.Sign(DScroll));
			prevScrollTotal = Mouse.GetState().ScrollWheelValue;

			// Pan
			if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
				camera.Pan(Mouse.GetState().X - prevMX, Mouse.GetState().Y - prevMY);
			prevMX = Mouse.GetState().X;
			prevMY = Mouse.GetState().Y;

			base.Update(gameTime);
		}

		/// <summary>
		/// Rounds an angle by pi/4
		/// </summary>
		/// <param name="counterClockwise">which direction to round it</param>
		/// <returns>The rounded angle</returns>
		private float SnapAngle(bool counterClockwise)
		{
			if (counterClockwise)
			{
				float rounded = selTexRotation + MathHelper.TwoPi - 0.01f;
				int countRotations = 0;
				while (rounded > MathHelper.PiOver4)
				{
					countRotations++;
					rounded -= MathHelper.PiOver4;
				}

				if (rounded == 0)
					countRotations--;
				return MathHelper.PiOver4 * countRotations;
			}
			else
			{
				float rounded = selTexRotation + 0.01f;
				int countRotations = 0;
				while (rounded <= MathHelper.TwoPi - MathHelper.PiOver4)
				{
					countRotations++;
					rounded += MathHelper.PiOver4;
				}

				if (rounded == MathHelper.TwoPi)
					countRotations--;
				return MathHelper.TwoPi - (MathHelper.PiOver4 * countRotations);
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			// Draw objects
			foreach (ObjectInformation tile in objectInfo)
				spriteBatch.Draw(tile.texture, camera.GlobalToCameraPos(tile.X, tile.Y),
								 null, Color.White, tile.Rotation, new Vector2(tile.texture.Width / 2, tile.texture.Height / 2), tile.Scale * camera.TotalScale, tile.Effect, tile.Layer);

			// Draw collision map

			// Draw components
			Vector2 levelTopLeft = camera.GlobalToCameraPos(levelSize.X, levelSize.Y);
			Rectangle levelSizeSkewed = new Rectangle((int)levelTopLeft.X, (int)levelTopLeft.Y,
													  changingBounds ? Mouse.GetState().X - (int)levelTopLeft.X : (int)(levelSize.Width * camera.TotalScale),
													  changingBounds ? Mouse.GetState().Y - (int)levelTopLeft.Y : (int)(levelSize.Height * camera.TotalScale));
			DrawRectangleOutline(spriteBatch, levelSizeSkewed, Color.Black);
			sideBar.Draw(spriteBatch);
			spriteBatch.DrawString(Font, "Layer: " + layer.ToString("0.00"), new Vector2(Graphics.PreferredBackBufferWidth - 128, Graphics.PreferredBackBufferHeight - 32), Color.Black);
			if (sideBar.PageIndex == 1)
			{
				if (selectedTexture != null)
				{
					MouseState state = Mouse.GetState();
					if (state.X > (sideBar.Hidden ? 32 : SideBar.WIDTH))
						spriteBatch.Draw(selectedTexture, new Vector2(state.X, state.Y), null, new Color(255, 255, 255, 125), selTexRotation,
										 new Vector2(selectedTexture.Width / 2, selectedTexture.Height / 2), selTexScale, selTexEffect, layer);
				}
			}

			// Make sure you include scale values for camera and shit

			// Draw debug stuff
//			spriteBatch.DrawString(Global.Font, "(" + Mouse.GetState().X.ToString() + ", " + Mouse.GetState().Y.ToString() + ")", Vector2.Zero, Color.Black);
//			spriteBatch.DrawString(Font, camera.X.ToString(), Vector2.Zero, Color.Black);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		public static void DrawLine(SpriteBatch sb, Vector2 point1, Vector2 point2, Color c)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);
			sb.Draw(BlankTexture, point1, null, c, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
		}

		public static void DrawRectangleOutline(SpriteBatch sb, Rectangle r, Color c)
		{
			DrawLine(sb, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y), c);
			DrawLine(sb, new Vector2(r.X, r.Y), new Vector2(r.X, r.Y + r.Height), c);
			DrawLine(sb, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height), c);
			DrawLine(sb, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X + r.Width, r.Y + r.Height), c);
		}
	}
}
