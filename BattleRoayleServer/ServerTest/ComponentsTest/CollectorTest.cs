using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CSInteraction.Common;
using System.Drawing;
using CSInteraction.ProgramMessage;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class CollectorTest
	{
		[TestMethod]
		public void Test_CreateCollector()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			ICollector collector = new Collector(player);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateCollector1()
		{
			ICollector collector = new Collector(new StubPlayer());
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateCollector12()
		{
			ICollector collector = new Collector(null);
		}

		[TestMethod]
		public void Test_GetWeapon()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			ICollector collector = new Collector(player);
			Assert.IsNull(collector.GetWeapon(TypesWeapon.Gun));
		}

		[TestMethod]
		public void Test_CollectorState()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			ICollector collector = new Collector(player);
			Assert.IsNotNull(collector.State);
		}

		[TestMethod]
		public void Test_PickUpWeapon()
		{
			var Room = new RoyalGameModel();
			var gun = new Gun(Room, new PointF(50, 70));
			Room.GameObjects.Add(gun.ID, gun);

			var player1 = new Gamer(Room, new PointF(50, 70));
			Room.GameObjects.Add(player1.ID, player1);
			Room.Players.Add(player1);

			Room.Field.Step(1f / 60f, 6, 3);

			//поднимаем оружие
			player1.SendMessage(new TryPickUp());
			player1.Update(new TimeQuantPassed(1));
			var collector = player1.Components.GetComponent<Collector>();
			Assert.IsNotNull(collector.GetWeapon(TypesWeapon.Gun));
			
		}

		[TestMethod]
		public void Test_PickUpLootBox()
		{
			var Room = new RoyalGameModel();
			var weapons = new Weapon[4];

			weapons[0] = new Gun(Room, new PointF(50, 70));
			var lootBox = new LootBox(Room, new Collector(new StubPlayer(), weapons), new PointF(50, 70));
			Room.GameObjects.Add(lootBox.ID, lootBox);

			var player1 = new Gamer(Room, new PointF(50, 70));
			Room.GameObjects.Add(player1.ID, player1);
			Room.Players.Add(player1);

			Room.Field.Step(1f / 60f, 6, 3);

			//поднимаем оружие
			player1.SendMessage(new TryPickUp());
			player1.Update(new TimeQuantPassed(1));
			var collector = player1.Components.GetComponent<Collector>();
			Assert.IsNotNull(collector.GetWeapon(TypesWeapon.Gun));

		}
	}
}
