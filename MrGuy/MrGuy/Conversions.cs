using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MrGuy
{
	class Conversions
	{
		public static float PixelToMeter(float pixels)
		{
			return pixels / 100.0f;
		}

		public static float MeterToPixel(float meters)
		{
			return 1 / PixelToMeter(meters);
		}
	}
}
