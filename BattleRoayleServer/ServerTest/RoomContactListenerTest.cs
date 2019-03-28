using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Threading;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System.Drawing;

namespace ServerTest
{
	[TestClass]
	public class RoomContactListenerTest
	{
		[TestMethod]
		public void TestRoomContactListener_Add()
		{
			var Room = new RoyalGameModel();
			var gun = new Gun(new PointF(50, 70), Room);
			Room.GameObjects.Add(gun.ID, gun);
			var player1 = new Gamer(new PointF(50, 70), Room);

		    ISolidBody solid = (ISolidBody)player1.Components.GetComponent<SolidBody>();
			Room.Field.Step(1 / 60, 6, 3);
			Assert.AreEqual(solid.CoveredObjects.Count, 1);
		}

		[TestMethod]
		public void TestRoomContactListener_Remove()
		{
			var Room = new RoyalGameModel();
			var gun = new Gun(new PointF(50, 70), Room);
			Room.GameObjects.Add(gun.ID, gun);
			var player1 = new Gamer(new PointF(50, 70), Room);

			ISolidBody solid = player1.Components.GetComponent<SolidBody>();
			solid.Body.SetLinearVelocity(new Vec2(30F, 0));
			Room.Field.Step(1, 6, 3);
			Room.Field.Step(1/60, 6, 3);
			Assert.AreEqual(solid.CoveredObjects.Count, 0);
		}

		[TestMethod]
		public void TestRoomContactListener_RemoveObject()
		{
			var Room = new RoyalGameModel();
			var gun = new Gun(new PointF(50, 70), Room);
			Room.GameObjects.Add(gun.ID, gun);
			var player1 = new Gamer(new PointF(50, 70), Room);

			ISolidBody solid = player1.Components.GetComponent<SolidBody>();
			Room.Field.Step(1 / 60, 6, 3);
			solid.Parent.Model.Field.DestroyBody(solid.Body);
			Assert.AreEqual(solid.CoveredObjects.Count, 0);
		}

		
	}
}
