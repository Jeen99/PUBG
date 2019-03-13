using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoyalClient
{
	static class ConvertPosition
	{
		private const int scale = 10;

		public static PointF ConvertToViewLocation(PointF location)
		{
			location.X *= scale;
			location.Y *= scale;
			return location;
		}

		public static PointF ConvertToServerLocation(PointF location)
		{
			location.X /= scale;
			location.Y /= scale;
			return location;
		}

		public static float ConvertToViewAxis(float coordiant)
		{
			return coordiant * scale;
		}

		public static float ConvertToServerAxis(float coordiant)
		{
			return coordiant / scale;
		}
	}
}
