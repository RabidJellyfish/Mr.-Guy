using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Dynamics;

namespace MrGuy.Objects
{
	class PlayerGuy : Guy
	{
		public PlayerGuy(World w, float x, float y, Texture2D texture) : base(w, x, y, texture) 
		{
			this.Index = -1;

			texIdle = new AnimatedTexture(texture, 24, 0, 0, 120, 140);
			texRun = new AnimatedTexture(texture, 19, 0, 140, 120, 140);
			texJump = new AnimatedTexture(texture, 9, 19 * 120, 140, 120, 140, 1, false, false);
			texRoll = new AnimatedTexture(texture, 1, 4 * 120, 280, 120, 140);
			currentTexture = texIdle;
		}

		public override void Update(List<GameObject> otherObjects)
		{
			MovingLeft = Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(0).IsButtonDown(Buttons.DPadLeft);
			MovingRight = Keyboard.GetState().IsKeyDown(Keys.D) || GamePad.GetState(0).IsButtonDown(Buttons.DPadRight);

			Jumping = Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(0).IsButtonDown(Buttons.A);
			Crouching = Keyboard.GetState().IsKeyDown(Keys.LeftControl) || GamePad.GetState(0).IsButtonDown(Buttons.B);

			base.Update(otherObjects);
		}
	}
}
