using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrGuyLevelEditor
{
	public class ObjectListItem
	{
		public string Type;
		public string[] Parameters;

		public ObjectListItem(string line)
		{
			string[] split = line.Split('#');
			this.Type = split[0];
			if (split.Length > 1)
			{
				this.Parameters = new string[split.Length - 1];
				for (int i = 1; i < split.Length; i++)
					Parameters[i - 1] = split[i];
			}
		}

		public string[] GetExtraParameters()
		{
			if (Parameters == null)
				return null;

			List<string> p = new List<string>();
			for (int i = 0; i < Parameters.Length; i++)
			{
				if (Parameters[i] != "FacingLeft" && Parameters[i] != "Width" && Parameters[i] != "Height" && Parameters[i] != "Rotation" && Parameters[i] != "Scale" &&
						Parameters[i] != "Rotation" && Parameters[i] != "Radius" && Parameters[i] != "Position2")
					p.Add(Parameters[i]);
			}
			return p.ToArray();
		}

		public bool HasExtraParameters()
		{
			if (Parameters != null)
			{
				for (int i = 0; i < Parameters.Length; i++)
				{
					if (Parameters[i] != "FacingLeft" && Parameters[i] != "Width" && Parameters[i] != "Height" && Parameters[i] != "Rotation" && Parameters[i] != "Scale" &&
							Parameters[i] != "Rotation" && Parameters[i] != "Radius" && Parameters[i] != "Position2")
						return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return Type.Split(',')[0];
		}
	}
}
