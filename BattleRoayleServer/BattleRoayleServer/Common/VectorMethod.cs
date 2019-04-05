using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;

namespace BattleRoayleServer
{
	static class VectorMethod
	{
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
	}
}
