using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MrGuy.Sprites;

namespace MrGuyLevelEditor.Components
{
	class SideBar
	{
		public const int WIDTH = 240;

		public bool Hidden { get; set; }
		public Button SelectedButton { get { return pages[PageIndex].SelectedButton; } }

		// Drawbox for the sidebar
		private Rectangle hitBox;
		private Button sideButton;

		// Tabs
		public int PageIndex { get; set; }
		private Page[] pages;
		private bool tabPressed;

		public SideBar()
		{
			sideButton = new Button(">>", 0, 0, 32, Global.Graphics.PreferredBackBufferHeight);
			hitBox = new Rectangle(0, 0, WIDTH, Global.Graphics.PreferredBackBufferHeight);
			PageIndex = 0;

			pages = new Page[3];

			// File page
			pages[0] = new Page();

			// Tiles page
			pages[1] = new Page();
			Rectangle[] tileData = new Rectangle[] { new Rectangle(0, 0, 64, 64), new Rectangle(64, 0, 64, 32), new Rectangle(64, 32, 64, 32),
													 new Rectangle(0, 64, 32, 32), new Rectangle(32, 64, 32, 32), new Rectangle(0, 96, 32, 32), 
													 new Rectangle(32, 96, 32, 32) };
			Tileset tiles = new Tileset(Global.TilesetTexture, tileData.ToList<Rectangle>());
			for (int i = 0; i < tileData.Length; i++)
				pages[1].Add(new Button(tiles.Tiles[i], 32, 48 + 90 * i, 72, 72));

			// Objects page
			pages[2] = new Page();
		}

		public void Update()
		{
			MouseState mstate = Mouse.GetState();
			Point coordinates = new Point(mstate.X, mstate.Y);

			// Change state of sidebar
			if (sideButton.HitBox.Contains(coordinates))
			{
				if (mstate.LeftButton == ButtonState.Pressed)
					sideButton.State = Button.BState.Selected;
			}
			else
				sideButton.State = Button.BState.Hover;

			if (sideButton.State == Button.BState.Selected)
				Hidden = !Hidden;

			if (Hidden)
			{
				hitBox.Width = 0;
				sideButton.X = 0;
				sideButton.message = ">>";
			}
			else
			{
				hitBox.Width = WIDTH;
				sideButton.X = WIDTH - 32;
				sideButton.message = "<<";
			}

			// Change sidebar page
			if (Keyboard.GetState().IsKeyDown(Keys.Tab))
			{
				if (!tabPressed)
				{
					tabPressed = true;
					PageIndex++;
					if (PageIndex > 2)
						PageIndex = 0;
				}
			}
			else
				tabPressed = false;

			pages[PageIndex].Update();
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(Global.BlankTexture, hitBox, Color.DarkGray);
			sideButton.Draw(sb);
			if (!Hidden)
			{
				sb.DrawString(Global.Font, "File", new Vector2(4, 0), PageIndex == 0 ? Color.White : Color.Black);
				sb.DrawString(Global.Font, "Tiles", new Vector2(70, 0), PageIndex == 1 ? Color.White : Color.Black);
				sb.DrawString(Global.Font, "Objs", new Vector2(148, 0), PageIndex == 2 ? Color.White : Color.Black);
				EditorGUI.DrawLine(sb, new Vector2(0, 32), new Vector2(WIDTH - sideButton.Width, 32), Color.Black);
				EditorGUI.DrawLine(sb, new Vector2(60, 0), new Vector2(60, 32), Color.Black);
				EditorGUI.DrawLine(sb, new Vector2(138, 0), new Vector2(138, 32), Color.Black);
				pages[PageIndex].Draw(sb);
			}
		}

		class Page
		{
			private List<Button> buttons;
			private int scrollValue;
			private int totalButtonHeight;

			private Button selectedButton;
			public Button SelectedButton { get { return selectedButton; } }

			public Page()
			{
				buttons = new List<Button>();
				scrollValue = 0;
				totalButtonHeight = 0;
			}

			public void Add(Button button)
			{
				buttons.Add(button);
				foreach (Button b in buttons)
				{
					if (b.Y + b.Height + 2> totalButtonHeight)
						totalButtonHeight = b.Y + b.Height + 2;
				}
			}

			public void Update()
			{
				MouseState mstate = Mouse.GetState();
				Point coordinates = new Point(mstate.X, mstate.Y);
				Button changedButton = null;
				foreach (Button b in buttons)
				{
					if (b.HitBox.Contains(coordinates))
					{
						if (mstate.LeftButton == ButtonState.Pressed)
						{
							if (b.State == Button.BState.Hover)
							{
								changedButton = b;
								b.State = Button.BState.Held;
								selectedButton = b;
								break;
							}
						}
						else
						{
							if (b.State == Button.BState.Held)
								b.State = Button.BState.Selected;
							else if (b.State == Button.BState.Idle)
								b.State = Button.BState.Hover;
						}
					}
					else
						if (b.State == Button.BState.Hover)
							b.State = Button.BState.Idle;
				}

				if (changedButton != null)
				{
					foreach (Button b in buttons)
					{
						if (b != changedButton)
							b.State = Button.BState.Idle;
					}
				}

				if (totalButtonHeight > Global.Graphics.PreferredBackBufferHeight)
				{
					scrollValue -= Global.DScroll;
					if (scrollValue < 0)
						scrollValue = 0;
					else if (scrollValue + Global.Graphics.PreferredBackBufferHeight > totalButtonHeight + 16)
						scrollValue = totalButtonHeight - Global.Graphics.PreferredBackBufferHeight + 16;
					foreach (Button b in buttons)
						b.Y = b.InitialY - scrollValue;
				}
			}

			public void Draw(SpriteBatch sb)
			{
				foreach (Button b in buttons)
					b.Draw(sb);
			}
		}
	}
}
