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
	public class EditorGUI : Microsoft.Xna.Framework.Game
	{
		SpriteBatch spriteBatch;
		private Rectangle camera, levelSize; // When writing to XML file, subtract levelSize.X and levelSize.Y from all of the positions
		private Vector2 cameraCenter; // Position of camera
		private float totalScale;
		private bool changingBounds;

		SideBar sideBar;	

		private Rectangle selectedRectangle;
		private float selTexRotation, selTexScale;
		private SpriteEffects selTexEffect;
		private float layer;

		private bool xPressed;
		private bool rotOnceLeft, rotOnceRight;
		private bool mleftPressed;
		private int prevScrollTotal;
		private int prevMX, prevMY;

		public List<TileInformation> tileInfo;

		public EditorGUI()
		{
			Global.Graphics = new GraphicsDeviceManager(this);
			Global.Graphics.PreferredBackBufferWidth = 1280;
			Global.Graphics.PreferredBackBufferHeight = 720;
			camera = new Rectangle(0, 0, 1280, 720);
			cameraCenter = new Vector2(640, 360);
			levelSize = new Rectangle(0, 0, 1280, 720);
			totalScale = 1.0f;
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
			tileInfo = new List<TileInformation>();
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
			Global.TilesetTexture = Content.Load<Texture2D>("tiles/tileset test");

			sideBar = new SideBar();
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
					selectedRectangle = sideBar.SelectedButton.TileInfo;
			}
			else if (sideBar.PageIndex == 2)
			{
				// Select objects
			}

			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				if (!mleftPressed && Mouse.GetState().X > (sideBar.Hidden ? 32 : SideBar.WIDTH))
				{
					if (Keyboard.GetState().IsKeyUp(Keys.LeftShift)) // Make sure not changing bounds
					{
						if (sideBar.PageIndex == 1)
						{
							// Create new tile object
							TileInformation i = new TileInformation();
							i.srcRectangle = selectedRectangle;
							i.X = (int)((Mouse.GetState().X + camera.X - cameraCenter.X) / totalScale + cameraCenter.X);
							i.Y = (int)((Mouse.GetState().Y + camera.Y - cameraCenter.Y) / totalScale + cameraCenter.Y);
							i.Scale = selTexScale / totalScale;
							i.Rotation = selTexRotation;
							i.Layer = layer;
							i.Effect = selTexEffect;

							// Add tile to list sorted by layer
							if (tileInfo.Count > 0)
							{
								int id;
								for (id = 0; id < tileInfo.Count; id++)
								{
									if (tileInfo[id].Layer > i.Layer)
										break;
								}
								if (id < tileInfo.Count)
									tileInfo.Insert(id, i);
								else
									tileInfo.Add(i);
							}
							else
								tileInfo.Add(i);
						}
					}
					else
					{
						// Change level size when holding shift
						if (!changingBounds)
						{
							changingBounds = true;
							levelSize.X = (int)((Mouse.GetState().X + camera.X - cameraCenter.X) / totalScale + cameraCenter.X);
							levelSize.Y = (int)((Mouse.GetState().Y + camera.Y - cameraCenter.Y) / totalScale + cameraCenter.Y);
						}
						else
						{
							changingBounds = false;
							levelSize.Width = (int)((Mouse.GetState().X + camera.X - cameraCenter.X) / totalScale + cameraCenter.X) - levelSize.X;
							levelSize.Height = (int)((Mouse.GetState().Y + camera.Y - cameraCenter.Y) / totalScale + cameraCenter.Y) - levelSize.Y;
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
						float rounded = selTexRotation + MathHelper.TwoPi - 0.01f;
						int countRotations = 0;
						while (rounded > MathHelper.PiOver4)
						{
						    countRotations++;
						    rounded -= MathHelper.PiOver4;
						}

						if (rounded == 0)
						    countRotations--;

						selTexRotation = MathHelper.PiOver4 * countRotations;
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
						float rounded = selTexRotation + 0.01f;
						int countRotations = 0;
						while (rounded <= MathHelper.TwoPi - MathHelper.PiOver4)
						{
							countRotations++;
							rounded += MathHelper.PiOver4;
						}

						if (rounded == MathHelper.TwoPi)
							countRotations--;

						selTexRotation = MathHelper.TwoPi - (MathHelper.PiOver4 * countRotations);
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
			if (selTexRotation < 0)
				selTexRotation += MathHelper.TwoPi;
			else if (selTexRotation >= MathHelper.TwoPi)
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

			// Change zoom
			Global.DScroll = Mouse.GetState().ScrollWheelValue - prevScrollTotal;
			if (Global.DScroll != 0 && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
				totalScale += Global.DScroll > 0 ? 0.05f : -0.05f;
			prevScrollTotal = Mouse.GetState().ScrollWheelValue;

			// Pan
			if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
			{
				camera.X -= (int)((Mouse.GetState().X - prevMX) / totalScale);
				camera.Y -= (int)((Mouse.GetState().Y - prevMY) / totalScale);
			}
			cameraCenter = new Vector2(camera.Width / 2 + camera.X, camera.Height / 2 + camera.Y);
			prevMX = Mouse.GetState().X;
			prevMY = Mouse.GetState().Y;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			// Draw collision map

			// Draw objects

			// Draw tiles
			Vector2 cameraCenter = new Vector2(camera.Width / 2 + camera.X, camera.Height / 2 + camera.Y);
			foreach (TileInformation tile in tileInfo)
				spriteBatch.Draw(Global.TilesetTexture, new Vector2((tile.X - cameraCenter.X) * totalScale + cameraCenter.X - camera.X, (tile.Y - cameraCenter.Y) * totalScale + cameraCenter.Y - camera.Y), 
								 tile.srcRectangle, Color.White, tile.Rotation, new Vector2(tile.srcRectangle.Width / 2, tile.srcRectangle.Height / 2), tile.Scale * totalScale, tile.Effect, tile.Layer);

			// Draw components
			Rectangle levelSizeSkewed = new Rectangle((int)((levelSize.X - cameraCenter.X) * totalScale + cameraCenter.X - camera.X), (int)((levelSize.Y - cameraCenter.Y) * totalScale + cameraCenter.Y - camera.Y),
													  changingBounds ? Mouse.GetState().X - (int)((levelSize.X - cameraCenter.X) * totalScale + cameraCenter.X - camera.X) : (int)(levelSize.Width * totalScale),
													  changingBounds ? Mouse.GetState().Y - (int)((levelSize.Y - cameraCenter.Y) * totalScale + cameraCenter.Y - camera.Y) : (int)(levelSize.Height * totalScale));
			DrawRectangleOutline(spriteBatch, levelSizeSkewed, Color.Black);
			sideBar.Draw(spriteBatch);
			spriteBatch.DrawString(Global.Font, "Layer: " + layer.ToString("0.00"), new Vector2(Global.Graphics.PreferredBackBufferWidth - 128, Global.Graphics.PreferredBackBufferHeight - 32), Color.Black);
			if (sideBar.PageIndex == 1)
			{
				if (selectedRectangle != null)
				{
					MouseState state = Mouse.GetState();
					if (state.X > (sideBar.Hidden ? 32 : SideBar.WIDTH))
						spriteBatch.Draw(Global.TilesetTexture, new Vector2(state.X, state.Y), selectedRectangle, new Color(255, 255, 255, 125), selTexRotation,
										 new Vector2(selectedRectangle.Width / 2, selectedRectangle.Height / 2), selTexScale, selTexEffect, layer);
				}
			}

			// Make sure you include scale values for camera and shit

			// Draw debug stuff
//			spriteBatch.DrawString(Global.Font, "(" + Mouse.GetState().X.ToString() + ", " + Mouse.GetState().Y.ToString() + ")", Vector2.Zero, Color.Black);
//			spriteBatch.DrawString(Global.Font, selTexRotation.ToString(), Vector2.Zero, Color.Black);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		public static void DrawLine(SpriteBatch sb, Vector2 point1, Vector2 point2, Color c)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);
			sb.Draw(Global.BlankTexture, point1, null, c, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
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
