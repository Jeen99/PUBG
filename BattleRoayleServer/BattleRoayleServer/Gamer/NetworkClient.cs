using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CommonLibrary.GameMessages;
using CommonLibrary;
using CSInteraction.Server;

namespace BattleRoayleServer
{
	public class NetworkClient : INetworkClient, IController<IMessage>
	{
		public IPlayer Player { get; private set; }
		public event GamerIsLoaded Event_GamerIsLoaded;
		public event NetorkClientDisconnect EventNetorkClientDisconnect;

		public string Nick { get; private set; }

		public ConnectedClient<IMessage> Client { get; private set; }

		public string Password { get; private set; }

		private const int widthVisibleArea = 160;
		private const int heightVisibleArea = 160;
		private RectangleF visibleArea;
		private IGameModel model;

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

		public NetworkClient(IGameModel model, int index, ConnectedClient<IMessage> client, string nick, string password)
		{
			this.model = model;
			this.Player = model.Players[index];
			visibleArea = new RectangleF(0, 0, widthVisibleArea, heightVisibleArea);
			Nick = nick;
			Client = client;
			Client.Controler = this;
			Client.EventEndSession += Client_EventEndSession;
			Password = password;
			//посылаем сообщение о том, что игрок добавлен в игровую комнату
			Client.SendMessage(new AddInBattle(Player.ID));

		}
		//игрок вышел из игры до завершения игры
		private void Client_EventEndSession(ConnectedClient<IMessage> Client)
		{
			Client.Close();
			EventNetorkClientDisconnect?.Invoke(this);
		}

		void IController<IMessage>.Hanlder_NewMessage()
		{
			IMessage msg = Client.GetRecievedMsg();
			switch (msg.TypeMessage)
			{
				case TypesMessage.LoadedBattleForm:
					Handler_LoadedBattleForm();
					break;
				default:
					model.AddIncomingMessage(msg);
					break;
			}
					
		}
		public void Handler_LoadedBattleForm()
		{
			Event_GamerIsLoaded(this);
		}

		IController<IMessage> IController<IMessage>.GetNewControler(ConnectedClient<IMessage> client)
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
		public void SaveStatistics(IMessage msg)
		{
			int deaths;
			if (msg.Result) deaths = 1;
			else deaths = 0;
			BDAccounts.AddToStatistic(new DataOfAccount(Nick, Password, msg.Kills, deaths, 1, msg.Time));
		}
	}
}