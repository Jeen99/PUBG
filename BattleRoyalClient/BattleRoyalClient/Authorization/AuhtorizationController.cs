using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Client;
using System.Threading;

namespace BattleRoyalClient
{
	public class AuthorizationController
	{
		private AuthorizationModel model;
		//чтобы не обрабатывать большо одного нажатия кнопки одновременно
		private bool PerfomConnect = false;
		private BaseClient client;
		private BattleRoyalClient.Autorization view;

		public IAuthorizationModel Model {
			get
			{
				return model;
			}
		}
		public void SignIn(object sender, EventArgs e)
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
				if (client.ConnectToServer())
				{
					
					client.SendMessage(new CSInteraction.ProgramMessage.Authorization(model.NickName, model.Password));
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
			client = null;
		}

		private void Client_EventNewMessage()
		{
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.ErrorAuhorization:
					Handler_ErrorAuhorization();
					break;
				case TypesProgramMessage.SuccessAuthorization:
					Handler_SuccessAuthorization();
					break;
			}
		}

		private void Handler_ErrorAuhorization()
		{

			model.State = StatesAutorizationModel.IncorrectData;
			view.Dispatcher.Invoke(() =>
			{
				model.CreateModelChange();
			});
		}

		private void Handler_SuccessAuthorization()
		{
			view.Dispatcher.Invoke(() =>
			{
				client.EventNewMessage -= this.Client_EventNewMessage;
				client.EventEndSession -= this.Client_EventEndSession;
				Account account = new Account(client, model.NickName, model.Password);
				account.Show();
				view.Transition = true;
				view.Close();
			});
		}
		/// <summary>
		/// Загружает сетевый настройки из файла и создает объект BaseClient
		/// </summary>
		private BaseClient LoadNetworkSettings()
		{
			return new BaseClient("127.0.0.1", 11000);
		}
		public AuthorizationController(Autorization view)
		{
			model = new AuthorizationModel();
			this.view = view;
		}

		public string NickName
		{
			get{ return model.NickName;}
			set{model.NickName = value;}
		}

		public string Password
		{
			get { return model.Password; }
			set { model.Password = value; }
		}

	}
}