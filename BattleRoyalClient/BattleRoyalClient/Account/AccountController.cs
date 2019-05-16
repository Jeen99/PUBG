using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CommonLibrary;
using CommonLibrary.AccountMessages;
using CommonLibrary.QueueMessages;


namespace BattleRoyalClient
{
	class AccountController
	{
		private IAccountModelForController model;
		
		public BaseClient<IMessage> Client { get; private set; }
		private bool OnClick = false;

		public AccountController(BaseClient<IMessage> client, IAccountModelForController model)
		{
			this.Client = client;
			this.model = model;
			model = new AccountModel();
			client.EventEndSession += Client_EventEndSession;
			client.EventNewMessage += Client_EventNewMessage;
			this.Client.SendMessage(new LoadedAccountForm());
		}

		public void InQueue(object sender, EventArgs e)
		{
			if (!OnClick)
			{
				OnClick = true;
				Client.SendMessage(new RequestJoinToQueue());
				OnClick = false;
			}
		}

		private void Client_EventNewMessage(IMessage msg)
		{			
			switch (msg.TypeMessage)
			{
				case TypesMessage.RequestJoinToQueue:
					Handler_JoinedToQueue();
					break;
			}
			model.Update(msg);
		}
		private void Handler_JoinedToQueue()
		{
			Client.EventEndSession -= this.Client_EventEndSession;
			Client.EventNewMessage -= this.Client_EventNewMessage;
		}
	
		private void Client_EventEndSession()
		{
			model.HappenedLossConnectToServer();
		}

		public void ViewIsClose()
		{
			model.ClearModel();
		}
	}
}
