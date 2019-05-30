using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using System.Drawing;
using CommonLibrary.CommonElements;

namespace ServerTest.ComponentsTest
{
	[TestClass()]
	public class AITests
	{
		[TestMethod()]
		public void AITest()
		{
			var model = new RoyalGameModel();

			var gameObject = new GameObject(model, TypesGameObject.Player, TypesBehaveObjects.Active);
			var ai = new AI(gameObject);
			var solidBody = new SolidBody(gameObject);
			var movement = new Movement(gameObject, 10);

			gameObject.Components.Add(ai);
			gameObject.Components.Add(solidBody);
			gameObject.Components.Add(movement);

			model.AddOrUpdateGameObject(gameObject);

			PointF startPos = solidBody.Shape.Location;
			model.MakeStep(100000);

			PointF endPos = solidBody.Shape.Location;

			Assert.AreNotEqual(endPos, startPos);
			
		}

		[TestMethod()]
		public void SetupTest()
		{

		}

		[TestMethod()]
		public void DisposeTest()
		{

		}
	}
}