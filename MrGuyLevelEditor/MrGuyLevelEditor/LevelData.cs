using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MrGuyLevelEditor
{
	public class LevelData
	{
		public Vector2 size;
		public List<TileInformation> tiles;
		public List<StaticBodyInformation> staticBodies;
		public List<ObjectInformation> objects;
		// There'll probably be more, like background and stuff
	}
}
