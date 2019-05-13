using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Drawing;
using Box2DX.Common;
using CommonLibrary.GameMessages;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class SolidBodyTest
	{
		[TestMethod]
		public void Test_CreateSolidBody()
		{
			SolidBody solidBody = new SolidBody(new StubPlayer());
			Assert.IsNotNull(solidBody.Body);
		}

		[TestMethod]
		public void Test_SolidBodyState()
		{
			SolidBody solidBody = new SolidBody(new StubPlayer());
			Assert.IsNotNull(solidBody.State);
		}
	
		[TestMethod]
		public void Test_UpdateComponent_TimeQuantPassed()
		{
			var Room = new RoyalGameModel();
			var player1 = BuilderGameObject.CreateGamer(Room, new PointF(50, 70));

			SolidBody solid = player1.Components.GetComponent<SolidBody>();
			RectangleF compareShape = solid.Shape;
			Assert.AreEqual(solid.Shape, compareShape);
			Vec2 compareVec = solid.Body.GetPosition();
			solid.Body.SetLinearVelocity(new Vec2(40f,0));

			int quantTime = 60;
			Room.Field.Step((float)quantTime/1000f, 8, 3);
			var A = solid.Body.GetPosition();
			Assert.AreNotEqual(A, compareVec);

			player1.Update(new TimeQuantPassed(quantTime));
			Assert.AreNotEqual(solid.Shape, compareVec);
		}
	}
}
