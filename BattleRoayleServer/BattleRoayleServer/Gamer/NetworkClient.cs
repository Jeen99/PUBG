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
		public event NetorkClientDisconnect EventNetorkClientDisconnect;
		public event GetViewMsg Event_GetViewMsg;

		public string Nick { get; private set; }

		public ServerClient Client { get; private set; }

		public string Password { get; private set; }

		private const int widthVisibleArea = 160;
		private const int heightVisibleArea = 160;
		private RectangleF visibleArea;

		public RectangleF VisibleArea
		{
			get
			{
				//определяем положение, чтобы игрок был примерно в центре видимой области
				var location = new PointF(Player.Location.X - widthVisibleArea/2, Player.Location.Y - heightVisibleArea / 2);
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

		public void HanlderNewMessage()
		{
			IMessage msg = Client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.LoadedBattleForm:
					Handler_LoadedBattleForm();
					break;
				case TypesProgramMessage.PlayerTurn:
					Event_GetViewMsg?.Invoke(Player.ID, msg);
					break;
				default:
					Player.Update(msg);
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

		/// <summary>
		/// Cохраняем результаты битвы в базе данных
		/// </summary>
		public void SaveStatistics(EndGame msg)
		{
			int deaths;
			if (msg.YouDied) deaths = 1;
			else deaths = 0;
			BDAccounts.AddToStatistic(new DataOfAccount(Nick, Password, msg.Kills, deaths, 1, msg.TimeLife));
		}
	}
}