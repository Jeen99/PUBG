using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;

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
			weapon.Components.Add(new Magazin(weapon, TypesWeapon.Gun, 500, 3000,8));
			Shot shot = new Shot(weapon);
		}

		[TestMethod]
		public void Test_UpdateComponent_MakeShot()
		{
			//создаем коллекцию объектов для теста
			var Room = new RoyalGameModel();

			var gun = new Gun(Room, new PointF(50, 70));
			gun.Setup();
			Room.GameObjects.Add(gun.ID, gun);

			var player1 = new Gamer(Room, new PointF(50, 70));
			player1.Setup();
			Room.GameObjects.Add(player1.ID, player1);

			var player2 = new Gamer(Room, new PointF(35, 75));
			player2.Setup();
			Room.GameObjects.Add(player2.ID, player2);

			Room.Field.Step(1f / 60f, 6, 3);
			//поднимаем оружие
			player1.Update(new TryPickUp());
			player1.Update(new TimeQuantPassed(1));
			CurrentWeapon currentWeapon = player1.Components.GetComponent<CurrentWeapon>();
			Assert.IsNotNull(currentWeapon.GetCurrentWeapon);
			//делаем выстрел
			player1.Update(new MakeShot(new PointF(35, 75)));
			player1.Update(new TimeQuantPassed(100));
			//проверяем
			player2.Update(new TimeQuantPassed(100));
			Healthy healtySecondGamer = player2.Components.GetComponent<Healthy>();
			Assert.AreEqual(healtySecondGamer.HP, 92);

			//делаем 2 выстрел
			player1.Update(new MakeShot(new PointF(35, 75)));
			player1.Update(new TimeQuantPassed(401));
			//выстрел не должен произойти
			player2.Update(new TimeQuantPassed(401));
			Assert.AreEqual(healtySecondGamer.HP, 92);

			Thread.Sleep(550);
			//делаем 3 выстрел
			player1.Update(new MakeShot(new PointF(35, 75)));
			player1.Update(new TimeQuantPassed(100));
			//выстрел должен произойти
			player2.Update(new TimeQuantPassed(100));
			Assert.AreEqual(healtySecondGamer.HP, 84);
		}

	}
}
