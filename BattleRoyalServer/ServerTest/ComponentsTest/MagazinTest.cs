using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using ServerTest.Common;
using CommonLibrary;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class MagazinTest
	{
		[TestMethod]
		public void Test_CreateMagazin()
		{
			Magazin magazin = new Magazin(new StubWeapon(), TypesWeapon.Gun, 500, 3000, 8);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateMagazin()
		{
			Magazin magazin = new Magazin(null, TypesWeapon.Gun, 500, 3000, 8);
		}

		[TestMethod]
		public void Test_StateMagazin()
		{
			Magazin magazin = new Magazin(new StubWeapon(), TypesWeapon.Gun, 500, 3000, 8);
			Assert.IsNotNull(magazin.State);
		}

		[TestMethod]
		public void Test_GetBullet()
		{
			var model = new MockRoyalGameModel();
			var player = new MockPlayer()
			{
				Model = model
			};
			var weapon = new Weapon(model, TypesGameObject.Weapon, TypesBehaveObjects.Active, TypesWeapon.Gun);
			(weapon as Weapon).Owner = player;
			Magazin magazin = new Magazin(weapon, TypesWeapon.Gun, 50, 300, 8);
			magazin.Setup();

			//делаем 8 выстрелов
			for (int i = 0; i < 8; i++)
			{
				Assert.IsNotNull(magazin.GetBullet());
				Assert.IsNull(magazin.GetBullet());
				magazin.Parent.Update(new TimeQuantPassed(51));
			}
			//перезаряжаем 
			Assert.IsNull(magazin.GetBullet());
			magazin.Parent.Update(new TimeQuantPassed(301));
			Assert.IsNotNull(magazin.GetBullet());
		}

		[TestMethod]
		public void Test_UpdateComponent_MakeReload()
		{
			TypesWeapon typesWeapon = TypesWeapon.Gun;
			int bullet_In_Magazine = 8;
			int duration_Magazine = 300;
			int duration_betweenShots = 50;

			var model = new MockRoyalGameModel();
			var player = new MockPlayer()
			{
				Model = model
			};
			var weapon = new Weapon(model, TypesGameObject.Weapon, TypesBehaveObjects.Active, TypesWeapon.Gun);
			weapon.Owner = player;

			Magazin magazin = new Magazin(weapon, typesWeapon, duration_betweenShots, duration_Magazine, bullet_In_Magazine);
			weapon.Components.Add(magazin);
			weapon.Setup();

			Assert.IsNotNull(magazin.GetBullet());
			player.Update_MakeReloadWeapon(new MakeReloadWeapon(weapon.ID));	// игров собирается выполнить перезарядку
			Assert.IsNull(magazin.GetBullet());

			weapon.Update(new TimeQuantPassed(duration_Magazine - 1));
			Assert.IsNull(magazin.GetBullet());

			weapon.Update(new TimeQuantPassed(1));
			Assert.IsNotNull(magazin.GetBullet());

			var outMgs = model.outgoingMessages;

			Assert.IsTrue(outMgs.Dequeue().Count == 7);

			var msg_startReload = outMgs.Dequeue();
			var msg_ChangeBullet_1 = outMgs.Dequeue();
			var mgs_endReload = outMgs.Dequeue();
			var msg_ChangeBullet_2 = outMgs.Dequeue();

			Assert.IsTrue(msg_startReload.StartOrEnd == true);
			Assert.IsTrue(msg_ChangeBullet_1.Count == 8);
			Assert.IsTrue(mgs_endReload.StartOrEnd == false);
			Assert.IsTrue(msg_ChangeBullet_2.Count == 7);
		}
	}
}
