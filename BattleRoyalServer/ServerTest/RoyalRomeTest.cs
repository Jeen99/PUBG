using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using System.Collections.Generic;
using CSInteraction.Server;
using System.Net.Sockets;
using System.Threading;
using CSInteraction;
using CommonLibrary;

namespace ServerTest
{
	[TestClass]
	public class RoyalRomeTest
	{
		[TestMethod]
		public void Test_CreateAndDisposeRoom()
		{
#warning Нужно использовать stub и mock объекты!
			Rooms rooms = new Rooms();
			int count = rooms.CollectionRooms.Count;

			//создаем комнату с одним игроком
			rooms.AddRoom(new List<QueueGamer>() {
				new QueueGamer(
					new ConnectedClient<IMessage>(new TcpClient(), new AuthorizationController()),
					new  DataOfAccount("", "", 0, 0, 0, new TimeSpan())),
				new QueueGamer(
					new ConnectedClient<IMessage>(new TcpClient(), new AuthorizationController()),
					new  DataOfAccount("", "", 0, 0, 0, new TimeSpan()))
			});

			Assert.AreEqual(count + 1, rooms.CollectionRooms.Count);
			RoyalRoom royalRoom = (RoyalRoom)rooms.CollectionRooms[0];
			GameObject player = (GameObject)royalRoom.GameLogic.RoomModel.Players[0];
			player.SetDestroyed();
			royalRoom.GameLogic.RoomModel.MakeStep(1000);
			Thread.Sleep(500);
			Assert.AreEqual(count, rooms.CollectionRooms.Count);
		}
	}
}
