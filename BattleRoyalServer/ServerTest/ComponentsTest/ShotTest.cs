using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class ShotTest
	{
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorSetupShot()
		{
			Shot shot = new Shot(new StubWeapon());
			shot.Setup();
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateShot()
		{
			Shot shot = new Shot(null);
		}

		[TestMethod]
		public void Test_CreateShot()
		{
			var weapon = new StubWeapon();
			weapon.Components.Add(new Magazin(weapon, TypesWeapon.Gun, 500, 3000, 8));
			Shot shot = new Shot(weapon);
		}

		[TestMethod]
		public void Test_UpdateComponent_MakeShot()
		{
			//создаем коллекцию объектов для теста
			var Room = new RoyalGameModel();

			var gun = BuilderGameObject.CreateGun(Room, new PointF(50, 70));
			var player1 = BuilderGameObject.CreateGamer(Room, new PointF(50, 70));
			var player2 = BuilderGameObject.CreateGamer(Room, new PointF(35, 75));

			Room.Field.Step(1f / 60f, 6, 3);
			//поднимаем оружие
			player1.Update(new TryPickUp(player1.ID));
			player1.Update(new TimeQuantPassed(1));
			CurrentWeapon currentWeapon = player1.Components.GetComponent<CurrentWeapon>();
			Assert.IsNotNull(currentWeapon.GetCurrentWeapon);
			//делаем выстрел
			player1.Update(new MakeShot(player1.ID, new PointF(35, 75)));
			player1.Update(new TimeQuantPassed(100));
			//проверяем
			player2.Update(new TimeQuantPassed(100));
			Healthy healtySecondGamer = player2.Components.GetComponent<Healthy>();
			Assert.AreEqual(92, healtySecondGamer.HP);

			//делаем 2 выстрел
			player1.Update(new MakeShot(player1.ID, new PointF(35, 75)));
			player1.Update(new TimeQuantPassed(401));
			//выстрел не должен произойти
			player2.Update(new TimeQuantPassed(401));
			Assert.AreEqual(92, healtySecondGamer.HP);

			Thread.Sleep(550);
			//делаем 3 выстрел
			player1.Update(new MakeShot(player1.ID, new PointF(35, 75)));
			player1.Update(new TimeQuantPassed(100));
			//выстрел должен произойти
			player2.Update(new TimeQuantPassed(100));
			Assert.AreEqual(healtySecondGamer.HP, 84);
		}

	}
}
