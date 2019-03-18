using System;
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
		public event GamerIsLoaded Event_GamerIsLoaded;
		public string Nick { get; private set; }

		public ServerClient Gamer { get; private set; }

		public string Password { get; private set; }

		public NetworkClient(IPlayer gamerRoomLogic, ServerClient gamer, string nick, string password, byte id)
		{
			this.gamerRoomLogic = gamerRoomLogic;
			Nick = nick;
			Gamer = gamer;
			Gamer.Controler = this;
			Password = password;
			//посылаем сообщение о том, что игрок добавлен в игровую комнату
			gamer.SendMessage(new AddInBattle(gamerRoomLogic.ID));

		}

		public void HanlderNewMessage()
		{
			IMessage msg = Gamer.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.LoadedBattleForm:
					Handler_LoadedBattleForm();
					break;
				default:
					gamerRoomLogic.PerformAction(msg);
					break;
			}
					
		}
		public void Handler_LoadedBattleForm()
		{
			Event_GamerIsLoaded(this);
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