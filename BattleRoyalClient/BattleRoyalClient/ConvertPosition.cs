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
			return new PointF(location.X * scale, location.Y * scale);
		}

		public static PointF ConvertToServerLocation(PointF location)
		{
			return new PointF(location.X / scale, location.Y / scale);
		}
	}
}
