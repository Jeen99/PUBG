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
		public BaseClient<IMessage> Client { get; private set }

		public GameActionController(BaseClient<IMessage> client, IBattleModelForController model)
		{
			this.Client = client;
			this.model = model;
			client.EventEndSession += Client_EventEndSession;
			client.EventNewMessage += Client_EventNewMessage;
		}

		private void Client_EventNewMessage()
		{
			IMessage msg = Client.GetRecievedMsg();
			model.AddOutgoingMsg(msg);
		}

		private void Client_EventEndSession()
		{
			model.AddOutgoingMsg(new ConnectionBroken());
		}

	}
}
