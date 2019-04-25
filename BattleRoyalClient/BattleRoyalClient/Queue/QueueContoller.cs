using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CommonLibrary;
using CommonLibrary.QueueMessages;

namespace BattleRoyalClient
{
    class QueueContoller
    {
		private QueueModel model;
		private BaseClient<IMessage> client;
		private Queue view;

		public IQueueModel Model
		{
			get { return model; }
		}

		public QueueContoller(BaseClient<IMessage> client, Queue view)
		{
			this.client = client;
			this.view = view;
			model = new QueueModel();
			client.EventEndSession += Client_EventEndSession;
			client.EventNewMessage += Client_EventNewMessage;
			client.SendMessage(new LoadedQueueForm());
		}

		private void Client_EventNewMessage()
		{
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesMessage.ChangeCountPlayersInQueue:
					Handler_ChangeCountPlayersInQueue((ChangeCountPlayersInQueue)msg);
					break;
				case TypesMessage.ResultRequestExit:
					Handler_ResultRequestExit(msg);
					break;
				case TypesMessage.AddInBattle:
					Handler_AddInBattle(msg);
					break;
				default:
					int a = 10;
					break;
			}
		}

		private void Handler_AddInBattle(IMessage msg)
		{
			view.Dispatcher.Invoke(()=>
			{
				client.EventEndSession -= this.Client_EventEndSession;
				client.EventNewMessage -= this.Client_EventNewMessage;
				BattleView3d battleForm = new BattleView3d(msg.ID, client);
				battleForm.Show();
				view.Transition = true;
				view.Close();
			});
		}

		private void Handler_ResultRequestExit(IMessage msg)
		{
			if (msg.Result)
			{
				view.Dispatcher.Invoke(() =>
				{
					client.EventEndSession -= this.Client_EventEndSession;
					client.EventNewMessage -= this.Client_EventNewMessage;
					Account formAccount = new Account(client);
					formAccount.Show();
					view.Transition = true;
					view.Close();
				});
			}
			else
			{
				//если попытка выхода закончилась неудачей, значит игрок уже добавлен в игровую комнату
			}
		}	

		private void Handler_ChangeCountPlayersInQueue(ChangeCountPlayersInQueue msg)
		{
			model.PlaysersInQueue = msg.Count;
			view.Dispatcher.Invoke(() =>
			{
				model.CreateChangeModel();
			});
		}

		public void Handler_SignOutOfQueue(object sender, EventArgs e)
		{
			client.SendMessage(new RequestExitOfQueue());
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
