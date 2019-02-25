using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    public class AccountControler : IControler
	{
		private ServerClient client;

		public IControler GetNewControler(ServerClient client)
        {
            throw new NotImplementedException();
        }

        public void HanlderNewMessage(IMessage msg)
        {
            throw new NotImplementedException();
        }

		private void HandlerDisconnectClient(ServerClient client)
		{
			throw new System.NotImplementedException();
		}

		public AccountControler(ServerClient client, string Login, string password)
		{

		}

		public AccountControler(QueueGamer gamer)
		{

		}
	}
}