using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Drawing;
using CSInteraction.ProgramMessage;

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
			IGameModel model = new RoyalGameModel();
			int count = model.GameObjects.Count;
			var stone = new Stone((IModelForComponents)model, new PointF(30, 20), new Size(10, 10));
			stone.Setup();

			model.AddOrUpdateGameObject(stone);
			Assert.AreEqual(count + 1, model.GameObjects.Count);
		}
		[TestMethod]
		public void Test_RemoveGameObject()
		{
			IGameModel model = new RoyalGameModel();
			int count = model.GameObjects.Count;
			var stone = new Stone((IModelForComponents)model, new PointF(30, 20), new Size(10, 10));
			stone.Setup();

			model.AddOrUpdateGameObject(stone);
			Assert.AreEqual(count + 1, model.GameObjects.Count);
			model.RemoveGameObject(stone);
			Assert.AreEqual(count, model.GameObjects.Count);
		}

		[TestMethod]
		public void Test_AddEvent()
		{
			IModelForComponents model = new RoyalGameModel();
			int count = (model as IGameModel).HappenedEvents.Count;
			model.AddEvent(new ObjectMoved(10, new PointF(10,50)));
			Assert.AreEqual(count + 1, (model as IGameModel).HappenedEvents.Count);
		}

	}
}
