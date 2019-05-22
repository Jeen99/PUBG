using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using System.Drawing;
using CSInteraction;
using CommonLibrary.GameMessages;

namespace ServerTest
{
	[TestClass]
	public class RoyalGameModelTest
	{
		[TestMethod]
		public void Test_CreateModel()
		{
			IGameModel model = new RoyalGameModel(2);
		}

		[TestMethod]
		public void Test_AddGameObject()
		{
			var model = new RoyalGameModel();
			int count = model.gameObjects.Count;
			var stone = BuilderGameObject.CreateStone(model, new PointF(30, 20));
			stone.Setup();

			model.AddOrUpdateGameObject(stone);
			Assert.AreEqual(count + 1, model.gameObjects.Count);
		}
		[TestMethod]
		public void Test_RemoveGameObject()
		{
			var model = new RoyalGameModel();
			int count = model.gameObjects.Count;
			var stone = BuilderGameObject.CreateStone(model, new PointF(30, 20));
			stone.Setup();

			model.AddOrUpdateGameObject(stone);
			Assert.AreEqual(count + 1, model.gameObjects.Count);
			model.RemoveGameObject(stone);
			Assert.AreEqual(count, model.gameObjects.Count);
		}

		[TestMethod]
		public void Test_AddIncomingMessage()
		{
			var model = new RoyalGameModel();
			int count = model.incomingMessages.Count;
			model.AddIncomingMessage(new ObjectMoved(10, new PointF(10, 50)));
			Assert.AreEqual(count + 1, model.incomingMessages.Count);
		}

		[TestMethod]
		public void Test_AddOutgoingMessage()
		{
			var model = new RoyalGameModel();
			int count = model.outgoingMessages.Count;
			model.AddOutgoingMessage(new ObjectMoved(10, new PointF(10, 50)));
			Assert.AreEqual(count + 1, model.outgoingMessages.Count);
		}

	}
}
