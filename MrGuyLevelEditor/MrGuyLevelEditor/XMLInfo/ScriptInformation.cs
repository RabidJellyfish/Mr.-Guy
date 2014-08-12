using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrGuyLevelEditor.XMLInfo
{
	public class ScriptInformation
	{
		public string Name;
		public int InitDelay;
		public int LoopCount;
		public int LoopDelay;
		public string TriggerName;
		public string[] Params;

		public ScriptInformation()
			: this("")
		{
		}

		public ScriptInformation(string name)
			: this(name, 0, -1, 0, "", null)
		{
		}

		public ScriptInformation(string name, int initDelay, int loopCount, int loopDelay, string trigger, string[] param)
		{
			this.Name = name;
			this.InitDelay = initDelay;
			this.LoopCount = loopCount;
			this.LoopDelay = loopDelay;
			this.TriggerName = trigger;
			this.Params = param;
		}

		public override string ToString()
		{
			string ret = Name + "(";
			if (Params != null)
			{
				for (int i = 0; i < Params.Length - 1; i++)
					ret += Params[i] + ", ";
				if (Params.Length >= 1)
					ret += Params[Params.Length - 1];
			}
			return ret + ")";
		}
	}
}
