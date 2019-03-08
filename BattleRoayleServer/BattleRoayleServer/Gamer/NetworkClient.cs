﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
	public class NetworkClient : INetworkClient, IController
	{
		private IPlayer gamerRoomLogic;

		public string Nick { get; private set; }

		public ServerClient Gamer { get; private set; }

		public string Password { get; private set; }

		public byte ID { get; private set; } 

		public NetworkClient(IPlayer gamerRoomLogic, ServerClient gamer, string nick, string password, byte id)
		{
			this.gamerRoomLogic = gamerRoomLogic;
			Nick = nick;
			Gamer = gamer;
			Gamer.Controler = this;
			Password = password;
			ID = id;

		}

		public void HanlderNewMessage()
		{
			IMessage msg = Gamer.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Handler_StandartExceptions.Handler_ErrorHandlingClientMsg(this.ToString(), msg.TypeMessage.ToString());
					break;
			}
		}

		public IController GetNewControler(ServerClient client)
		{
			throw new Exception("Данный класс не может реализовать данную функцию");
		}

		public override string ToString()
		{
			return "NetworkClient";
		}

		public void Dispose()
		{
			//ничего не делаем;
		}
	}
}