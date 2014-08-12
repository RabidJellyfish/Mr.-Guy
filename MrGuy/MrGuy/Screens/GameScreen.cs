using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;

using MrGuy.Objects;

namespace MrGuy.Screens
{
	public interface GameScreen
	{
		/// <summary>
		/// Returns the camera used by the game screen
		/// </summary>
		/// <returns>The camera used by the game screen</returns>
		Camera GetCamera();

		/// <summary>
		/// Gets the list of game objects in this screen
		/// </summary>
		/// <returns>The list of game objects in this screen</returns>
		List<GameObject> GetGameObjects();

		/// <summary>
		/// Adds a GameObject to the game world
		/// </summary>
		/// <param name="obj">The object to create</param>
		void CreateObject(GameObject obj);

		/// <summary>
		/// Gets the physics engine's world
		/// </summary>
		/// <returns>The World object used by the physics engine</returns>
		World GetWorld();

		/// <summary>
		/// Runs once every frame, updates the game screen
		/// </summary>
		/// <returns>If the current gamescreen should ever change, this method will return that gamescreen. Otherwise it returns itself</returns>
		GameScreen Update(Game game, GameTime gameTime);

		/// <summary>
		/// Called when the escape key is pressed
		/// </summary>
		/// <returns>The screen to show when the current screen is exited</returns>
		GameScreen Exit();

		/// <summary>
		/// Draws the current game screen
		/// </summary>
		/// <param name="sb">The SpriteBatch used to draw the screen</param>
		void Draw(Game game, SpriteBatch sb, GameTime gameTime);
	}
}
