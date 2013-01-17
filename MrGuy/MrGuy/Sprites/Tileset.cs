using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuy.Sprites
{
	public class Tileset
	{
		private Texture2D tilesetTexture;
		private List<Rectangle> tiles = new List<Rectangle>();
		private int tileWidth, tileHeight, tilesWide, tilesHigh;

		public Tileset(Texture2D tilesetTexture, int tileWidth, int tileHeight, int tilesWide, int tilesHigh)
		{
			this.tilesetTexture = tilesetTexture;
			this.tileWidth = tileWidth;
			this.tileHeight = tileHeight; 
			this.tilesWide = tilesWide;
			this.tilesHigh = tilesHigh;
			CreateRectangles(tilesWide, tilesHigh);
		}

		public Tileset(Texture2D tilesetTexture, List<Rectangle> srcRectangles)
		{
			this.tilesetTexture = tilesetTexture;
			this.tiles = srcRectangles;
		}

		public Texture2D GetTileAsTexture(int index)
		{
			var g = tilesetTexture.GraphicsDevice;
			var ret = new RenderTarget2D(g, tiles[index].Width, tiles[index].Height);
			var sb = new SpriteBatch(g);

			g.SetRenderTarget(ret);
			g.Clear(new Color(0, 0, 0, 0));

			sb.Begin();
			sb.Draw(tilesetTexture, Vector2.Zero, tiles[index], Color.White);
			sb.End();

			g.SetRenderTarget(null); // set back to main window

			return (Texture2D)ret;
		}

		public List<Rectangle> Tiles
		{
			get { return tiles; }
		}
		public Texture2D TilesetTexture
		{
			get { return tilesetTexture; }
		}
		public int TileWidth
		{
			get { return tileWidth; }
		}
		public int TileHeight
		{
			get { return tileHeight; }
		}

		// 0 3 6
		// 1 4 7
		// 2 5 8
		public void CreateRectangles(int tilesWide, int tilesHigh)
		{
			Rectangle rectangle = new Rectangle();
			rectangle.Width = tileWidth;
			rectangle.Height = tileHeight;
			tiles.Clear();
			for (int i = 0; i < tilesWide; i++)
			{
				for (int j = 0; j < tilesHigh; j++)
				{
					rectangle.X = i * tileWidth;
					rectangle.Y = j * tileHeight;
					tiles.Add(rectangle);
				}
			}
		}
	}
}