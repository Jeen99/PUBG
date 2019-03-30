using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Drawing;

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
			var Box = new Stone((IModelForComponents)model, new PointF(30, 20), new Size(10, 10));
			model.AddGameObject(Box);
			Assert.AreEqual(count + 1, model.GameObjects.Count);
		}
		[TestMethod]
		public void Test_RemoveGameObject()
		{
			IGameModel model = new RoyalGameModel();
			int count = model.GameObjects.Count;
			var Box = new Stone((IModelForComponents)model, new PointF(30, 20), new Size(10, 10));
			model.AddGameObject(Box);
			Assert.AreEqual(count + 1, model.GameObjects.Count);
			model.RemoveGameObject(Box);
			Assert.AreEqual(count, model.GameObjects.Count);
		}
	}
}
