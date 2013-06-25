using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;

using Krypton;
using Krypton.Lights;

using MrGuyLevelEditor;

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

			XmlSerializer ser = new XmlSerializer(typeof(LevelData));
			LevelData level;
			using (XmlReader reader = XmlReader.Create("Content\\levels\\" + name + ".xml"))
			{
				level = (LevelData)ser.Deserialize(reader);
			}

			foreach (TileInformation t in level.tiles)
				this.tiles.Add(new Tile(t.texture, new Vector2(t.X, t.Y), t.Scale, t.Rotation, t.Layer, t.Effect));
			foreach (StaticBodyInformation sb in level.staticBodies)
			{
				StaticBody body = new StaticBody(sb.Points);
				body.CreateBody(world);
				this.collisionMap.Add(body);
			}
//			foreach (PhysicsObject obj in level.physicsObjects)
//				this.objects.Add(obj);
		}

		public Camera GetCamera()
		{
			return this.camera;
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
