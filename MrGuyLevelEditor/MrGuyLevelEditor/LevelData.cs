using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MrGuy;

namespace MrGuyLevelEditor
{
	class LevelData
	{
		public Vector2 size;
		public List<Tile> tiles;
		public List<StaticBody> staticBodies;
		public List<PhysicsObject> physicsObjects;
		// There'll probably be more, like background and stuff
	}
}
