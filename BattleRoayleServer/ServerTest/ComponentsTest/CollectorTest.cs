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
		public void Test_ErrorSetupCollector()
		{
			Collector collector = new Collector(null);
			collector.Setup();
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateCollector()
		{
			ICollector collector = new Collector(null);
		}

		[TestMethod]
		public void Test_GetWeapon()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			ICollector collector = new Collector(player);
			player.Setup();
			Assert.IsNull(collector.GetWeapon(TypesWeapon.Gun));
		}

		[TestMethod]
		public void Test_CollectorState()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			ICollector collector = new Collector(player);
			player.Setup();
			Assert.IsNotNull(collector.State);
		}

		[TestMethod]
		public void Test_PickUpWeapon()
		{
			var Room = new RoyalGameModel();

			var gun = new Gun(Room, new PointF(50, 70));
			gun.Setup();
			Room.GameObjects.Add(gun.ID, gun);

			var player1 = new Gamer(Room, new PointF(50, 70));
			player1.Setup();
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
