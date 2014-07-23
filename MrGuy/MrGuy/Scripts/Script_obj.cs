using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Xna.Framework;

namespace MrGuy.Scripts
{
	public partial class Script
	{
		public string Name;
		public int InitDelay;
		public int LoopCount;
		public int LoopDelay;
		public string TriggerName;
		public string[] Param;

		private int maxLoopDelay;

		public Script(string name) : this(name, 0, -1, 0, "", null) { }

		public Script(string name, string triggerName) : this(name, 0, -1, 0, triggerName, null) { }

		public Script(string name, int initDelay, int loopCount, int loopDelay, string triggerName, string[] param)
		{
			this.Name = name;
			this.InitDelay = initDelay;
			this.LoopCount = loopCount;
			this.LoopDelay = loopDelay;
			this.TriggerName = triggerName;
			this.Param = param;
		}

		public void Update(GameTime gameTime)
		{
			if (InitDelay <= 0)
			{
				if (LoopCount != 0)
				{
					maxLoopDelay = Math.Max(maxLoopDelay, LoopDelay);
					if (LoopDelay <= 0)
					{
						if (TriggerName != "" && Trigger.IsTriggered(TriggerName))
							Call(this.Name, this.Param);
						LoopDelay = maxLoopDelay;
						LoopCount--;
					}
					else
						LoopDelay -= gameTime.ElapsedGameTime.Milliseconds;
				}
			}
			else
				InitDelay -= gameTime.ElapsedGameTime.Milliseconds;
		}

		public static void Call(string scriptName, string[] param)
		{
			Type type = typeof(Script);
			MethodInfo method = type.GetMethod(scriptName);
			method.Invoke(null, param);
		}
	}
}
