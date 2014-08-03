using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using MrGuyLevelEditor.XMLInfo;

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
		public static Dictionary<string, Texture2D> TileTextures;
		public static Dictionary<string, Texture2D> ObjectTextures;

		SpriteBatch spriteBatch;
		private Rectangle levelSize; // When writing to XML file, subtract levelSize.X and levelSize.Y from all of the positions
		private bool changingBounds;
		private bool creatingObject;

		private bool escapePressed = true;

		Camera camera;
		private int step;
		Vector2[] currentBox;

		Controls controls;
		System.Windows.Forms.Form thisForm;
		bool unfocused;
		RightClickMenu rcMenu;
		List<RightClickMenu.MenuItem> objMenuChoices;

		private Texture2D selectedTexture;
		private float selTexRotation;
		private Vector2 selTexScale;
		private SpriteEffects selTexEffect;
		private float layer;

		private bool xPressed;
		private bool rotOnceLeft, rotOnceRight;
		private bool mleftPressed, mRightPressed;
		private int prevScrollTotal;
		private int prevMX, prevMY;

		public List<TileInformation> tileInfo;
		public List<StaticBodyInformation> sbInfo;
		StaticBodyInformation currentBody;
		public List<ObjectInformation> objInfo;
		public List<CameraBoxInformation> camInfo;
		public List<TriggerInformation> triggerInfo;

		List<int> indices;
		int index;

		#region Creation

		/// <summary>
		/// Editor constructor
		/// </summary>
		public Editor()
		{
			Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = 1280;
			Graphics.PreferredBackBufferHeight = 720;

			levelSize = new Rectangle(0, 0, 1920, 1080);
			camera = new Camera();

			selTexRotation = 0.0f;
			selTexScale = Vector2.One;
			selTexEffect = SpriteEffects.None;
			layer = 0.55555555555555556f;

			IsMouseVisible = true;
			prevMX = 0;
			prevMY = 0;
			prevScrollTotal = 0;

			xPressed = false;
			mleftPressed = false;
			rotOnceLeft = false;
			rotOnceRight = false;
			unfocused = false;
			creatingObject = false;
			step = 1;

			currentBox = new Vector2[2];
			tileInfo = new List<TileInformation>();
			sbInfo = new List<StaticBodyInformation>();
			objInfo = new List<ObjectInformation>();
			camInfo = new List<CameraBoxInformation>();
			triggerInfo = new List<TriggerInformation>();

			indices = new List<int>();
			index = 0;

			objMenuChoices = new List<RightClickMenu.MenuItem>();
			objMenuChoices.Add(new RightClickMenu.MenuItem("Obj ID: ", ""));
			objMenuChoices.Add(new RightClickMenu.MenuItem("Edit fields", "edit"));
			objMenuChoices.Add(new RightClickMenu.MenuItem("Delete", "delete"));
			objMenuChoices.Add(new RightClickMenu.MenuItem("Move", "move"));
			objMenuChoices.Add(new RightClickMenu.MenuItem("Manage Scripts", "scripts"));

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Initializes components
		/// </summary>
		protected override void Initialize()
		{
			controls = new Controls();
			controls.Visible = true;
			base.Initialize();
			thisForm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(this.Window.Handle);
		}

		/// <summary>
		/// Loads textures and sets up editor values
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Font = Content.Load<SpriteFont>("mainfont");
			BlankTexture = new Texture2D(Graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			BlankTexture.SetData(new[] { Color.White });
			BuildTileTextureDictionary();
			BuildObjectsList();
			camera.Zoom(-0.4f);
			camera.Pan(-190, -112);
		}

		/// <summary>
		/// Loads all tile textures from the Content folder
		/// </summary>
		private void BuildTileTextureDictionary()
		{
			TileTextures = new Dictionary<string, Texture2D>();

			string[] files = Directory.GetFiles("Content\\tiles");
			for (int i = 0; i < files.Length; i++)
			{
				string s = files[i].Replace(".xnb", "").Split('\\')[files[i].Split('\\').Length - 1];
				TileTextures.Add(s, Content.Load<Texture2D>("tiles\\" + s));
			}
		}

		/// <summary>
		/// Loads all object textures and object properties
		/// </summary>
		private void BuildObjectsList()
		{
			using (StreamReader reader = new StreamReader("ObjectListItems.txt"))
			{
				List<ObjectListItem> items = new List<ObjectListItem>();
				while (!reader.EndOfStream)
					items.Add(new ObjectListItem(reader.ReadLine()));
				controls.AddItems(items);
				reader.Close();
			}

			// Add to here everytime new object is added to editor
			ObjectTextures = new Dictionary<string, Texture2D>();
			ObjectTextures.Add("MrGuy.Objects.Box", Content.Load<Texture2D>("objects/box"));
			ObjectTextures.Add("MrGuy.Objects.LevelLink", Content.Load<Texture2D>("objects/unknown"));

		}

		#endregion

		/// <summary>
		/// Updates the screen
		/// </summary>
		/// <param name="gameTime">The amount of time passed</param>
		protected override void Update(GameTime gameTime)
		{
			UpdateFocus();

			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				if (!escapePressed)
				{
					escapePressed = true;

					if (controls.CreatingCam)
					{
						controls.CreatingCam = false;
						step = 1;
					}
					else if (controls.CreatingMap)
					{
						controls.CreatingMap = false;
						currentBody = null;
					}
					else if (controls.CreatingTrigger)
					{
						controls.CreatingTrigger = false;
						step = 1;
					}
					else if (changingBounds)
						changingBounds = false;
					else
						this.Exit();
				}
			}
			else
				escapePressed = false;

			if (controls.Tab == 0)
			{
				if (controls.NewPressed)
					NewLevel();
				else if (controls.SavePressed)
					SaveLevel();
				else if (controls.LoadPressed)
					LoadLevel();
				else if (controls.ColDonePressed || controls.CamDonePressed)
					ResetControlButtons();
			}
			else if (controls.Tab == 1)
			{
				if (controls.SelectedTile != null)
					selectedTexture = TileTextures[controls.SelectedTile];
			}
			else if (controls.Tab == 2)
			{
				if (controls.SelectedObject != null)
					selectedTexture = ObjectTextures[controls.SelectedObject.ToString()];
			}

			CheckRightClickMenu();
			UpdateMouseStuff();

			base.Update(gameTime);
		}

		#region Control operations

		/// <summary>
		/// Changes which form has focus based on mouse position
		/// </summary>
		private void UpdateFocus()
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
		}

		/// <summary>
		/// Clears the editor window
		/// </summary>
		private void NewLevel()
		{
			tileInfo.Clear();
			sbInfo.Clear();
			objInfo.Clear();
			camInfo.Clear();
			triggerInfo.Clear();
			indices.Clear();
			index = 0;
			controls.NewPressed = false;
		}

		/// <summary>
		/// Exports the level into an XML file
		/// </summary>
		private void SaveLevel()
		{
			LevelData level = new LevelData();
			level.size = new Vector2(levelSize.Width, levelSize.Height);
			level.R = controls.R;
			level.G = controls.G;
			level.B = controls.B;

			level.tiles = new List<TileInformation>();
			foreach (TileInformation t in tileInfo)
				TileInformation.AddTile(level.tiles, t.texture, new Vector2(t.X - levelSize.X, t.Y - levelSize.Y), t.Scale, t.Rotation, t.Layer, t.Effect);
			level.staticBodies = new List<StaticBodyInformation>();
			foreach (StaticBodyInformation sb in sbInfo)
			{
				StaticBodyInformation body = new StaticBodyInformation();
				foreach (Vector2 p in sb.Points)
					body.AddPoint(new Vector2(p.X - levelSize.X, p.Y - levelSize.Y));
				level.staticBodies.Add(body);
			}
			level.objects = new List<ObjectInformation>();
			foreach (ObjectInformation obj in objInfo)
			{
				ObjectInformation moved = new ObjectInformation();
				moved.ParameterNames = obj.ParameterNames;
				moved.ParameterValues = obj.ParameterValues;
				moved.Position = obj.Position - new Vector2(levelSize.X, levelSize.Y);
				moved.Texture = obj.Texture;
				moved.Type = obj.Type;
				level.objects.Add(moved);
			}
			level.cameras = new List<CameraBoxInformation>();
			foreach (CameraBoxInformation cam in camInfo)
			{
				CameraBoxInformation moved = new CameraBoxInformation(
					cam.Bounds.Left - (int)levelSize.X,
					cam.Bounds.Top - (int)levelSize.Y,
					cam.Bounds.Right - (int)levelSize.X,
					cam.Bounds.Bottom - (int)levelSize.Y,
					cam.Target, cam.Priority);
				level.cameras.Add(moved);
			}
			level.triggers = new List<TriggerInformation>();
			foreach (TriggerInformation trigger in triggerInfo)
			{
				TriggerInformation moved = new TriggerInformation(
					trigger.Bounds.Left - (int)levelSize.X,
					trigger.Bounds.Top - (int)levelSize.Y,
					trigger.Bounds.Right - (int)levelSize.X,
					trigger.Bounds.Bottom - (int)levelSize.Y,
					trigger.Name, trigger.ObjID, trigger.WhenTrigger);
				level.triggers.Add(moved);
			}

			controls.Save(level);

			controls.SavePressed = false;
		}

		/// <summary>
		/// Loads a level from an XML file
		/// </summary>
		private void LoadLevel()
		{
			LevelData level = controls.LoadLevel();
			if (level == null)
				return;

			levelSize = new Rectangle(0, 0, (int)level.size.X, (int)level.size.Y);
			controls.R = level.R;
			controls.G = level.G;
			controls.B = level.B;

			tileInfo = level.tiles;
			sbInfo = level.staticBodies;
			objInfo = level.objects;
			camInfo = level.cameras;
			triggerInfo = level.triggers;

			indices.Clear();
			foreach (ObjectInformation obj in objInfo)
				indices.Add(obj.Index);
			UpdateIndex();

			controls.LoadPressed = false;
		}

		/// <summary>
		/// Resets the buttons in the control window
		/// </summary>
		private void ResetControlButtons()
		{
			if (currentBody != null && currentBody.Count() > 2)
			{
				sbInfo.Add(currentBody);
				currentBody = null;
			}
			step = 1;
			currentBox = new Vector2[2];
			controls.ColDonePressed = false;
			controls.CreatingMap = false;
			controls.CamDonePressed = false;
			controls.CreatingCam = false;
			controls.TriggerDonePressed = false;
			controls.CreatingTrigger = false;
		}

		#endregion

		#region Editor operations

		/// <summary>
		/// Updates mouse-dependent actions
		/// </summary>
		private void UpdateMouseStuff()
		{
			#region Mouse left

			if (Mouse.GetState().LeftButton == ButtonState.Pressed && MouseInBounds())
			{
				if (!mleftPressed)
				{
					if (Keyboard.GetState().IsKeyUp(Keys.LeftShift)) // Make sure not changing bounds
					{
						if (!changingBounds)
						{
							if (controls.Tab == 0)
							{
								if (controls.CreatingMap)
									CreateMap();
								else if (controls.CreatingCam)
									CreateCam();
								else if (controls.CreatingTrigger)
									CreateTrigger();
							}
							else if (controls.Tab == 1 && selectedTexture != null)
								CreateTile();
							else if (controls.Tab == 2 && selectedTexture != null)
								CreateObject();
						}
						else
							FinishChangingBounds();
					}
					else
					{
						if (!changingBounds)
							StartChangingBounds();
						else
							FinishChangingBounds();
					}
				}
				mleftPressed = true;
			}
			else
				mleftPressed = false;

			#endregion

			#region Mouse right

			if (Mouse.GetState().RightButton == ButtonState.Pressed)
			{
				if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
					RemoveThings();
				else if (controls.Tab == 2 && !creatingObject && !mRightPressed)
				{
					bool foundObj = false;
					mRightPressed = true;
					foreach (ObjectInformation obj in objInfo)
					{
						Rectangle boundingBox = new Rectangle((int)camera.GlobalToCameraPos((int)(obj.Position.X - ObjectTextures[obj.Texture].Width * camera.TotalScale / 2), (int)(obj.Position.Y - ObjectTextures[obj.Texture].Height * camera.TotalScale / 2)).X,
															  (int)camera.GlobalToCameraPos((int)(obj.Position.X - ObjectTextures[obj.Texture].Width * camera.TotalScale / 2), (int)(obj.Position.Y - ObjectTextures[obj.Texture].Height * camera.TotalScale / 2)).Y,
															  (int)(ObjectTextures[obj.Texture].Width * camera.TotalScale),
															  (int)(ObjectTextures[obj.Texture].Width * camera.TotalScale));
						if (boundingBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
						{
							MouseState state = Mouse.GetState();
							rcMenu = new RightClickMenu(obj, state.X, state.Y, objMenuChoices);
							foundObj = true;
							break;
						}
					}
					if (!foundObj)
						rcMenu = null;
				}
			}
			else
				mRightPressed = false;
			#endregion

			#region Rotating

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

			#endregion

			#region Scaling

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

			#endregion

			#region Flipping

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

			#endregion

			#region Layer

			// Change selected layer
			Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();
			if (pressedKeys.Count() > 0)
				if (pressedKeys[0] >= Keys.D0 && pressedKeys[0] <= Keys.D9)
					layer = float.Parse(Enum.GetName(typeof(Keys), pressedKeys[0]).Substring(1)) / 9.0f;

			#endregion

			#region Camera controls

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

			#endregion
		}

		/// <summary>
		/// Proceeds to the next step of drawing collision polygons
		/// </summary>
		private void CreateMap()
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

		/// <summary>
		/// Proceeds to the next step of creating a camera box
		/// </summary>
		private void CreateCam()
		{
			if (step == 1)
			{
				step++;
				currentBox[0] = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
			}
			else if (step == 2)
			{
				step++;
				currentBox[1] = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
			}
			else
			{
				step = 1;
				camInfo.Add(new CameraBoxInformation(
								(int)currentBox[0].X,
								(int)currentBox[0].Y,
								(int)currentBox[1].X,
								(int)currentBox[1].Y,
								camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y),
								controls.CamPriority));
				currentBox = new Vector2[2];
				controls.CreatingCam = false;
			}
		}

		/// <summary>
		/// Proceeds to the next step of creating a trigger
		/// </summary>
		private void CreateTrigger()
		{
			if (step == 1)
			{
				step++;
				currentBox[0] = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
			}
			else if (step == 2)
			{
				step = 3;
				currentBox[1] = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
				TriggerEditor editor = new TriggerEditor();
				editor.Location = controls.Location;
				System.Windows.Forms.DialogResult result = editor.ShowDialog();
				TriggerInformation info = new TriggerInformation((int)currentBox[0].X, (int)currentBox[0].Y, (int)currentBox[1].X, (int)currentBox[1].Y, editor.Name, editor.ObjectIDs, editor.WhenTrigger);
				triggerInfo.Add(info);
				controls.CreatingTrigger = false;
				step = 1;
			}
		}

		/// <summary>
		/// Adds a tile
		/// </summary>
		private void CreateTile()
		{
			TileInformation.AddTile(tileInfo,
									  controls.SelectedTile,
									  camera.CameraToGlobalPos(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)),
									  selTexScale / camera.TotalScale,
									  selTexRotation,
									  layer,
									  selTexEffect);
		}

		/// <summary>
		/// Adds an object
		/// </summary>
		private void CreateObject()
		{
			if (controls.SelectedObject.Parameters.Length > 0 && !creatingObject)
			{
				creatingObject = true;
				MouseState state = Mouse.GetState();
				ParameterEditor editor = new ParameterEditor();
				editor.Location = controls.Location;
				UpdateIndex();
				indices.Add(index);
				editor.Text = "ID: " + index;
				for (int i = 0; i < controls.SelectedObject.Parameters.Length; i++)
				{
					System.Windows.Forms.Label l = new System.Windows.Forms.Label();
					l.Text = controls.SelectedObject.Parameters[i];
					l.Location = new System.Drawing.Point(10, 10 + i * 30);
					l.Tag = i;
					editor.Controls.Add(l);
					System.Windows.Forms.TextBox t = new System.Windows.Forms.TextBox();
					t.Location = new System.Drawing.Point(130, 10 + i * 30);
					t.Size = new System.Drawing.Size(275, t.Height);
					t.Tag = i;
					editor.Controls.Add(t);
				}
				System.Windows.Forms.DialogResult result = editor.ShowDialog();
				ObjectInformation info = new ObjectInformation();
				info.Type = controls.SelectedObject.Type;
				info.Index = index;
				info.Position = camera.CameraToGlobalPos(new Vector2(state.X, state.Y));
				info.Texture = controls.SelectedObject.ToString();
				info.ParameterNames = editor.ParameterNames;
				info.ParameterValues = editor.ParameterValues;
				objInfo.Add(info);
				creatingObject = false;
			}
		}

		/// <summary>
		/// Updates the index to a new unique number
		/// </summary>
		private void UpdateIndex()
		{
			index = 0;
			while (indices.Contains(index))
				index++;
		}

		/// <summary>
		/// Starts changing level bounds
		/// </summary>
		private void StartChangingBounds()
		{
			changingBounds = true;
			Vector2 size = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
			levelSize.X = (int)size.X;
			levelSize.Y = (int)size.Y;
		}

		/// <summary>
		/// Finishes changing level bounds
		/// </summary>
		private void FinishChangingBounds()
		{
			changingBounds = false;
			Vector2 size = camera.CameraToGlobalPos(Mouse.GetState().X, Mouse.GetState().Y);
			levelSize.Width = (int)size.X - levelSize.X;
			levelSize.Height = (int)size.Y - levelSize.Y;
		}

		/// <summary>
		/// Removes the items selected by the cursor
		/// </summary>
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

					CameraBoxInformation toRemove2 = null;
					foreach (CameraBoxInformation cam in camInfo)
					{
						if ((camToGlob - cam.Target).Length() <= 5)
						{
							toRemove2 = cam;
							break;
						}
					}
					if (toRemove2 != null)
						camInfo.Remove(toRemove2);

					TriggerInformation toRemove3 = null;
					foreach (TriggerInformation trigger in triggerInfo)
					{
						Vector2 topleft = new Vector2(trigger.Bounds.Left, trigger.Bounds.Top);
						Vector2 topright = new Vector2(trigger.Bounds.Right, trigger.Bounds.Top);
						Vector2 bottomleft = new Vector2(trigger.Bounds.Left, trigger.Bounds.Bottom);
						Vector2 bottomright = new Vector2(trigger.Bounds.Right, trigger.Bounds.Bottom);
						if ((camToGlob - topleft).Length() <= 5 || (camToGlob - topright).Length() <= 5 || (camToGlob - bottomleft).Length() <= 5 || (camToGlob - bottomright).Length() <= 5)
						{
							toRemove3 = trigger;
							break;
						}
					}
					if (toRemove3 != null)
						triggerInfo.Remove(toRemove3);
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
					Rectangle boundingBox = new Rectangle((int)camera.GlobalToCameraPos((int)(obj.X - TileTextures[obj.texture].Width / 2 * selTexScale.X), (int)(obj.Y - TileTextures[obj.texture].Height / 2 * selTexScale.Y)).X,
														  (int)camera.GlobalToCameraPos((int)(obj.X - TileTextures[obj.texture].Width / 2 * selTexScale.X), (int)(obj.Y - TileTextures[obj.texture].Height / 2 * selTexScale.Y)).Y,
														  (int)(TileTextures[obj.texture].Width * selTexScale.X * camera.TotalScale),
														  (int)(TileTextures[obj.texture].Width * selTexScale.Y * camera.TotalScale));
					if (boundingBox.Contains((int)translated.X, (int)translated.Y))
						toRemove.Add(obj);
				}
				foreach (TileInformation obj in toRemove)
					tileInfo.Remove(obj);
			}
			else if (controls.Tab == 2)
			{
				List<ObjectInformation> toRemove = new List<ObjectInformation>();
				foreach (ObjectInformation obj in objInfo)
				{
					Rectangle boundingBox = new Rectangle((int)camera.GlobalToCameraPos((int)(obj.Position.X - ObjectTextures[obj.Texture].Width / 2 * selTexScale.X), (int)(obj.Position.Y - ObjectTextures[obj.Texture].Height / 2 * selTexScale.Y)).X,
														  (int)camera.GlobalToCameraPos((int)(obj.Position.X - ObjectTextures[obj.Texture].Width / 2 * selTexScale.X), (int)(obj.Position.Y - ObjectTextures[obj.Texture].Height / 2 * selTexScale.Y)).Y,
														  (int)(ObjectTextures[obj.Texture].Width * selTexScale.X * camera.TotalScale),
														  (int)(ObjectTextures[obj.Texture].Width * selTexScale.Y * camera.TotalScale));
					if (boundingBox.Contains(Mouse.GetState().X, Mouse.GetState().Y))
						toRemove.Add(obj);
				}
				foreach (ObjectInformation obj in toRemove)
				{
					indices.Remove(obj.Index);
					objInfo.Remove(obj);
				}
				UpdateIndex();
			}
		}

		/// <summary>
		/// Checks if the mouse is in the editor window
		/// </summary>
		/// <returns>True if the mouse is in the editor window</returns>
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

		/// <summary>
		/// Checks the right click menu for selected choices
		/// </summary>
		private void CheckRightClickMenu()
		{
			if (rcMenu != null && rcMenu.Visible)
			{
				mleftPressed = true;
				string result = rcMenu.Update(Mouse.GetState());
				if (result != "")
				{
					ObjectInformation obj = rcMenu.SelectedObj;
					if (result == "edit")
					{
						creatingObject = true;
						ParameterEditor editor = new ParameterEditor();
						editor.Location = controls.Location;
						editor.Text = "ID: " + obj.Index.ToString();
						for (int i = 0; i < obj.ParameterNames.Length; i++)
						{
							System.Windows.Forms.Label l = new System.Windows.Forms.Label();
							l.Text = obj.ParameterNames[i];
							l.Location = new System.Drawing.Point(10, 10 + i * 30);
							l.Tag = i;
							editor.Controls.Add(l);
							System.Windows.Forms.TextBox t = new System.Windows.Forms.TextBox();
							t.Location = new System.Drawing.Point(130, 10 + i * 30);
							t.Size = new System.Drawing.Size(275, t.Height);
							t.Text = obj.ParameterValues[i];
							t.Tag = i;
							editor.Controls.Add(t);
						}
						System.Windows.Forms.DialogResult dialogResult = editor.ShowDialog();
						obj.ParameterNames = editor.ParameterNames;
						obj.ParameterValues = editor.ParameterValues;
						creatingObject = false;
					}
					else if (result == "delete")
					{
						indices.Remove(obj.Index);
						objInfo.Remove(obj);
						UpdateIndex();
					}
					else if (result == "move")
					{
						// Probably save all objInfo values in temp variables and go back to stage 1 of create
						// In create, after final step, check if temp variables are null, if they are, show param editor, if not, get values from temp variables
					}
					else if (result == "scripts")
					{
						// Probably similar to parameter editor showing up
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Renders everything in the editor
		/// </summary>
		/// <param name="gameTime">The amount of time passed</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			MouseState state = Mouse.GetState();

			DrawTextures();
			DrawCollisionMaps(state);
			DrawTriggers(state);
			DrawCameraBoxes(state);

			Vector2 levelTopLeft = camera.GlobalToCameraPos(levelSize.X, levelSize.Y);
			DrawLevelBounds(levelTopLeft, state);
			DrawInformation(levelTopLeft, state);

			DrawMouse(state);

			if (rcMenu != null)
				rcMenu.Draw(spriteBatch, state);

			// Make sure you include scale values for camera and shit

			// Draw debug stuff
//			spriteBatch.DrawString(Global.Font, "(" + Mouse.GetState().X.ToString() + ", " + Mouse.GetState().Y.ToString() + ")", Vector2.Zero, Color.Black);
//			spriteBatch.DrawString(Font, camera.X.ToString(), Vector2.Zero, Color.Black);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		#region Drawing

		/// <summary>
		/// Draws all the objects
		/// </summary>
		private void DrawTextures()
		{
			foreach (TileInformation tile in tileInfo)
				tile.Draw(spriteBatch, camera);
			foreach (ObjectInformation obj in objInfo)
				obj.Draw(spriteBatch, camera);
		}

		/// <summary>
		/// Draws all collision polygons
		/// </summary>
		/// <param name="state">The state of the mouse</param>
		private void DrawCollisionMaps(MouseState state)
		{
			foreach (StaticBodyInformation sb in sbInfo)
				sb.Draw(spriteBatch, camera);
			if (currentBody != null)
			{
				currentBody.Draw(spriteBatch, camera);
				DrawLine(spriteBatch, camera.GlobalToCameraPos(currentBody.LastPoint()), new Vector2(state.X, state.Y), Color.Lime);
			}
		}

		/// <summary>
		/// Draws all camera boxes
		/// </summary>
		/// <param name="state">The state of the mouse</param>
		private void DrawCameraBoxes(MouseState state)
		{
			foreach (CameraBoxInformation box in camInfo)
				box.Draw(spriteBatch, camera);

			if (step == 2)
				DrawRectangleOutline(spriteBatch, new Rectangle(
													(int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).X,
													(int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).Y,
													Mouse.GetState().X - (int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).X,
													Mouse.GetState().Y - (int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).Y), Color.Cyan);
			else if (step == 3)
				DrawRectangleOutline(spriteBatch, new Rectangle(
													(int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).X,
													(int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).Y,
													(int)camera.GlobalToCameraPos((int)currentBox[1].X, (int)currentBox[1].Y).X - (int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).X,
													(int)camera.GlobalToCameraPos((int)currentBox[1].X, (int)currentBox[1].Y).Y - (int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).Y), Color.Cyan);
		}

		/// <summary>
		/// Draw all triggers
		/// </summary>
		/// <param name="state">The state of the mouse</param>
		private void DrawTriggers(MouseState state)
		{
			foreach (TriggerInformation trigger in triggerInfo)
				trigger.Draw(spriteBatch, camera, state);

			if (step == 2)
				DrawRectangleOutline(spriteBatch, new Rectangle(
													(int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).X,
													(int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).Y,
													Mouse.GetState().X - (int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).X,
													Mouse.GetState().Y - (int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).Y), Color.Cyan);
			else if (step == 3)
				DrawRectangleOutline(spriteBatch, new Rectangle(
													(int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).X,
													(int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).Y,
													(int)camera.GlobalToCameraPos((int)currentBox[1].X, (int)currentBox[1].Y).X - (int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).X,
													(int)camera.GlobalToCameraPos((int)currentBox[1].X, (int)currentBox[1].Y).Y - (int)camera.GlobalToCameraPos((int)currentBox[0].X, (int)currentBox[0].Y).Y), Color.Cyan);
		}

		/// <summary>
		/// Draws the bounds of the level
		/// </summary>
		/// <param name="levelTopLeft">The position of the level's bounding box</param>
		/// <param name="state">The state of the mouse</param>
		private void DrawLevelBounds(Vector2 levelTopLeft, MouseState state)
		{
			Rectangle levelSizeSkewed = new Rectangle((int)levelTopLeft.X, (int)levelTopLeft.Y,
													  changingBounds ? state.X - (int)levelTopLeft.X : (int)(levelSize.Width * camera.TotalScale),
													  changingBounds ? state.Y - (int)levelTopLeft.Y : (int)(levelSize.Height * camera.TotalScale));
			DrawRectangleOutline(spriteBatch, levelSizeSkewed, Color.Black);
			if (changingBounds)
				spriteBatch.DrawString(Font,
										"<" + ((state.X - (int)levelTopLeft.X) / camera.TotalScale).ToString() +
										", " + ((state.Y - (int)levelTopLeft.Y) / camera.TotalScale).ToString() + ">",
										Vector2.UnitX * state.X + Vector2.UnitY * state.Y, Color.Black);
		}

		/// <summary>
		/// Draws information at the bottom of the screen
		/// </summary>
		/// <param name="levelTopLeft">The position of the level's bounding box</param>
		/// <param name="state">The state of the mouse</param>
		private void DrawInformation(Vector2 levelTopLeft, MouseState state)
		{
			spriteBatch.DrawString(Font, "Mouse: <" + ((state.X - (int)levelTopLeft.X) / camera.TotalScale).ToString("0") +
								   ", " + ((state.Y - (int)levelTopLeft.Y) / camera.TotalScale).ToString("0") + ">",
								   new Vector2(Graphics.PreferredBackBufferWidth - 546, Graphics.PreferredBackBufferHeight - 32), Color.Black);
			spriteBatch.DrawString(Font, "Zoom: x" + camera.TotalScale.ToString("0.00"), new Vector2(Graphics.PreferredBackBufferWidth - 300, Graphics.PreferredBackBufferHeight - 32), Color.Black);
			spriteBatch.DrawString(Font, "Layer: " + layer.ToString("0.00"), new Vector2(Graphics.PreferredBackBufferWidth - 134, Graphics.PreferredBackBufferHeight - 32), Color.Black);
		}

		/// <summary>
		/// Draws mouse related drawings
		/// </summary>
		/// <param name="state">The state of the mouse</param>
		private void DrawMouse(MouseState state)
		{
			if (controls.Tab == 1)
			{
				if (selectedTexture != null)
				{
					spriteBatch.Draw(selectedTexture, new Vector2(state.X, state.Y), null, new Color(255, 255, 255, 125), selTexRotation,
										new Vector2(selectedTexture.Width / 2, selectedTexture.Height / 2), selTexScale, selTexEffect, layer);
				}
			}
			else if (controls.Tab == 2)
			{
				if (selectedTexture != null)
				{
					spriteBatch.Draw(selectedTexture, new Vector2(state.X, state.Y), null, new Color(255, 255, 255, 125), 0f,
										new Vector2(selectedTexture.Width / 2, selectedTexture.Height / 2), camera.TotalScale, SpriteEffects.None, 0.555556f);
				}
			}
		}

		/// <summary>
		/// Draws a line
		/// </summary>
		/// <param name="sb">The sprite batch used to draw the line</param>
		/// <param name="point1">Initial point</param>
		/// <param name="point2">Terminating point</param>
		/// <param name="c">The color of the line</param>
		public static void DrawLine(SpriteBatch sb, Vector2 point1, Vector2 point2, Color c)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);
			sb.Draw(BlankTexture, point1, null, c, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
		}

		/// <summary>
		/// Draws a rectangle
		/// </summary>
		/// <param name="sb">The sprite batch used to draw the rectangle</param>
		/// <param name="r">The rectangle to draw</param>
		/// <param name="c">The color of the rectangle</param>
		public static void DrawRectangleOutline(SpriteBatch sb, Rectangle r, Color c)
		{
			DrawLine(sb, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y), c);
			DrawLine(sb, new Vector2(r.X, r.Y), new Vector2(r.X, r.Y + r.Height), c);
			DrawLine(sb, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height), c);
			DrawLine(sb, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X + r.Width, r.Y + r.Height), c);
		}

		#endregion
	}
}
