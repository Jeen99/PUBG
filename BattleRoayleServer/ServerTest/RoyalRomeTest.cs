using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Collections.Generic;
using CSInteraction.Server;
using System.Net.Sockets;
using System.Threading;

namespace ServerTest
{
	[TestClass]
	public class RoyalRomeTest
	{
		[TestMethod]
		public void Test_CreateAndDisposeRoom()
		{
			Rooms rooms = new Rooms();
			int count = rooms.CollectionRooms.Count;

			//создаем комнату с одним игроком
			rooms.AddRoom(new List<QueueGamer>() {
				new QueueGamer(
					new ServerClient(new TcpClient(), new AuthorizationController()), 
					new  DataOfAccount("", "", 0, 0, 0, new TimeSpan()))
			});

			Assert.AreEqual(count+1, rooms.CollectionRooms.Count);
			RoyalRoom royalRoom = (RoyalRoom)rooms.CollectionRooms[0];
			GameObject player = (GameObject)royalRoom.GameLogic.Players[0];
			player.Dispose();
			Thread.Sleep(100);
			Assert.AreEqual(count, rooms.CollectionRooms.Count);
		}
	}
}
