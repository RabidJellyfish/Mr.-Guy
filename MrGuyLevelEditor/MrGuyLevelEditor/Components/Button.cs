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
	class Button
	{
		// Button structure
		private Rectangle hitBox;

		// Visual info
		private Texture2D foreTexture, backTexture;
		public string message { get; set; }
		public Texture2D ForeTexture { get { return foreTexture; } }

		// Button state
		public enum BState
		{
			Idle,
			Hover,
			Held,
			Selected
		}
		public BState State { get; set; }
		private Color c_idle = new Color(120, 120, 120, 140);
		private Color c_hover = new Color(180, 180, 180, 200);
		private Color c_press = new Color(235, 235, 235, 255);

		// Position/size properties
		private int initialX, initialY;
		public int InitialX { get { return initialX; } }
		public int InitialY { get { return initialY; } }
		public int X
		{
			get { return hitBox.X; }
			set { hitBox.X = value; }
		}
		public int Y
		{
			get { return hitBox.Y; }
			set { hitBox.Y = value; }
		}
		public int Width
		{
			get { return hitBox.Width; }
			set { hitBox.Width = value; }
		}
		public int Height
		{
			get { return hitBox.Height; }
			set { hitBox.Height = value; }
		}
		public Rectangle HitBox { get { return hitBox; } }

		// Constructors
		public Button()
		{
			State = BState.Idle;
			backTexture = Editor.BlankTexture;
		}

		public Button(Texture2D texture, int x, int y)
			: this(texture, x, y, texture.Width + 8, texture.Height + 8)
		{
		}
		public Button(Texture2D texture, int x, int y, int width, int height)
			: this()
		{
			this.hitBox = new Rectangle(x, y, width, height);
			initialX = X;
			initialY = Y;
			this.foreTexture = texture;
		}

		public Button(string message, int x, int y)
			: this(message, x, y, (int)Editor.Font.MeasureString(message).X + 8, (int)Editor.Font.MeasureString(message).Y)
		{
		}
		public Button(string message, int x, int y, int width, int height)
			: this()
		{
			this.message = message;
			this.hitBox = new Rectangle(x, y, width, height);
			initialX = X;
			initialY = Y;
		}

		public void Deselect()
		{
			this.State = BState.Idle;
		}

		// Draw method
		public void Draw(SpriteBatch sb)
		{			
			// Change back color based on state
			Color backColor = Color.Black;
			switch (State)
			{
				case BState.Idle:
					backColor = c_idle;
					break;
				case BState.Hover:
					backColor = c_hover;
					break;
				case BState.Held:
					backColor = c_press;
					break;
				case BState.Selected:
					backColor = c_press;
					break;
			}
			sb.Draw(backTexture, hitBox, backColor);

			// Show button foreground
			if (foreTexture != null)
			{
				Rectangle drawBox = new Rectangle(X + (Width - foreTexture.Width) / 2, Y + (Height - foreTexture.Height) / 2, foreTexture.Width, foreTexture.Height);
				sb.Draw(foreTexture, drawBox, Color.White);
			}
			else if (message != null)
			{
				Vector2 drawPos = new Vector2(X + (Width - (int)Editor.Font.MeasureString(message).X) / 2, Y + (Height - (int)Editor.Font.MeasureString(message).Y) / 2);
				sb.DrawString(Editor.Font, message, drawPos, (State != BState.Idle) ? Color.Black : Color.White);
			}
		}
	}
}
