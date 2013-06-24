using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;

using Krypton;
using Krypton.Lights;

namespace MrGuy
{
	class GameWorld : GameScreen
	{
		World world;
		KryptonEngine lighting;
		Camera camera;
		Vector2 size;

		List<Tile> tiles;
		List<StaticBody> collisionMap;
		List<PhysicsObject> objects;

		Dictionary<string, Texture2D> textures;

		public GameWorld(Game game, string name, string type)
		{
			world = new World(9.8f * Vector2.UnitY);
			lighting = new KryptonEngine(game, "KryptonEffect");
			lighting.Initialize();

			tiles = new List<Tile>();
			collisionMap = new List<StaticBody>();
			objects = new List<PhysicsObject>();

			Load(game, name, type);
			camera = new Camera(Vector2.Zero, size);
		}

		private void Load(Game game, string name, string type)
		{
			// Load tiles for world type
			textures = new Dictionary<string, Texture2D>();
			string[] files = Directory.GetFiles("Content\\tiles\\" + type);
			for (int i = 0; i < files.Length; i++)
			{
				string s = files[i].Replace(".xnb", "").Split('\\')[files[i].Split('\\').Length - 1];
				textures.Add(s, game.Content.Load<Texture2D>("tiles\\" + type + "\\" + s));
			}

			//
			// Normally you'd use 'name' to load an xml file here for object placement and stuff
			// but I'm not there yet
			// Make a Level object instead of a list of stuff. Why didn't you think of that sooner?
			//
			this.size = new Vector2(2560, 720);
			tiles.Add(new Tile("dirt", Vector2.One * 120, Vector2.One, 0f, 0.5f, SpriteEffects.None));
			tiles.Add(new Tile("flower1", Vector2.UnitX * 2500 + Vector2.UnitY * 120, Vector2.One * 2, MathHelper.PiOver4, 0.4f, SpriteEffects.None));

			List<Vector2> polyList = new List<Vector2>();
			polyList.Add(new Vector2(360, 380));
			polyList.Add(new Vector2(500, 380));
			polyList.Add(new Vector2(500, 100));
			collisionMap.Add(new StaticBody(world, polyList));
		}

		public GameScreen Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Left))
				camera.X -= 10;
			else if (Keyboard.GetState().IsKeyDown(Keys.Right))
				camera.X += 10;
			camera.Update();
			world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
			return this;
		}

		public GameScreen Exit()
		{
			return null;
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (Tile t in tiles)
				t.Draw(sb, textures, camera);
			foreach (StaticBody b in collisionMap)
				b.DebugDraw(sb, camera);
		}
	}
}
