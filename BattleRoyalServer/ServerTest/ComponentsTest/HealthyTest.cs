using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using CommonLibrary.GameMessages;
using ServerTest.Common;
using CommonLibrary;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class HealthyTest
	{
		[TestMethod]
		public void Test_CreateHealthy()
		{
			Healthy healthy = new Healthy(new StubPlayer());
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateHealthy()
		{
			Healthy healthy = new Healthy(null);
		}

		[TestMethod]
		public void Test_HealthyState()
		{
			Healthy healthy = new Healthy(new StubPlayer());
			Assert.IsNotNull(healthy.State);
		}

		[TestMethod]
		public void Test_Handler_GotDamage()
		{
			//Arrange (подготовка)
			int expectedHP = 92;
			int DamageHP = 8;

			MockRoyalGameModel model = new MockRoyalGameModel();
			var player = new MockPlayer()
			{
				Model = model
			};

			Healthy healthy = new Healthy(player);
			healthy.Setup();

			//Act (выполнение)
			player.Update_GotDamage(new GotDamage(player.ID, DamageHP));
			IMessage msg = model.outgoingMessages.Dequeue();

			//Assert (проверка)
			Assert.IsTrue((msg as ChangedValueHP).HP == expectedHP);
			Assert.AreEqual(expectedHP, healthy.HP);
			Assert.AreEqual(0, model.outgoingMessages.Count);
		}

		[TestMethod]
		public void Test_Handler_GotDamage_100HP()
		{
			//Arrange (подготовка)
			int expectedHP = 0;
			int DamageHP = 100;

			MockRoyalGameModel model = new MockRoyalGameModel();
			var player = new MockPlayer()
			{
				Model = model
			};

			Healthy healthy = new Healthy(player);
			healthy.Setup();

			//Act (выполнение)
			player.Update_GotDamage(new GotDamage(player.ID, DamageHP));
			IMessage msg = model.outgoingMessages.Dequeue();

			//Assert (проверка)
			Assert.IsTrue(player.Destroyed);
			Assert.IsTrue((msg as ChangedValueHP).HP == expectedHP);
			Assert.AreEqual(expectedHP, healthy.HP);
			Assert.AreEqual(0, model.outgoingMessages.Count);
		}
	}
}
