using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using ServerTest.Common;

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
			(weapon as Weapon).Parent = player;
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
			var model = new MockRoyalGameModel();
			var player = new MockPlayer()
			{
				Model = model
			};
			var weapon = new Weapon(model, TypesGameObject.Weapon, TypesBehaveObjects.Active, TypesWeapon.Gun);
			(weapon as Weapon).Parent = player;

			Magazin magazin = new Magazin(weapon, TypesWeapon.Gun, 50, 300, 8);
			magazin.Setup();

			weapon.Update(new MakeReloadWeapon(weapon.ID));
			Assert.IsNull(magazin.GetBullet());
			weapon.Update(new TimeQuantPassed(301));
			Assert.IsNotNull(magazin.GetBullet());
		}
	}
}
