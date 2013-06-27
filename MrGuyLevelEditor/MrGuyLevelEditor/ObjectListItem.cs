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

		public override string ToString()
		{
			return Type.Split(',')[0];
		}
	}
}
