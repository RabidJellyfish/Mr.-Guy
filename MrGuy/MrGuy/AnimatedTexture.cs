using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuy
{
	class AnimatedTexture
	{
		public bool IsPlaying { get; set; }
		public int Frame { get; set; }
		public int Length { get { return coordinates.Length; } }

		private Texture2D texture;
		private Rectangle[] coordinates;
		private bool playOnce;
		private int timePerFrame, currentTime;

		public AnimatedTexture(Texture2D texture, int numFrames, int startX, int startY, int width, int height, int timePerFrame, bool isPlaying, bool playOnce)
		{
			this.texture = texture;

			// Generate array of rectangles specifying frames in tilesheet
			coordinates = new Rectangle[numFrames];
			int x = startX, y = startY;
			for (int i = 0; i < coordinates.Length; i++)
			{
				if (y + height > texture.Height)
					throw new ArgumentOutOfRangeException("The specified texture does not contain enough frames.");
				coordinates[i] = new Rectangle(x, y, width, height);
				x += width;
				if (x + width > texture.Width)
				{
					x = 0;
					y += height;
				}
			}
			this.IsPlaying = isPlaying;
			this.playOnce = playOnce;
			this.timePerFrame = timePerFrame;
			currentTime = 0;
			Frame = 0;
		}

		public AnimatedTexture(Texture2D texture, int numFrames, int width, int height, int timePerFrame, bool isPlaying, bool playOnce)
			: this(texture, numFrames, 0, 0, width, height, timePerFrame, isPlaying, playOnce)
		{
		}

		public AnimatedTexture(Texture2D texture, int numFrames, int width, int height)
			: this(texture, numFrames, 0, 0, width, height)
		{
		}

		public AnimatedTexture(Texture2D texture, int numFrames, int startX, int startY, int width, int height)
			: this(texture, numFrames, startX, startY, width, height, 1, true, false)
		{
		}

		// Returns whether or not the animation has ended
		public bool Update()
		{
			if (IsPlaying)
			{
				currentTime++;
				if (currentTime > timePerFrame)
				{
					currentTime = 0;
					Frame++;
					if (Frame >= Length)
					{
						if (playOnce)
						{
							IsPlaying = false;
							return true;
						}
						Frame = 0;
					}
				}
			}

			return false;
		}

		public void Draw(SpriteBatch sb, Vector2 position, Color c)
		{
			this.Draw(sb, position, c, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
		}

		public void Draw(SpriteBatch sb, Vector2 position, Color c, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
		{
			sb.Draw(texture, position, coordinates[Frame], c, rotation, origin, scale, effects, layerDepth);
		}
	}
}
