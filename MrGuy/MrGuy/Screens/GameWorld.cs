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
using MrGuyLevelEditor.XMLInfo;
using MrGuy.Objects;

namespace MrGuy.Screens
{
	class GameWorld : GameScreen
	{
		World world;
		KryptonEngine globalLighting;
		Camera camera;
		Vector2 size;

		List<Tile> tiles;
		List<StaticBody> collisionMap;
		List<GameObject> objects;

		PlayerGuy player;

		Dictionary<string, Texture2D> textures;

		public GameWorld(Game game, string name)
		{
			world = new World(9.8f * Vector2.UnitY);
			globalLighting = new KryptonEngine(game, "KryptonEffect");
			globalLighting.Initialize();

			tiles = new List<Tile>();
			collisionMap = new List<StaticBody>();
			objects = new List<GameObject>();

			Load(game, name);
			player = new PlayerGuy(world, 100, 200, MainGame.texPlayer);
			camera = new Camera(Vector2.Zero, size, 15f, player);
		}

		private void Load(Game game, string name)
		{
			// Load tiles for world type
			textures = new Dictionary<string, Texture2D>();
			string[] files = Directory.GetFiles("Content\\tiles");
			for (int i = 0; i < files.Length; i++)
			{
				string s = files[i].Replace(".xnb", "").Split('\\')[files[i].Split('\\').Length - 1];
				textures.Add(s, game.Content.Load<Texture2D>("tiles\\" + s));
			}

			XmlSerializer ser = new XmlSerializer(typeof(LevelData));
			LevelData level;
			using (XmlReader reader = XmlReader.Create("Content\\levels\\" + name + ".xml"))
			{
				level = (LevelData)ser.Deserialize(reader);
			}

			this.size = level.size;
			foreach (TileInformation t in level.tiles)
				this.tiles.Add(new Tile(t.texture, new Vector2(t.X, t.Y), t.Scale, t.Rotation, t.Layer, t.Effect));
			foreach (StaticBodyInformation sb in level.staticBodies)
			{
				StaticBody body = new StaticBody(sb.Points);
				body.CreateBody(world, globalLighting);
				this.collisionMap.Add(body);
			}
			foreach (ObjectInformation obj in level.objects)
			{
//				Console.WriteLine(typeof(Box).AssemblyQualifiedName.ToArray());
				object[] parameters = new object[obj.ParameterValues.Count() + 2];
				parameters[0] = world;
				parameters[1] = obj.Position;
				for (int i = 2; i < parameters.Length; i++)
					parameters[i] = obj.ParameterValues[i - 2];
				GameObject converted = Activator.CreateInstance(Type.GetType(obj.Type), parameters) as GameObject;
				this.objects.Add(converted);
			}
			foreach (CameraBoxInformation cam in level.cameras)
				this.objects.Add(new CameraBox(cam.Target, cam.Bounds, cam.Priority));

			Texture2D lightTexture = LightTextureBuilder.CreatePointLight(game.GraphicsDevice, 512);
			Light2D globalLight = new Light2D()
			{
				Texture = lightTexture,
				Range = 10000,
				Color = Color.White,
				Intensity = 1f,
				X = level.size.X / 2,
				Y = 0,
				Fov = MathHelper.TwoPi
			};
			
			globalLighting.Lights.Add(globalLight);
			globalLighting.AmbientColor = new Color(50, 50, 50);
		}

		public Camera GetCamera()
		{
			return this.camera;
		}

		public GameScreen Update(GameTime gameTime)
		{
			foreach (GameObject obj in objects)
				obj.Update(objects);
			player.Update(objects);

			camera.Update(objects);
			world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
			return this;
		}

		public GameScreen Exit()
		{
			return null;
		}

		public void Draw(Game game, SpriteBatch sb, GameTime gameTime)
		{
			Matrix transformMatrix;
			if (camera != null)
				transformMatrix = Matrix.CreateTranslation(new Vector3(-camera.Position, 0));
			else
				transformMatrix = Matrix.Identity;
			transformMatrix *= Matrix.CreateScale(MainGame.RESOLUTION_SCALE);

			globalLighting.SpriteBatchCompatablityEnabled = true;
			globalLighting.Matrix = transformMatrix;
			globalLighting.CullMode = CullMode.None;
			globalLighting.Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
			globalLighting.Bluriness = 5;
			globalLighting.LightMapPrepare();

			game.GraphicsDevice.Clear(new Color(5, 5, 5));

			globalLighting.Draw(gameTime);

			sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, transformMatrix);
			player.Draw(sb);
			foreach (GameObject obj in objects)
				obj.Draw(sb);
			foreach (Tile t in tiles)
				t.Draw(sb, textures);
//			foreach (StaticBody b in collisionMap)
//				b.DebugDraw(sb);
			sb.End();

			globalLighting.Draw(gameTime);
		}
	}
}
