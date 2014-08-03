using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using MrGuyLevelEditor.XMLInfo;

namespace MrGuyLevelEditor
{
	class RightClickMenu
	{
		private List<MenuItem> items;
		public int X { get; set; }
		public int Y { get; set; }
		public bool Visible { get; set; }
		public ObjectInformation SelectedObj { get; set; }

		private const int itemWidth = 256;
		private const int itemHeight = 30;

		private bool leftMousePressed;

		public RightClickMenu(ObjectInformation selectedObj, int x, int y, List<MenuItem> choices)
		{
			this.X = x;
			this.Y = y;
			this.Visible = true;
			this.SelectedObj = selectedObj;
			items = choices;
			items[0].Text = "Obj index: " + selectedObj.Index.ToString();
			items[0].Value = "";
			leftMousePressed = true;
		}

		private Rectangle GetItemBox(int index)
		{
			return new Rectangle(X, Y + itemHeight * index, itemWidth, itemHeight);
		}

		public string Update(MouseState state)
		{
			if (Visible)
			{
				if (state.LeftButton == ButtonState.Pressed)
				{
					if (!leftMousePressed)
					{
						this.Visible = false;
						leftMousePressed = true;
						for (int i = 0; i < items.Count; i++)
							if (GetItemBox(i).Contains(state.X, state.Y))
								return items[i].Value;
					}
				}
				else
					leftMousePressed = false;
			}

			return "";
		}

		public void Draw(SpriteBatch sb, MouseState state)
		{
			if (Visible)
			{
				sb.Draw(Editor.BlankTexture, new Rectangle(X, Y, itemWidth, itemHeight * items.Count), Color.LightGray);
				for (int i = 0; i < items.Count; i++)
				{
					sb.DrawString(Editor.Font, items[i].Text, GetItemBox(i).X * Vector2.UnitX + GetItemBox(i).Y * Vector2.UnitY, Color.Black);
					if (GetItemBox(i).Contains(state.X, state.Y))
						Editor.DrawRectangleOutline(sb, GetItemBox(i), Color.Black);
				}
			}
		}

		public class MenuItem
		{
			public string Text { get; set; }
			public string Value { get; set; }

			public MenuItem(string text, string value)
			{
				this.Text = text;
				this.Value = value;
			}
		}
	}
}
