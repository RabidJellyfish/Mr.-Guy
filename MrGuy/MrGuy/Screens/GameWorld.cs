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
using MrGuy.Scripts;

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
		List<Trigger> triggers;

		PlayerGuy player;

		Dictionary<string, Texture2D> textures;

		private Color targetLight;

		public GameWorld(Vector2 playerPos, Game game, string name)
		{
			world = new World(9.8f * Vector2.UnitY);
			
			globalLighting = new KryptonEngine(game, "KryptonEffect");
			globalLighting.Initialize();

			tiles = new List<Tile>();
			collisionMap = new List<StaticBody>();
			objects = new List<GameObject>();
			triggers = new List<Trigger>();

			Load(game, name);

			player = new PlayerGuy(world, playerPos.X, playerPos.Y, MainGame.texPlayer);
			objects.Add(player); // Player will have an index of -1
			camera = new Camera(player.Position, size, 15f, this.player);
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
				object[] parameters = new object[obj.ParameterValues == null ? 3 : (obj.ParameterValues.Count() + 3)];
				parameters[0] = world;
				parameters[1] = obj.Index;
				parameters[2] = obj.Position;
				for (int i = 3; i < parameters.Length; i++)
					parameters[i] = obj.ParameterValues[i - 3];
				GameObject converted = Activator.CreateInstance(Type.GetType(obj.Type), parameters) as GameObject;
				converted.Scripts = new List<Script>();
				foreach (ScriptInformation s in obj.Scripts)
					converted.Scripts.Add(new Script(s.Name, s.InitDelay, s.LoopCount, s.LoopDelay, s.TriggerName, s.Params));
				this.objects.Add(converted);
			}
			foreach (CameraBoxInformation cam in level.cameras)
				this.objects.Add(new CameraBox(cam.Target, cam.Bounds, cam.Priority));
			foreach (TriggerInformation trigger in level.triggers)
				this.triggers.Add(new Trigger(trigger.Bounds, trigger.Name, trigger.ObjID, trigger.WhenTrigger));

			targetLight = new Color(level.R, level.G, level.B);
			globalLighting.AmbientColor = new Color(0, 0, 0);

			//---- Temp ----
			Texture2D lightTexture = LightTextureBuilder.CreatePointLight(game.GraphicsDevice, 512);
			Light2D globalLight = new Light2D()
			{
				Texture = lightTexture,
				Range = 1600,
				Color = Color.White,
				Intensity = 1f,
				X = level.size.X / 2,
				Y = 600,
				Fov = MathHelper.TwoPi
			};
			globalLighting.Lights.Add(globalLight);
			// -------------
		}

		public Camera GetCamera()
		{
			return this.camera;
		}

		public List<GameObject> GetGameObjects()
		{
			return this.objects;
		}

		public void CreateObject(GameObject obj)
		{
			objects.Add(obj);
		}

		public World GetWorld()
		{
			return this.world;
		}

		public GameScreen Update(Game game, GameTime gameTime)
		{
			// Update light
			if (globalLighting.AmbientColor != targetLight)
				globalLighting.AmbientColor = new Color(Math.Min(globalLighting.AmbientColor.R + 5, targetLight.R),
														Math.Min(globalLighting.AmbientColor.G + 5, targetLight.G),
														Math.Min(globalLighting.AmbientColor.B + 5, targetLight.B));

			// Update objects
			foreach (GameObject obj in objects)
				obj.Update(objects, gameTime);
			foreach (Trigger t in triggers)
				t.Update(objects);
			camera.Update(objects);
			world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

			// Change level if level link is hit
			GameWorld changeWorld;
			var links = from GameObject o in objects
						where o is LevelLink
						select o as LevelLink;
			foreach (LevelLink l in links)
			{
				changeWorld = l.CheckWorldChange(game, player);
				if (changeWorld == null)
					changeWorld = this;
				else
					return changeWorld;
			}
			return this;
		}

		public GameScreen Exit()
		{
			// TODO: Show dialog, return to menu and stuff
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
			globalLighting.Bluriness = 10;
			globalLighting.LightMapPrepare();

			game.GraphicsDevice.Clear(Color.CornflowerBlue);

			globalLighting.Draw(gameTime);

			sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, transformMatrix);
			foreach (GameObject obj in objects)
				obj.Draw(sb);
			foreach (Tile t in tiles)
				t.Draw(sb, textures);
//			foreach (StaticBody b in collisionMap)
//				b.DebugDraw(sb);
			foreach (Trigger t in triggers)
				MainGame.DrawRectangleOutline(sb, t.Bounds, t.Name == "makeBox" ? Color.Blue : Color.Lime);

			sb.End();

			globalLighting.Draw(gameTime);
		}
	}
}
