using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CSInteraction.ProgramMessage;

namespace BattleRoyalClient
{
	class AccountController
	{
		private AccountModel model;
		public IAccountModel Model {
			get
			{
				return model;
			}
		}
		private BaseClient client;
		private bool OnClick = false;
		private Account view;

		public AccountController(BaseClient client, Account view)
		{
			this.client = client;
			this.view = view;
			model = new AccountModel();
			client.EventEndSession += Client_EventEndSession;
			client.EventNewMessage += Client_EventNewMessage;
			this.client.SendMessage(new LoadedAccountForm());
		}

		public void InQueue(object sender, EventArgs e)
		{
			if (!OnClick)
			{
				OnClick = true;
				client.SendMessage(new JoinToQueue());
				OnClick = false;
			}
		}

		private void Client_EventNewMessage()
		{
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.DataAccount:
					Handle_DataAccount((DataAccount)msg);
					break;
				case TypesProgramMessage.JoinedToQueue:
					Handler_JoinedToQueue();
					break;
			}
		}
		private void Handler_JoinedToQueue()
		{
			view.Dispatcher.Invoke(() =>
			{
				client.EventEndSession -= this.Client_EventEndSession;
				client.EventNewMessage -= this.Client_EventNewMessage;
				Queue formQueue = new Queue(client);
				formQueue.Show();
				view.Transition = true;
				view.Close();
				
			});
		}
		private void Handle_DataAccount(DataAccount msg)
		{
			model.Kills = msg.Kills;
			model.Deaths = msg.Deaths;
			model.Battles = msg.Battles;
			model.GameTime = msg.GameTime;
			view.Dispatcher.Invoke(() =>
			{
				model.CreateChangeModel();
			});
		}
		

		private void Client_EventEndSession()
		{
			view.Dispatcher.Invoke(() =>
			{
				client = null;
				Autorization authorization = new Autorization();
				authorization.Show();
				view.Transition = true;
				view.Close();
			});
		}
	}
}
