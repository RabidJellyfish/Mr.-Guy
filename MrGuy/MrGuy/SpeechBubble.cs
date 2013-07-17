using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MrGuy.Objects;

namespace MrGuy
{
	class SpeechBubble
	{
		private int TEXT_BUFFER = 20;
		public static Texture2D texSpeech_Edge, texSpeech_Corner, texSpeech_Arrow;
		public static SpriteFont fntSpeech;

		public string Text { get; set; }
		private GameObject target;
		private int objectDistance;

		private Vector2 topLeft;
		private Vector2 center;
		private Vector2 arrowPosition;

		private int width, height;

		public SpeechBubble(GameObject target, int objectDistance, string text)
		{
			this.Text = text;
			this.target = target;
			this.objectDistance = objectDistance;

			this.width = 2 * TEXT_BUFFER + (int)fntSpeech.MeasureString(text).X;
			this.height = 2 * TEXT_BUFFER + (int)fntSpeech.MeasureString(text).Y;
			topLeft = target.Position - Vector2.UnitX * (objectDistance + width) - Vector2.UnitY * (objectDistance + height);
			center = new Vector2(topLeft.X + (float)width / 2, (float)topLeft.Y + height / 2);
			arrowPosition = center;
		}

		public void Update(Camera cam)
		{
			topLeft = target.Position - Vector2.UnitX * (objectDistance + width) - Vector2.UnitY * (objectDistance + height);
			if (topLeft.X < cam.X)
				topLeft.X = target.Position.X + objectDistance;
			if (topLeft.Y < cam.Y)
				topLeft.Y = target.Position.Y + objectDistance;

			center = new Vector2(topLeft.X + (float)width / 2, (float)topLeft.Y + height / 2);

			float bubbleSlope = (float)height / width;
			Vector2 arrowSlope = new Vector2(target.Position.X - center.X, target.Position.Y - center.Y);
			if (Math.Abs(arrowSlope.Y / arrowSlope.X) < bubbleSlope)
			{
				arrowPosition.X = arrowSlope.X > 0 ? center.X + width / 2f : center.X - width / 2f;
				arrowPosition.Y = center.Y + (arrowSlope.Y / arrowSlope.X) * (arrowPosition.X - center.X);
			}
			else
			{
				arrowPosition.Y = arrowSlope.Y > 0 ? center.Y + height / 2f : center.Y - height / 2f;
				arrowPosition.X = center.X + (arrowSlope.X / arrowSlope.Y) * (arrowPosition.Y - center.Y);
			}
		}

		public void Draw(SpriteBatch sb)
		{
			// Draw outline
//			DrawBase(sb, Vector2.One * 4, Color.Black);
//			DrawBase(sb, Vector2.One * -4, Color.Black);
//			DrawBase(sb, Vector2.UnitX * -4 + Vector2.UnitY * 4, Color.Black);
//			DrawBase(sb, Vector2.UnitX * 4 + Vector2.UnitY * -4, Color.Black);

			// Draw bubble
			sb.Draw(MainGame.blank, new Rectangle((int)topLeft.X + 16, (int)topLeft.Y + 16, width - 32, height - 32), Color.White);
			DrawBase(sb, Vector2.Zero, Color.White);

			// Draw text
			sb.DrawString(fntSpeech, Text, topLeft + Vector2.One * TEXT_BUFFER, Color.Black);
		}

		public void DrawBase(SpriteBatch sb, Vector2 offset, Color c)
		{
			// Draw arrow
			float arrowRotation = (float)Math.Atan2(target.Position.Y - center.Y, target.Position.X - center.X) + 3 * MathHelper.PiOver4;
			sb.Draw(texSpeech_Arrow, arrowPosition, null, c, arrowRotation, Vector2.UnitX * 16 + Vector2.UnitY * 16, 1f, SpriteEffects.None, 0f);

			// Draw edges
			int numHorizontEdges = width > 64 ? (width - 32) / 32 : 0;
			int numVerticalEdges = height > 64 ? (height - 32) / 32 : 0;
			for (int i = 1; i <= numHorizontEdges; i++)
				sb.Draw(texSpeech_Edge, offset + topLeft + Vector2.UnitX * (32 * i), null, c, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			for (int i = 1; i <= numHorizontEdges; i++)
				sb.Draw(texSpeech_Edge, offset + topLeft + Vector2.UnitX * (32 * i + 32) + Vector2.UnitY * height, null, c, MathHelper.Pi, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			for (int j = 1; j <= numVerticalEdges; j++)
				sb.Draw(texSpeech_Edge, offset + topLeft + Vector2.UnitY * (32 * j + 32), null, c, -MathHelper.PiOver2, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			for (int j = 1; j <= numVerticalEdges; j++)
				sb.Draw(texSpeech_Edge, offset + topLeft + Vector2.UnitX * width + Vector2.UnitY * (32 * j), null, c, MathHelper.PiOver2, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			// Draw corners
			sb.Draw(texSpeech_Corner, offset + topLeft, null, c, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			sb.Draw(texSpeech_Corner, offset + topLeft + Vector2.UnitX * (width - 32), null, c, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(texSpeech_Corner, offset + topLeft + Vector2.UnitY * (height - 32), null, c, 0f, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 0f);
			sb.Draw(texSpeech_Corner, offset + topLeft + Vector2.UnitX * (width - 32) + Vector2.UnitY * (height - 32), null, c, 0f, Vector2.Zero, 1f, SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally, 0f);
		}
	}
}
