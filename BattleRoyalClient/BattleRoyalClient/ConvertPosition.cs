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

		public static PointF ConvertToViewLocation(PointF location, PointF startAxises)
		{
			location.X = (location.X - startAxises.X) * scale;
			location.Y = (location.Y - startAxises.Y) * scale;
			return location;
		}

		public static PointF ConvertToServerLocation(PointF location, PointF startAxises)
		{
			location.X  = (location.X + startAxises.X) / scale; ;
			location.Y = (location.Y + startAxises.Y) / scale;
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

		public static SizeF ConvertToViewSize(SizeF size)
		{
			size.Width *= scale;
			size.Height *= scale;
			return size;
		}

		public static SizeF ConvertToServerSize(SizeF size)
		{
			size.Width /= scale;
			size.Height /= scale;
			return size;
		}
	}
}
