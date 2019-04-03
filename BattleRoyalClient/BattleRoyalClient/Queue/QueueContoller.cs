using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CSInteraction.ProgramMessage;

namespace BattleRoyalClient
{
    class QueueContoller
    {
		private QueueModel model;
		private BaseClient client;
		private Queue view;

		public IQueueModel Model
		{
			get { return model; }
		}

		public QueueContoller(BaseClient client, Queue view)
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
				case TypesProgramMessage.ChangeCountPlayersInQueue:
					Handler_ChangeCountPlayersInQueue((ChangeCountPlayersInQueue)msg);
					break;
				case TypesProgramMessage.SuccessExitOfQueue:
					Handler_SuccessExitOfQueue();
					break;
				case TypesProgramMessage.FailedExitOfQueue:
					Handle_FailedExitOfQueue();
					break;
				case TypesProgramMessage.AddInBattle:
					Handler_AddInBattle((AddInBattle)msg);
					break;
				default:
					int a = 10;
					break;
			}
		}

		private void Handler_AddInBattle(AddInBattle msg)
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

		private void Handler_SuccessExitOfQueue()
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
		private void Handle_FailedExitOfQueue()
		{
			//если попытка выхода закончилась неудачей, значит игрок уже добавлен в игровую комнату
		}

		private void Handler_ChangeCountPlayersInQueue(ChangeCountPlayersInQueue msg)
		{
			model.PlaysersInQueue = msg.PlayersInQueue;
			view.Dispatcher.Invoke(() =>
			{
				model.CreateChangeModel();
			});
		}

		public void Handler_SignOutOfQueue(object sender, EventArgs e)
		{
			client.SendMessage(new DeleteOfQueue());
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
