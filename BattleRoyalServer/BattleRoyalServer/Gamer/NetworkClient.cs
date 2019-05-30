using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CommonLibrary.GameMessages;
using CommonLibrary;
using CSInteraction.Server;

namespace BattleRoyalServer
{
	public class NetworkClient : INetworkClient, IController<IMessage>
	{
		public IPlayer Player { get; private set; }
		public event GamerIsLoaded Event_GamerIsLoaded;
		public event NetorkClientDisconnect EventNetorkClientDisconnect;

		public string Nick { get; private set; }
		public string Password { get; private set; }

		private readonly ConnectedClient<IMessage> _client;

		private readonly VisibleArea _visibleArea;
		public RectangleF VisibleArea => _visibleArea.Area;

		private readonly IGameModel _model;

		public NetworkClient(IGameModel model, int index, ConnectedClient<IMessage> client, string nick, string password)
		{
			this._model = model;
			this.Player = model.Players[index];
			this._visibleArea = new VisibleArea(Player);

			Nick = nick;
			Password = password;

			this._client = client;
			this._client.Controler = this;
			this._client.EventEndSession += Client_EventEndSession;
			//посылаем сообщение о том, что игрок добавлен в игровую комнату
			this._client.SendMessage(new AddInBattle(Player.ID));

		}
		//игрок вышел из игры до завершения игры
		private void Client_EventEndSession(ConnectedClient<IMessage> Client)
		{
			Client.Close();
			EventNetorkClientDisconnect?.Invoke(this);
		}

		void IController<IMessage>.Hanlder_NewMessage(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesMessage.LoadedBattleForm:
					Handler_LoadedBattleForm();
					break;
				default:
					msg.ID = Player.ID;
					_model.AddIncomingMessage(msg);
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
			_client.EventEndSession -= Client_EventEndSession;
			//отправляем игроку сообщение о переходе в окно аккаунта
			//переходим в окно аккаунта
			new AccountController(_client, Nick, Password);
		}

		/// <summary>
		/// Cохраняем результаты битвы в базе данных
		/// </summary>
		public void SaveStatistics(IMessage msg)
		{
			int deaths;
			deaths = msg.Result ? 1 : 0;
			BDAccounts.AddToStatistic(new DataOfAccount(Nick, Password, msg.Kills, deaths, 1, msg.Time));
		}

		public void SendMessgaeToClient(IMessage msg)
		{
			_client.SendMessage(msg);
		}
	}
}