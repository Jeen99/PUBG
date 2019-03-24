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
		    SolidBody solid = (SolidBody)gamer.GetComponent(typeof(SolidBody));
			Room.Field.Step(1 / 60, 6, 3);
			Assert.AreEqual(solid.CoveredObjects.Count, 1);
		}

		[TestMethod]
		public void TestRoomContactListener_Remove()
		{
			var Room = new RoyalGameModel(1);
			Gamer gamer = (Gamer)Room.Players[0];
			SolidBody solid = (SolidBody)gamer.GetComponent(typeof(SolidBody));
			solid.Body.SetLinearVelocity(new Vec2(30F, 0));
			Room.Field.Step(1, 6, 3);
			solid.BodyMove();
			Room.Field.Step(1/60, 6, 3);
			solid.BodyMove();
			Assert.AreEqual(solid.CoveredObjects.Count, 0);
		}

		[TestMethod]
		public void TestRoomContactListener_RemoveObject()
		{
			var Room = new RoyalGameModel(1);
			Gamer gamer = (Gamer)Room.Players[0];
			SolidBody solid = (SolidBody)gamer.GetComponent(typeof(SolidBody));
			Room.Field.Step(1 / 60, 6, 3);
			solid.Parent.Model.Field.DestroyBody(solid.Body);
			Assert.AreEqual(solid.CoveredObjects.Count, 0);
		}

		[TestMethod]
		public void TestMagazin_WorkMagazin()
		{
			Magazin magazin = new Magazin(null, TypesWeapon.Gun, 500, 3000);
			//делаем 8 выстрелов
			for (int i = 0; i < 8; i++)
			{
				Assert.IsNotNull(magazin.GetBullet());
				Assert.IsNull(magazin.GetBullet());
				Thread.Sleep(550);
			}
			//перезаряжаем 
			Assert.IsNull(magazin.GetBullet());
			Thread.Sleep(3050);
			Assert.IsNotNull(magazin.GetBullet());
		}
		[TestMethod]
		public void TestShot_Shot()
		{
			var Room = new RoyalGameModel(2);
			Gamer gamer = (Gamer)Room.Players[0];
			Room.Field.Step(1 / 60, 6, 3);
			//поднимаем оружие
			gamer.SendMessage(new TryPickUp());
			gamer.Update();
			CurrentWeapon currentWeapon = (CurrentWeapon)gamer.GetComponent(typeof(CurrentWeapon));
			Assert.IsNotNull(currentWeapon.GetCurrentWeapon);
			//удаляем объект с карты
			Room.NeedDelete[0].Body.GetWorld().DestroyBody(Room.NeedDelete[0].Body);
			Room.NeedDelete.Clear();
			//делаем выстрел
			gamer.SendMessage(new MakeShot(0));
			gamer.Update();
		}
	}
}
