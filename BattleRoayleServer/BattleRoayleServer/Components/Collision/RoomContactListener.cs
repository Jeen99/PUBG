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
	class RoomContactListener:ContactListener
	{
		public override void Add(ContactPoint point)
		{
			base.Add(point);
			if (point.Shape1.GetBody().GetUserData() is SolidBody)
			{
				SolidBody bodyA = (SolidBody)point.Shape1.GetBody().GetUserData();
				if (point.Shape2.GetBody().GetUserData() is SolidBody)
				{
					SolidBody bodyB = (SolidBody)point.Shape2.GetBody().GetUserData();
					if (bodyA.CoveredObjects.IndexOf(bodyB) == -1)
					{
						bodyA.CoveredObjects.Add(bodyB);
					}
					if (bodyB.CoveredObjects.IndexOf(bodyA) == -1)
					{
						bodyB.CoveredObjects.Add(bodyA);
					}
				}
			}
		}

		public override void Remove(ContactPoint point)
		{
			base.Remove(point);
			if (point.Shape1.GetBody().GetUserData() is SolidBody)
			{
				SolidBody bodyA = (SolidBody)point.Shape1.GetBody().GetUserData();
				if (point.Shape2.GetBody().GetUserData() is SolidBody)
				{
					SolidBody bodyB = (SolidBody)point.Shape2.GetBody().GetUserData();
					bodyA.CoveredObjects.Remove(bodyB);
					bodyB.CoveredObjects.Remove(bodyA);
				}
			}
		}
		
	}
}
