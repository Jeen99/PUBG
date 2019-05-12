using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using System.Drawing;
using Math = System.Math;

namespace BattleRoayleServer
{
	public static class VectorMethod
	{
		private static Random random = new Random();

		public static Vec2 RotateVector(float angle, float distance)
		{
			//в радианах, на вход угол в градусах
			//угол расчитывается против часовой стрелки
			double rad = (angle * Math.PI) / 180.0;

			// для корректного расчета, иначе sin(90град) != 1.0
			float deltaX = (float) Math.Round(Math.Cos(rad), 7);
			float deltaY = (float) Math.Round(-Math.Sin(rad), 7);

			return new Vec2()
			{
				X = distance * deltaX,
				Y = distance * deltaY
			};
		}

		public static float DefineDistance(Vec2 A, Vec2 B)
		{
			return (float)Math.Sqrt(Math.Pow(B.X-A.X,2) + Math.Pow(B.Y - A.Y, 2));
		}

		public static float DefineAngle(Vec2 start, Vec2 end)
		{
			//позиция мыши	
			float angle = (float)(Math.Atan2(end.Y - start.Y, end.X - start.X) / Math.PI * 180);
			angle = (angle < 0) ? angle + 360 : angle;

			return angle;
		}

		public static PointF CreateRandPosition(float sizeMap)
		{
			return new PointF()
			{
				X = random.Next(0, (int)sizeMap),
				Y = random.Next(0, (int)sizeMap)
			};
		}
	}
}
