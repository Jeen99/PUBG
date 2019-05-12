using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
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
			int count = model.GameObjects.Count;
			var stone = new Stone((IModelForComponents)model, new PointF(30, 20), new Size(10, 10));
			stone.Setup();

			model.AddOrUpdateGameObject(stone);
			Assert.AreEqual(count + 1, model.GameObjects.Count);
		}
		[TestMethod]
		public void Test_RemoveGameObject()
		{
			var model = new RoyalGameModel();
			int count = model.GameObjects.Count;
			var stone = new Stone(model, new PointF(30, 20), new Size(10, 10));
			stone.Setup();

			model.AddOrUpdateGameObject(stone);
			Assert.AreEqual(count + 1, model.GameObjects.Count);
			model.RemoveGameObject(stone);
			Assert.AreEqual(count, model.GameObjects.Count);
		}

		[TestMethod]
		public void Test_AddIncomingMessage()
		{
			var model = new RoyalGameModel();
			int count = model.IncomingMessages.Count;
			model.AddIncomingMessage(new ObjectMoved(10, new PointF(10, 50)));
			Assert.AreEqual(count + 1, model.IncomingMessages.Count);
		}

		[TestMethod]
		public void Test_AddOutgoingMessage()
		{
			var model = new RoyalGameModel();
			int count = model.OutgoingMessages.Count;
			model.AddIncomingMessage(new ObjectMoved(10, new PointF(10, 50)));
			Assert.AreEqual(count + 1, model.IncomingMessages.Count);
		}

	}
}
