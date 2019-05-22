using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using CommonLibrary.GameMessages;
using CommonLibrary.CommonElements;
using ServerTest.Common;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class MovementTest
	{
		[TestMethod]
		public void Test_CreatMovement()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			player.Setup();
			Movement movement = new Movement(player, 8);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateMovement()
		{
			Movement movement = new Movement(null, 10);
		}

		[TestMethod]
		public void Test_UpdateComponent_GoTo()
		{
			var model = new RoyalGameModel();
			//var model = new MockRoyalGameModel();
			//model.Field = new Box2DX.Dynamics.World()
			var player = BuilderGameObject.CreateGamer(model, new System.Drawing.PointF());

			var body = new SolidBody(player);
			player.Components.Add(body);
			player.Setup();

			float speed = 8;
			Movement movement = new Movement(player, speed);
			movement.Setup();
			player.Update(new GoTo(player.ID, new Direction(DirectionHorisontal.Left, DirectionVertical.Down)));

			var vector = body.Body.GetLinearVelocity();
			if (vector.X != -speed || vector.Y != -speed)
			{
				Assert.Fail();
			}
		}

	}
}
