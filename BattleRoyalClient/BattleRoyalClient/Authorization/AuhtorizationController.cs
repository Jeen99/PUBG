using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CSInteraction.Client;
using System.Threading;
using CommonLibrary.AutorizationMessages;

namespace BattleRoyalClient
{
	public class AuthorizationController
	{
		private AuthorizationModel model;
		//чтобы не обрабатывать большо одного нажатия кнопки одновременно
		private bool PerfomConnect = false;
		private BaseClient<IMessage> client;
		private BattleRoyalClient.Autorization view;

		public IAuthorizationModel Model {
			get
			{
				return model;
			}
		}
		public void SignIn()
		{
			if(!PerfomConnect)
			{
				PerfomConnect = true;
				if (client == null)
				{
					client = LoadNetworkSettings();
					client.EventNewMessage += Client_EventNewMessage;
					client.EventEndSession += Client_EventEndSession;
					
				}

				client.ConnectToServer();

				if (client.Status == StatusClient.Connect)
				{
					client.SendMessage(new RequestOnAutorization(model.NickName, model.Password));
				}
				else
				{
					model.State = StatesAutorizationModel.ErrorConnect;
					view.Dispatcher.Invoke(() =>
					{
						model.CreateModelChange();
					});
				}
				PerfomConnect = false;
			}
		}

		private void Client_EventEndSession()
		{
			//попробуем создать новый клиент
			if (client != null)
				client.Close();
			client = null;
		}

		private void Client_EventNewMessage(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesMessage.ResultAutorization:
					Handler_ResultAutorization(msg);
					break;
			}
		}

		private void Handler_ResultAutorization(IMessage msg)
		{
			if (msg.Result)
			{
				view.Dispatcher.Invoke(() =>
				{
					client.EventNewMessage -= this.Client_EventNewMessage;
					client.EventEndSession -= this.Client_EventEndSession;
					Account account = new Account(client);
					account.Show();
					view.Transition = true;
					view.Close();
				});
			}
			else
			{
				model.State = StatesAutorizationModel.IncorrectData;
				view.Dispatcher.Invoke(() =>
				{
					model.CreateModelChange();
				});
			}
			
		}

		/// <summary>
		/// Загружает сетевый настройки из файла и создает объект BaseClient
		/// </summary>
		private BaseClient<IMessage> LoadNetworkSettings()
		{
			return new BaseClient<IMessage>("127.0.0.1", 11000);
		}
		public AuthorizationController(Autorization view)
		{
			model = new AuthorizationModel();
			this.view = view;
		}

		public string NickName
		{
			get { return model.NickName; }
			set { model.NickName = value; }
		}

		public string Password
		{
			get { return model.Password; }
			set { model.Password = value; }
		}

	}
}