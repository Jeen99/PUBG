using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Drawing;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

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
			IMovement movement = new Movement(player, 8);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateMovement()
		{
			IMovement movement = new Movement(null, 10);
		}

		[TestMethod]
		public void Test_UpdateComponent_GoTo()
		{
			var player = new StubPlayer();
			var body = new SolidBody(player);
			player.Components.Add(body);
			float speed = 8;
			IMovement movement = new Movement(player, speed);
			movement.UpdateComponent(new GoTo(new Direction(DirectionHorisontal.Left, DirectionVertical.Down)));
			var vector = body.Body.GetLinearVelocity();
			if (vector.X != -speed || vector.Y != speed)
			{
				Assert.IsNotNull(null);
			}
		}
		
	}
}
