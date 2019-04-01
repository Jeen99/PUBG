using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
	public class NetworkClient : INetworkClient, IController
	{
		public IPlayer Player { get; private set; }
		public event GamerIsLoaded Event_GamerIsLoaded;
		public event NetworkClientEndWork EventNetworkClientEndWork;
		public event NetorkClientDisconnect EventNetorkClientDisconnect;

		public string Nick { get; private set; }

		public ServerClient Client { get; private set; }

		public string Password { get; private set; }

		private const int widthVisibleArea = 100;
		private const int heightVisibleArea = 100;
		private RectangleF visibleArea;

		public RectangleF VisibleArea
		{
			get
			{
				var location = Player.Location;
				if (location.X != visibleArea.X || location.Y != visibleArea.Y)
				{
					visibleArea.Location = location;
				}
				return visibleArea;
			}
		}

		public NetworkClient(IPlayer gamerRoomLogic, ServerClient gamer, string nick, string password)
		{
			this.Player = gamerRoomLogic;
			this.Player.EventPlayerDeleted += GamerRoomLogic_EventPlayerDeleted;
			visibleArea = new RectangleF(0,0, widthVisibleArea, heightVisibleArea);
			Nick = nick;
			Client = gamer;
			Client.Controler = this;
			Client.EventEndSession += Client_EventEndSession;
			Password = password;
			//посылаем сообщение о том, что игрок добавлен в игровую комнату
			gamer.SendMessage(new AddInBattle(gamerRoomLogic.ID));

		}
		//игрок вышел из игры до завершения игры
		private void Client_EventEndSession(ServerClient Client)
		{
			EventNetorkClientDisconnect?.Invoke(this);
		}

		private void GamerRoomLogic_EventPlayerDeleted(IPlayer player)
		{
			EventNetworkClientEndWork?.Invoke(this);
		}

		public void HanlderNewMessage()
		{
			IMessage msg = Client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.LoadedBattleForm:
					Handler_LoadedBattleForm();
					break;
				default:
					Player.PerformAction(msg);
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
			Client.EventEndSession -= Client_EventEndSession;
			//отправляем игроку сообщение о переходе в окно аккаунта
			//переходим в окно аккаунта
			new AccountController(Client, Nick, Password);
		}
	}
}