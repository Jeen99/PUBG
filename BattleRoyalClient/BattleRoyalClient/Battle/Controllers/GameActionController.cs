using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CommonLibrary.GameMessages;
using CommonLibrary;
using CommonLibrary.InternalForClient;
using CommonLibrary.CommonElements;
using System.Drawing;
using System.Threading;

namespace BattleRoyalClient
{
	class GameActionController
	{
		private IBattleModelForController model;
		private BaseClient<IMessage> client;

		public GameActionController(BaseClient<IMessage> client, IBattleModelForController model)
		{
			this.client = client;
			this.model = model;
			client.EventEndSession += Client_EventEndSession;
			client.EventNewMessage += Client_EventNewMessage;
		}

		private void Client_EventNewMessage()
		{
			IMessage msg = client.GetRecievedMsg();
			model.AddOutgoingMsg(msg);
		}

		private void Handler_EndGame(IMessage msg)
		{
			/*view.Dispatcher.Invoke(() =>
			{
				Account formAccount = new Account(client, msg);
				formAccount.Show();
				view.Transition = true;
				view.Close();
			});*/
		}

		private void Client_EventEndSession()
		{
			model.AddOutgoingMsg(new ConnectionBroken());
			/*view.Dispatcher.Invoke(() =>
			{
				client = null;
				Autorization authorization = new Autorization();
				authorization.Show();
				view.Transition = true;
				view.Close();
			});*/
		}

	}
}
