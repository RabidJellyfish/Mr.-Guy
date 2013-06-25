using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

// left click - place things
// right click - (page 1) remove point from polygon  (page 2/3) remove things
// shift+right click - (page 1) remove entire polygon rather than single point
// shift+left click - change level size
// a/d - rotate
// shift+a/d - snap rotate
// ctrl+a/s/d/w - stretch
// ctrl+shift - reset scale
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
		public static Dictionary<string, Texture2D> Textures;

		SpriteBatch spriteBatch;
		private Rectangle levelSize; // When writing to XML file, subtract levelSize.X and levelSize.Y from all of the positions
		private bool changingBounds;
		Camera camera;

		Controls controls;
		System.Windows.Forms.Form thisForm;
		bool unfocused;

		private Texture2D selectedTexture;
		private float selTexRotation;
		private Vector2 selTexScale;
		private SpriteEffects selTexEffect;
		private float layer;

		private bool xPressed;
		private bool rotOnceLeft, rotOnceRight;
		private bool mleftPressed;
		private int prevScrollTotal;
		private int prevMX, prevMY;

		public List<TileInformation> tileInfo;
		public List<StaticBodyInformation> sbInfo;
		StaticBodyInformation currentBody;

		public Editor()
		{
			Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = 1280;
			Graphics.PreferredBackBufferHeight = 720;

			levelSize = new Rectangle(0, 0, 1920, 1080);
			camera = new Camera();

			IsMouseVisible = true;
			prevMX = 0;
			prevMY = 0;
			prevScrollTotal = 0;
			selTexRotation = 0.0f;
			selTexScale = Vector2.One;
			selTexEffect = SpriteEffects.None;
			layer = 0.55555555555555556f;
			xPressed = false;
			mleftPressed = false;
			rotOnceLeft = false;
			rotOnceRight = false;
			unfocused = false;
			tileInfo = new List<TileInformation>();
			sbInfo = new List<StaticBodyInformation>();
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			controls = new Controls();
			controls.Visible = true;
			base.Initialize();
			thisForm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(this.Window.Handle);
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Font = Content.Load<SpriteFont>("mainfont");
			BlankTexture = new Texture2D(Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			BlankTexture.SetData(new[] { Color.White });
			BuildTextureDictionary();
			camera.Zoom(-0.4f);
			camera.Pan(-190, -112);
		}

		// Add to this every time you add a new texture
		// Change so that it adds from list
		private void BuildTextureDictionary()
		{
			Textures = new Dictionary<string, Texture2D>();
			Textures.Add("dirt", Content.Load<Texture2D>("tiles/dirt"));
			Textures.Add("flower1", Content.Load<Texture2D>("tiles/flower1"));
			Textures.Add("flower2", Content.Load<Texture2D>("tiles/flower2"));
			Textures.Add("flower3", Content.Load<Texture2D>("tiles/flower3"));
			Textures.Add("grass", Content.Load<Texture2D>("tiles/grass"));
			Textures.Add("leaves", Content.Load<Texture2D>("tiles/leaves"));
			Textures.Add("tree trunk", Content.Load<Texture2D>("tiles/tree trunk"));
		}

		protected override void Update(GameTime gameTime)
		{
			controls.Location = new System.Drawing.Point(thisForm.Location.X - 207, thisForm.Location.Y);
			if (this.IsActive)
			{
				if (unfocused)
				{
					unfocused = false;
					controls.Focus();
					thisForm.Focus();
				}
			}
			else
				unfocused = true;

			if (controls.HasFocus && MouseInBounds())
			{
				thisForm.Focus();
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			// Set up side bar
			if (controls.Tab == 0)
			{
				if (controls.NewPressed)
				{
					tileInfo.Clear();
					sbInfo.Clear();
					controls.NewPressed = false;
				}
				else if (controls.SavePressed)
				{
					LevelData level = new LevelData();
					level.size = new Vector2(levelSize.Width, levelSize.Height);
					level.tiles = new List<TileInformation>();
					foreach (TileInformation t in tileInfo)
						TileInformation.AddTile(level.tiles, t.texture, new Vector2(t.X - levelSize.X, t.Y - levelSize.Y), t.Scale, t.Rotation, t.Layer, t.Effect);
					level.staticBodies = sbInfo;
					// TODO: Add physics objects

					controls.Save(level);

					controls.SavePressed = false;
				}
				else if (controls.LoadPressed)
				{
					LevelData level = controls.LoadLevel();
					if (level == null)
						return;

					levelSize = new Rectangle(0, 0, (int)level.size.X, (int)level.size.Y);
					tileInfo = level.tiles;
					sbInfo = level.staticBodies;
					// TODO: Add physics objects

					controls.LoadPressed = false;
				}
				else if (controls.DonePressed)
				{
					if (currentBody != null && currentBody.Count() > 2)
					{
						sbInfo.Add(currentBody);
						currentBody = null;
					}
					controls.DonePressed = false;
					controls.CreatingMap = false;
				}
			}
			else if (controls.Tab == 1)
			{
				// Select tiles
				if (controls.SelectedTile != null)
					selectedTexture = Textures[controls.SelectedTile];
			}
			else if (controls.Tab == 2)
			{
				// Select physics objects and stuff
			}

			UpdateMouseStuff();

			base.Update(gameTime);
		}

		private void UpdateMouseStuff()
		{
			if (Mouse.GetState().LeftButton == ButtonState.Pressed && MouseInBounds())
			{
				if (!mleftPressed)
				{
					if (Keyboard.GetState().IsKeyUp(Keys.LeftShift)) // Make sure not changing bounds
					{
						if (!changingBounds)
						{
							// Draw collision polygons
							if (controls.Tab == 0 && controls.CreatingMap)
							{
								if (currentBody == null)
									currentBody = new StaticBodyInformation(camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y));
								else
								{
									bool done = currentBody.AddPoint(camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y));
									if (done)
									{
										sbInfo.Add(currentBody);
										currentBody = null;
										controls.CreatingMap = false;
									}		
								}
							}
							// Add tiles
							else if (controls.Tab == 1 && selectedTexture != null)
							{
								TileInformation.AddTile(tileInfo,
														  controls.SelectedTile,
														  camera.CameraToGlobalPos(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)),
														  selTexScale / camera.TotalScale,
														  selTexRotation,
														  layer,
														  selTexEffect);
							}
							// Add objects
							else if (controls.Tab == 2)
							{
								// add objects that do stuff instead of just sitting there like tiles
							}
						}
						else
						{
							changingBounds = false;
							Vector2 size = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
							levelSize.Width = (int)size.X - levelSize.X;
							levelSize.Height = (int)size.Y - levelSize.Y;
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
					}
				}
				mleftPressed = true;
			}
			else
				mleftPressed = false;

			if (Mouse.GetState().RightButton == ButtonState.Pressed)
			{
				if (!Keyboard.GetState().IsKeyDown(Keys.LeftControl))
					RemoveThings();
				else
				{
					// Drop down menus later
				}
			}

			// Rotate selection counterclockwise
			if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
			{
				if (Keyboard.GetState().IsKeyDown(Keys.A))
				{
					if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
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
					if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
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
			}

			// Scale selection
			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
			{
				if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
					selTexScale.X = selTexScale.Y = 1f;

				if (Keyboard.GetState().IsKeyDown(Keys.D))
					selTexScale.X += 0.1f;
				else if (Keyboard.GetState().IsKeyDown(Keys.A) && selTexScale.Length() > 0.05f)
					selTexScale.X -= 0.1f;
				if (Keyboard.GetState().IsKeyDown(Keys.W))
					selTexScale.Y += 0.1f;
				else if (Keyboard.GetState().IsKeyDown(Keys.S) && selTexScale.Length() > 0.05f)
					selTexScale.Y -= 0.1f;
			}
			else
			{
				if (Keyboard.GetState().IsKeyDown(Keys.W))
					selTexScale *= 1.03f;
				else if (Keyboard.GetState().IsKeyDown(Keys.S) && selTexScale.Length() > 0.05f)
					selTexScale /= 1.03f;
			}

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
		}
		
		private void RemoveThings()
		{
			Vector2 mouseCoords = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			if (controls.Tab == 0)
			{
				Vector2 camToGlob = camera.CameraToGlobalPos(mouseCoords);
				// Remove entire polygon
				if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
				{
					StaticBodyInformation toRemove = null;
					foreach (StaticBodyInformation sb in sbInfo)
					{
						bool brk = false;

						for (int i = 0; i < sb.Points.Count; i++)
							if ((camToGlob - sb.Points[i]).Length() <= 5)
							{
								toRemove = sb;
								brk = true;
								break;
							}

						if (brk)
							break;
					}
					if (toRemove != null)
						sbInfo.Remove(toRemove);
				}
				// Remove a point from the polyon and continue drawing
				else
				{
					StaticBodyInformation toRemove = null;
					foreach (StaticBodyInformation sb in sbInfo)
					{
						bool brk = false;

						for (int i = 0; i < sb.Points.Count; i++)
							if ((camToGlob - sb.Points[i]).Length() <= 5)
							{
								sb.RemovePoint(sb.Points[i]);
								toRemove = sb;
								currentBody = sb;
								controls.CreatingMap = true;
								brk = true;
								break;
							}

						if (brk)
							break;
					}
					if (toRemove != null)
						sbInfo.Remove(toRemove);
				}
			}
			else if (controls.Tab == 1)
			{
				List<TileInformation> toRemove = new List<TileInformation>();
				foreach (TileInformation obj in tileInfo)
				{
					Vector2 translated = mouseCoords - camera.GlobalToCameraPos(obj.X, obj.Y);
					float newAngle = (float)Math.Atan2(translated.Y, translated.X) - obj.Rotation;
					float m = translated.Length();
					translated.X = m * (float)Math.Cos(newAngle) + camera.GlobalToCameraPos(obj.X, obj.Y).X;
					translated.Y = m * (float)Math.Sin(newAngle) + camera.GlobalToCameraPos(obj.X, obj.Y).Y;
					Rectangle boundingBox = new Rectangle((int)camera.GlobalToCameraPos((int)(obj.X - Textures[obj.texture].Width / 2 * selTexScale.X), (int)(obj.Y - Textures[obj.texture].Height / 2 * selTexScale.Y)).X,
														  (int)camera.GlobalToCameraPos((int)(obj.X - Textures[obj.texture].Width / 2 * selTexScale.X), (int)(obj.Y - Textures[obj.texture].Height / 2 * selTexScale.Y)).Y,
														  (int)(Textures[obj.texture].Width * selTexScale.X * camera.TotalScale),
														  (int)(Textures[obj.texture].Width * selTexScale.Y * camera.TotalScale));
					if (boundingBox.Contains((int)translated.X, (int)translated.Y))
						toRemove.Add(obj);
				}
				foreach (TileInformation obj in toRemove)
					tileInfo.Remove(obj);
			}
			else if (controls.Tab == 2)
			{
				// Remove objects
			}
		}

		private bool MouseInBounds()
		{
			return Mouse.GetState().X >= 0 && Mouse.GetState().X <= Graphics.PreferredBackBufferWidth &&
				   Mouse.GetState().Y >= 0 && Mouse.GetState().Y <= Graphics.PreferredBackBufferHeight;
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
			MouseState state = Mouse.GetState();

			// Draw objects
			foreach (TileInformation tile in tileInfo)
				tile.Draw(spriteBatch, camera);

			// Draw collision map
			foreach (StaticBodyInformation sb in sbInfo)
				sb.Draw(spriteBatch, camera);
			if (currentBody != null)
			{
				currentBody.Draw(spriteBatch, camera);
				DrawLine(spriteBatch, camera.GlobalToCameraPos(currentBody.LastPoint()), new Vector2(state.X, state.Y), Color.Lime);
			}

			// Draw level bounds
			Vector2 levelTopLeft = camera.GlobalToCameraPos(levelSize.X, levelSize.Y);
			Rectangle levelSizeSkewed = new Rectangle((int)levelTopLeft.X, (int)levelTopLeft.Y,
													  changingBounds ? state.X - (int)levelTopLeft.X : (int)(levelSize.Width * camera.TotalScale),
													  changingBounds ? state.Y - (int)levelTopLeft.Y : (int)(levelSize.Height * camera.TotalScale));
			DrawRectangleOutline(spriteBatch, levelSizeSkewed, Color.Black);
			if (changingBounds)
				spriteBatch.DrawString(Font, 
										"<" + ((state.X - (int)levelTopLeft.X) / camera.TotalScale).ToString() + 
										", " + ((state.Y - (int)levelTopLeft.Y) / camera.TotalScale).ToString() + ">",
										Vector2.UnitX * state.X + Vector2.UnitY * state.Y, Color.Black);
			
			// Draw camera info
			spriteBatch.DrawString(Font, "Mouse: <" + ((state.X - (int)levelTopLeft.X) / camera.TotalScale).ToString("0") + 
								   ", " + ((state.Y - (int)levelTopLeft.Y) / camera.TotalScale).ToString("0") + ">",
								   new Vector2(Graphics.PreferredBackBufferWidth - 546, Graphics.PreferredBackBufferHeight - 32), Color.Black);
			spriteBatch.DrawString(Font, "Zoom: x" + camera.TotalScale.ToString("0.00"), new Vector2(Graphics.PreferredBackBufferWidth - 300, Graphics.PreferredBackBufferHeight - 32), Color.Black);
			spriteBatch.DrawString(Font, "Layer: " + layer.ToString("0.00"), new Vector2(Graphics.PreferredBackBufferWidth - 134, Graphics.PreferredBackBufferHeight - 32), Color.Black);

			// Draw creation selection at mouse
			if (controls.Tab == 1)
			{
				if (selectedTexture != null)
				{
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
