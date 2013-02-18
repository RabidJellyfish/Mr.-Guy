using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MrGuyLevelEditor
{
	class Global
	{
		public static Texture2D BlankTexture; // White texture
		public static Texture2D TilesetTexture; // Current tileset
		public static int DScroll; // Amount mouse has been scrolled
		public static GraphicsDeviceManager Graphics; // Stuff
		public static SpriteFont Font; // Font for everything
	}
}
