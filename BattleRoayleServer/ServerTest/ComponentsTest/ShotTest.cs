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
		public void Test_ErrorCreateShot1()
		{
			Shot shot = new Shot(new StubWeapon());
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateShot2()
		{
			Shot shot = new Shot(null);
		}

		[TestMethod]
		public void Test_CreateShot()
		{
			var weapon = new StubWeapon();
			weapon.Components.Add(new Magazin(weapon, TypesWeapon.Gun, 500, 3000));
			Shot shot = new Shot(weapon);
		}

		[TestMethod]
		public void Test_UpdateComponent_MakeShot()
		{
			//создаем коллекцию объектов для теста
			var Room = new RoyalGameModel();
			var gun = new Gun(new PointF(50, 70), Room);
			Room.GameObjects.Add(gun.ID, gun);
			var player1 = new Gamer(new PointF(50, 70), Room);
			Room.GameObjects.Add(player1.ID, player1);
			var player2 = new Gamer(new PointF(50, 85), Room);
			Room.GameObjects.Add(player2.ID, player2);

			Room.Field.Step(1f / 60f, 6, 3);
			//поднимаем оружие
			player1.SendMessage(new TryPickUp());
			player1.Update();
			ICurrentWeapon currentWeapon = player1.Components.GetComponent<CurrentWeapon>();
			Assert.IsNotNull(currentWeapon.GetCurrentWeapon);
			//делаем выстрел
			player1.SendMessage(new MakeShot(2));
			player1.Update();
			//проверяем
			player2.Update();
			IHealthy healtySecondGamer = player2.Components.GetComponent<Healthy>();
			Assert.AreEqual(healtySecondGamer.HP, 92);

			//делаем 2 выстрел
			player1.SendMessage(new MakeShot(0));
			player1.Update();
			//выстрел не должен произойти
			player2.Update();
			Assert.AreEqual(healtySecondGamer.HP, 92);

			Thread.Sleep(550);
			//делаем 3 выстрел
			player1.SendMessage(new MakeShot(0));
			player1.Update();
			//выстрел должен произойти
			player2.Update();
			Assert.AreEqual(healtySecondGamer.HP, 84);
		}

	}
}
