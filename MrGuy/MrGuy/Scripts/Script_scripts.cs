using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MrGuy.Objects;

namespace MrGuy.Scripts
{
	public partial class Script
	{
		public static void test(string force)
		{
			foreach (GameObject obj in MainGame.currentScreen.GetGameObjects())
				if (obj is Box)
					((Box)obj).box.ApplyForce(Vector2.UnitY * float.Parse(force));
		}

		public static void makeBox()
		{
			MainGame.currentScreen.CreateObject(new Box(MainGame.currentScreen.GetWorld(),
														100 + MainGame.currentScreen.GetGameObjects().Count,
														MainGame.currentScreen.GetGameObjects()[0].Position,
														"32", "32"));
		}
	}
}
