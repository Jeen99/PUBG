using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using System.Drawing;
using System.Diagnostics;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;

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
			Collector collector = new Collector(player);
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
			Collector collector = new Collector(null);
		}

		[TestMethod]
		public void Test_GetWeapon()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			Collector collector = new Collector(player);
			player.Setup();
			Assert.IsNull(collector.GetWeapon(TypesWeapon.Gun));
		}

		[TestMethod]
		public void Test_CollectorState()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			Collector collector = new Collector(player);
			player.Setup();

			Assert.IsNotNull(collector.State);
		}

		[TestMethod]
		public void Test_PickUpWeapon()
		{
			var Room = new RoyalGameModel();

			var gun = BuilderGameObject.CreateGun(Room, new PointF(50, 70));

			var player1 = BuilderGameObject.CreateGamer(Room, new PointF(50, 70));
			Room.Players.Add(player1);

			Room.Field.Step(1f / 60f, 6, 3);

			//поднимаем оружие
			player1.Update(new TryPickUp(player1.ID));
			player1.Update(new TimeQuantPassed(1));
			var collector = player1.Components.GetComponent<Collector>();

			var received_gun = collector.GetWeapon(TypesWeapon.Gun);
			Assert.AreEqual(gun ,received_gun);

		}

		[TestMethod]
		public void Test_Dispose()
		{
			var Room = new RoyalGameModel();
			var gun = BuilderGameObject.CreateGun(Room, new PointF(50, 70));

			var player1 = BuilderGameObject.CreateGamer(Room, new PointF(50, 70));
			Room.Field.Step(1f / 60f, 6, 3);

			//поднимаем оружие
			player1.Update(new TryPickUp(player1.ID));
			player1.Update(new TimeQuantPassed(1));
			var collector = player1.Components.GetComponent<Collector>();
			Assert.IsNotNull(collector.GetWeapon(TypesWeapon.Gun));
			Assert.IsNull(gun.Components.GetComponent<SolidBody>());

			int objectsInMap = Room.Field.GetBodyCount();

			//уничтожаем игрока 
			player1.Update(new GotDamage(player1.ID, 100));
			player1.Dispose();

			Assert.AreEqual(objectsInMap, Room.Field.GetBodyCount());
			Assert.IsNotNull(gun.Components.GetComponent<SolidBody>());

			//отслеживаем движение 
			var bodyGun = gun.Components.GetComponent<SolidBody>();
			PointF startStep = bodyGun.Shape.Location;
			for (int i = 0; i < 45; i++)
			{
				Room.Field.Step(0.1f, 6, 3);
				gun.Update(new TimeQuantPassed(100));
				PointF endStep = bodyGun.Shape.Location;
				Debug.WriteLine($"{endStep.X}:{endStep.Y}");
				Assert.AreNotEqual(startStep, endStep);
				startStep = endStep;
			}
		}
	}
}
