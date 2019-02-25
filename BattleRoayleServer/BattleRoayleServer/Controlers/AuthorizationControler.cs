using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    class AuthorizationControler:IControler
    {
        private ServerClient client;
        public AuthorizationControler()
        {
            client = null;
        }

        public AuthorizationControler(ServerClient client)
        {
            this.client = client;
        }

        public IControler GetNewControler(ServerClient client)
        {
            return new AuthorizationControler(client);
        }

		public void HanlderNewMessage(IMessage msg)
		{

		}

		public void HandlerDisconnectClient(ServerClient client)
		{
			throw new System.NotImplementedException();
		}
	}
}