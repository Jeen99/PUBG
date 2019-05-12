using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using System.Drawing;

namespace BattleRoayleServer
{
	public static class VectorMethod
	{
		private static Random random = new Random();
		public static Vec2 RotateVector(float angle, float distance)
		{
			//в радианах, на вход угол в градусах
			//угол расчитывается против часовой стрелки
			double RadAngle = angle * (System.Math.PI / 180);
			return new Vec2()
			{
				X = (float)(distance * System.Math.Cos(RadAngle)),
				Y = (float)(-distance * System.Math.Sin(RadAngle))
			};
		}

		public static float DefineDistance(Vec2 A, Vec2 B)
		{
			return (float)System.Math.Sqrt(System.Math.Pow(B.X-A.X,2) + System.Math.Pow(B.Y - A.Y, 2));
		}

		public static float DefineAngle(Vec2 start, Vec2 end)
		{
			//позиция мыши	
			float angle = (float)(System.Math.Atan2(end.Y - start.Y, end.X - start.X) / System.Math.PI * 180);
			angle = (angle < 0) ? angle + 360 : angle;
			return angle;
		}

		public static PointF CreateRandPosition(float sizeMap)
		{
			return new PointF(random.Next(0, (int)sizeMap), random.Next(0,(int)sizeMap));
		}
	}
}
