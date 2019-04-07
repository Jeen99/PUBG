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

			model.AddGameObject(stone);
			Assert.AreEqual(count + 1, model.GameObjects.Count);
		}
		[TestMethod]
		public void Test_RemoveGameObject()
		{
			IGameModel model = new RoyalGameModel();
			int count = model.GameObjects.Count;
			var stone = new Stone((IModelForComponents)model, new PointF(30, 20), new Size(10, 10));
			stone.Setup();

			model.AddGameObject(stone);
			Assert.AreEqual(count + 1, model.GameObjects.Count);
			model.RemoveGameObject(stone);
			Assert.AreEqual(count, model.GameObjects.Count);
		}

		[TestMethod]
		public void Test_AddLoot()
		{
			IModelForComponents model = new RoyalGameModel();
			int count = (model as IGameModel).Loot.Count;
			var gun = new Gun(model, new PointF(30, 20));
			gun.Setup();

			Assert.AreEqual(count + 1, (model as IGameModel).Loot.Count);
		}

		[TestMethod]
		public void Test_RemoveLoot()
		{
			IModelForComponents model = new RoyalGameModel();
			int count = (model as IGameModel).Loot.Count;
			var gun = new Gun(model, new PointF(30, 20));
			gun.Setup();

			Assert.AreEqual(count + 1, (model as IGameModel).Loot.Count);
			model.RemoveLoot(gun);
			Assert.AreEqual(count, (model as IGameModel).Loot.Count);
		}

		[TestMethod]
		public void Test_AddEvent()
		{
			IModelForComponents model = new RoyalGameModel();
			int count = (model as IGameModel).HappenedEvents.Count;
			model.AddEvent(new ObjectMoved(10, new PointF(10,50)));
			Assert.AreEqual(count + 1, (model as IGameModel).HappenedEvents.Count);
		}

		[TestMethod]
		public void Test_GetPickUpObjects()
		{
			IModelForComponents model = new RoyalGameModel();

			var gun = new Gun(model, new PointF(50, 70));
			gun.Setup();
			model.AddGameObject(gun);

			var box = new Box(model, new PointF(50, 70), new SizeF(10, 10));
			box.Setup();
			model.AddGameObject(box);

			var player1 = new Gamer(model, new PointF(50, 70));
			player1.Setup();
			model.AddGameObject(player1);

			(model as IGameModel).Players.Add(player1);
			Assert.AreEqual(model.GetPickUpObjects(player1.Components.GetComponent<SolidBody>().Shape).Count, 1);
			player1.SendMessage(new TryPickUp());
			player1.Update(new TimeQuantPassed(1));
			Assert.AreEqual(model.GetPickUpObjects(player1.Components.GetComponent<SolidBody>().Shape).Count, 0);
		}

	}
}
