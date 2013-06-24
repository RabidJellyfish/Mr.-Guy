using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MrGuy;

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

		public SideBar(Game game)
		{
			sideButton = new Button(true, ">>", 0, 0, 32, Editor.Graphics.PreferredBackBufferHeight);
			hitBox = new Rectangle(0, 0, WIDTH, Editor.Graphics.PreferredBackBufferHeight);
			PageIndex = 0;

			pages = new Page[3];

			// File page
			pages[0] = new Page();

			pages[0].Add(new Button(true, "New", 21, 64));
			pages[0].Add(new Button(true, "Save", 71, 64));
			pages[0].Add(new Button(true, "Load", 132, 64));

			// Tiles page
			pages[1] = new Page();

			string[] files = Directory.GetFiles("Content\\tiles");
			foreach (string f in files)
				Console.WriteLine(f);
			List<string> tiles = new List<string>();
			for (int i = 0; i < files.Length; i++)
				tiles.Add(files[i].Replace(".xnb", "").Split('\\')[files[i].Split('\\').Length - 1]);
			
			for (int i = 0; i < tiles.Count; i++)
				pages[1].Add(new Button(tiles[i], 24 + (i % 2) * 86, 48 + 90 * (int)(i / 2), 72, 72));

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

		public void DeselectButton()
		{
			pages[PageIndex].DeselectButton();
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(Editor.BlankTexture, hitBox, Color.DarkGray);
			sideButton.Draw(sb);
			if (!Hidden)
			{
				sb.DrawString(Editor.Font, "Stuff", new Vector2(4, 0), PageIndex == 0 ? Color.White : Color.Black);
				sb.DrawString(Editor.Font, "Tiles", new Vector2(75, 0), PageIndex == 1 ? Color.White : Color.Black);
				sb.DrawString(Editor.Font, "Objs", new Vector2(148, 0), PageIndex == 2 ? Color.White : Color.Black);
				Editor.DrawLine(sb, new Vector2(0, 32), new Vector2(WIDTH - sideButton.Width, 32), Color.Black);
				Editor.DrawLine(sb, new Vector2(67, 0), new Vector2(67, 32), Color.Black);
				Editor.DrawLine(sb, new Vector2(138, 0), new Vector2(138, 32), Color.Black);
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

			public void DeselectButton()
			{
				selectedButton.Deselect();
				selectedButton = null;
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

				if (totalButtonHeight > Editor.Graphics.PreferredBackBufferHeight)
				{
					if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
					{
						scrollValue -= Editor.DScroll;
						if (scrollValue < 0)
							scrollValue = 0;
						else if (scrollValue + Editor.Graphics.PreferredBackBufferHeight > totalButtonHeight + 16)
							scrollValue = totalButtonHeight - Editor.Graphics.PreferredBackBufferHeight + 16;
						foreach (Button b in buttons)
							b.Y = b.InitialY - scrollValue;
					}
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
