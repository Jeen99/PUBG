using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattleRoayleServer.Common
{
	struct AABB
	{
		/// <summary>
		/// верхний левый угол ограничивающего прямоугольника
		/// </summary>
		public Vector Min
		{
			get { return min; }
			set { min = value; }
		}
		private Vector min;

		private Vector max;
		/// <summary>
		/// нижний правый угол ограничивающего прямоугольника
		/// </summary>
		public Vector Max
		{
			get { return max; }
			set { max = value; }
		}

		public AABB(Vector min, Vector max)
		{
			this.min = min;
			this.max = max;
		}

		public AABB(double minX, double minY, double maxX, double maxY)
		{
			this.min = new Vector(minX, minY);
			this.max = new Vector(maxX, maxY);
		}
	}
}
