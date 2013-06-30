using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuy.Screens
{
	interface GameScreen
	{
		/// <summary>
		/// Returns the camera used by the game screen
		/// </summary>
		/// <returns>The camera used by the game screen</returns>
		Camera GetCamera();

		/// <summary>
		/// Runs once every frame, updates the game screen
		/// </summary>
		/// <returns>If the current gamescreen should ever change, this method will return that gamescreen. Otherwise it returns itself</returns>
		GameScreen Update(GameTime gameTime);

		/// <summary>
		/// Called when the escape key is pressed
		/// </summary>
		/// <returns>The screen to show when the current screen is exited</returns>
		GameScreen Exit();

		/// <summary>
		/// Draws the current game screen
		/// </summary>
		/// <param name="sb">The SpriteBatch used to draw the screen</param>
		void Draw(SpriteBatch sb);
	}
}
