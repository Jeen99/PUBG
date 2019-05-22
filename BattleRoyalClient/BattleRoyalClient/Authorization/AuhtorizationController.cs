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
		private IAuthorizationModelForController model;
		//чтобы не обрабатывать больше одного нажатия кнопки одновременно
		private bool PerfomConnect = false;
		public BaseClient<IMessage> Client { get; private set; }

		public void SignIn()
		{
			if(!PerfomConnect)
			{
				PerfomConnect = true;
				if (Client == null)
				{
					Client = LoadNetworkSettings();
					Client.EventNewMessage += Client_EventNewMessage;
					Client.EventEndSession += Client_EventEndSession;				
				}

				Client.ConnectToServer();

				if (Client.Status == StatusClient.Connect)
				{
					Client.SendMessage(new RequestOnAutorization(model.NickName, model.Password));
				}
				else
				{
					model.HappenedLossConnectToServer();
				}
				PerfomConnect = false;
			}
		}

		private void Client_EventEndSession()
		{
			//попробуем создать новый клиент
			if (Client != null)
				Client.Close();
			Client = null;
		}

		private void Client_EventNewMessage(IMessage msg)
		{
			
			switch(msg.TypeMessage)
			{
				case TypesMessage.ResultAutorization:
					Handler_ResultAutorization(msg);
					break;
			}
			model.Update(msg);
		}

		private void Handler_ResultAutorization(IMessage msg)
		{
			if (msg.Result)
			{
				Client.EventNewMessage -= this.Client_EventNewMessage;
				Client.EventEndSession -= this.Client_EventEndSession;
			}		
		}

		/// <summary>
		/// Загружает сетевый настройки из файла и создает объект BaseClient
		/// </summary>
		private BaseClient<IMessage> LoadNetworkSettings()
		{
			return new BaseClient<IMessage>("127.0.0.1", 11000);
		}

		public AuthorizationController(IAuthorizationModelForController model)
		{
			this.model = model;
		}

		public void ChangeNickName(string nickName)
		{
			model.NickName = nickName;
		}

		public void ChangePassword(string nickName)
		{
			model.Password = nickName;
		}

		public void MarkSaveAutorizationData()
		{
			model.ChangeSaveAutorizationData();
		}

		public void ViewIsLoad()
		{
			model.Load();
		}

		public void ViewIsClose()
		{
			model.SaveAndClearModel();
		}
	}
}