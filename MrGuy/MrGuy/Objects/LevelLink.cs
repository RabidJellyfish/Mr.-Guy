using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Dynamics;

using MrGuy.Screens;

namespace MrGuy.Objects
{
	class LevelLink : GameObject
	{
		private Rectangle bounds;
		private string destination;
		private Vector2 newPosition;

		public LevelLink(World world, int index, Vector2 pos, string width, string height, string destination, string newPos)
			: base()
		{
			this.Position = pos;
			this.Index = index;
			
			float w = float.Parse(width);
			float h = float.Parse(height);
			bounds = new Rectangle((int)(pos.X - w / 2), (int)(pos.Y - h / 2), (int)w, (int)h);

			this.destination = destination;
			string[] split = newPos.Replace(" ", "").Split(',');
			this.newPosition = new Vector2(float.Parse(split[0]), float.Parse(split[1]));
		}

		public GameWorld CheckWorldChange(Game g, PlayerGuy player)
		{
			if (bounds.Contains(new Point((int)player.Position.X, (int)player.Position.Y)))
			{
				player.Position = newPosition;
				return new GameWorld(player.Position, g, destination);
			}
			else
				return null;
		}

		public override void Update(List<GameObject> otherObjects, GameTime gameTime) 
		{
 			// Update fadeout here?

			base.Update(otherObjects, gameTime);
		}
		
		public override void Draw(SpriteBatch sb) { }
	}
}
