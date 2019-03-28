using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Threading;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace ServerTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestRoomContactListener_Add()
		{
			var Room = new RoyalGameModel(1);
			Gamer gamer = (Gamer)Room.Players[0];
		    ISolidBody solid = (ISolidBody)gamer.Components.GetComponent<ISolidBody>();
			Room.Field.Step(1 / 60, 6, 3);
			Assert.AreEqual(solid.CoveredObjects.Count, 1);
		}

		[TestMethod]
		public void TestRoomContactListener_Remove()
		{
			var Room = new RoyalGameModel(1);
			Gamer gamer = (Gamer)Room.Players[0];
			ISolidBody solid = (ISolidBody)gamer.Components.GetComponent<ISolidBody>();
			solid.Body.SetLinearVelocity(new Vec2(30F, 0));
			Room.Field.Step(1, 6, 3);
			Room.Field.Step(1/60, 6, 3);
			Assert.AreEqual(solid.CoveredObjects.Count, 0);
		}

		[TestMethod]
		public void TestRoomContactListener_RemoveObject()
		{
			var Room = new RoyalGameModel(1);
			Gamer gamer = (Gamer)Room.Players[0];
			ISolidBody solid = (ISolidBody)gamer.Components.GetComponent<ISolidBody>();
			Room.Field.Step(1 / 60, 6, 3);
			solid.Parent.Model.Field.DestroyBody(solid.Body);
			Assert.AreEqual(solid.CoveredObjects.Count, 0);
		}

		[TestMethod]
		public void TestShot_Shot()
		{
			var Room = new RoyalGameModel(2);
			Gamer firstGamer = (Gamer)Room.Players[0];
			Room.Field.Step(1 / 60, 6, 3);
			//поднимаем оружие
			firstGamer.SendMessage(new TryPickUp());
			firstGamer.Update();
			ICurrentWeapon currentWeapon = (ICurrentWeapon)firstGamer.Components.GetComponent<ICurrentWeapon>();
			Assert.IsNotNull(currentWeapon.GetCurrentWeapon);
			//делаем выстрел
			firstGamer.SendMessage(new MakeShot(2));
			firstGamer.Update();
			//проверяем
			Gamer secondGamer = (Gamer)Room.Players[1];
			secondGamer.Update();
			IHealthy healtySecondGamer = (IHealthy)secondGamer.Components.GetComponent<IHealthy>();
			Assert.AreEqual(healtySecondGamer.HP, 92);

			//делаем 2 выстрел
			firstGamer.SendMessage(new MakeShot(0));
			firstGamer.Update();
			//выстрел не должен произойти
			secondGamer.Update();
			Assert.AreEqual(healtySecondGamer.HP, 92);

			Thread.Sleep(500);
			//делаем 3 выстрел
			firstGamer.SendMessage(new MakeShot(0));
			firstGamer.Update();
			//выстрел должен произойти
			secondGamer.Update();
			Assert.AreEqual(healtySecondGamer.HP, 84);
		}
	}
}
