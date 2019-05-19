using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using Box2DX.Common;
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
			var box = BuilderGameObject.CreateBox(Room, new PointF(55, 70));
			var player1 = BuilderGameObject.CreateGamer(Room, new PointF(50, 70));

			SolidBody solid = (SolidBody)player1.Components.GetComponent<SolidBody>();
			Room.Field.Step(1 / 60, 6, 3);
			Assert.AreEqual(solid.CoveredObjects.Count, 1);
		}

		[TestMethod]
		public void TestRoomContactListener_Remove()
		{
			var Room = new RoyalGameModel();

			var box = BuilderGameObject.CreateBox(Room, new PointF(55, 70));
			box.Setup();
			Room.AddOrUpdateGameObject(box);

			var player1 = BuilderGameObject.CreateGamer(Room, new PointF(50, 70));

			SolidBody solid = player1.Components.GetComponent<SolidBody>();
			solid.Body.SetLinearVelocity(new Vec2(0, 40f));
			Room.Field.Step(2, 6, 3);
			//только после 2 перемещения срабатывает потеря наслоения
			Room.Field.Step(1f/60f, 6, 3);
			Assert.AreEqual(solid.CoveredObjects.Count, 0);
		}

		[TestMethod]
		public void TestRoomContactListener_RemoveObject()
		{
			var Room = new RoyalGameModel();

			var box = BuilderGameObject.CreateBox(Room, new PointF(55, 70));
			box.Setup();
			Room.AddOrUpdateGameObject(box);

			var player1 = BuilderGameObject.CreateGamer(Room, new PointF(50, 70));

			SolidBody solid = player1.Components.GetComponent<SolidBody>();
			Room.Field.Step(1f / 60f, 6, 3);
			solid.Parent.Model.Field.DestroyBody(solid.Body);
			Assert.AreEqual(solid.CoveredObjects.Count, 0);
		}

		
	}
}
