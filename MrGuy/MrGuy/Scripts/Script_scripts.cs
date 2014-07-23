using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MrGuy.Objects;

namespace MrGuy.Scripts
{
	public partial class Script
	{
		private static PlayerGuy guy;
		public static void test()
		{
			if (guy == null)
				guy = (PlayerGuy)GameObject.GetObjectFromIndex(-1, MainGame.currentScreen.GetGameObjects());
			guy.Position = new Microsoft.Xna.Framework.Vector2();
		}
	}
}
