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
		private IQueueModelForController model;
		public BaseClient<IMessage> Client { get; private set; }

		public QueueContoller(BaseClient<IMessage> client, IQueueModelForController model)
		{
			this.Client = client;
			this.model = model;
			client.EventEndSession += Client_EventEndSession;
			client.EventNewMessage += Client_EventNewMessage;
			client.SendMessage(new LoadedQueueForm());
		}

		private void Client_EventNewMessage(IMessage msg)
		{			
			switch (msg.TypeMessage)
			{
				case TypesMessage.AddInBattle:
					DeleteHandlers();
					break;
				case TypesMessage.ResultRequestExit:
					if (msg.Result)
					{
						DeleteHandlers();
					}
					break;
			}

			model.Update(msg);
		}

		private void DeleteHandlers()
		{
			Client.EventEndSession -= this.Client_EventEndSession;
			Client.EventNewMessage -= this.Client_EventNewMessage;
		}

		public void Handler_SignOutOfQueue(object sender, EventArgs e)
		{
			Client.SendMessage(new RequestExitOfQueue());
		}

		private void Client_EventEndSession()
		{
			model.HappenedLossConnectToServer();
		}
		public void ViewClose()
		{
			model.ClearModel();
		}
	}
}
