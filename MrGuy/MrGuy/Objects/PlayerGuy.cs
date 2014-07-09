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
		public static Keys Key_Left = Keys.A;
		public static Keys Key_Right = Keys.D;
		public static Keys Key_Jump = Keys.W;
		public static Keys Key_Crouch = Keys.LeftControl;

		public static Buttons Btn_Left = Buttons.DPadLeft;
		public static Buttons Btn_Right = Buttons.DPadRight;
		public static Buttons Btn_Jump = Buttons.A;
		public static Buttons Btn_Crouch = Buttons.B;

		public PlayerGuy(World w, float x, float y, Texture2D texture) : base(w, x, y, texture) 
		{
			this.Index = -1;
		}

		public override void Update(List<GameObject> otherObjects)
		{
			MovingLeft = Keyboard.GetState().IsKeyDown(Key_Left) || GamePad.GetState(0).IsButtonDown(Btn_Left);
			MovingRight = Keyboard.GetState().IsKeyDown(Key_Right) || GamePad.GetState(0).IsButtonDown(Btn_Right);

			Jumping = Keyboard.GetState().IsKeyDown(Key_Jump) || GamePad.GetState(0).IsButtonDown(Btn_Jump);
			Crouching = Keyboard.GetState().IsKeyDown(Key_Crouch) || GamePad.GetState(0).IsButtonDown(Btn_Crouch);

			base.Update(otherObjects);
		}
	}
}
