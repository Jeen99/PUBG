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
			client.Controler = this;
			client.EventEndSession += Handler_StandartExceptions.Handler_LostConnectServerClient;
        }

        public IControler GetNewControler(ServerClient client)
        {
            return new AuthorizationControler(client);
        }

		public void HanlderNewMessage(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.Authorization:
					Handler_AuthorizationMsg((Authorization)msg);
					break;
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Handler_StandartExceptions.Handler_ErrorHandlingClientMsg(this.ToString(), msg.TypeMessage.ToString());
					break;
			}
		}

		public void Handler_AuthorizationMsg(Authorization msg)
		{
			if (BDAccounts.ExistAccount(msg.Login, msg.Password))
			{
				new AccountControler(client, msg.Login, msg.Password);
				client.SendMessage(new SuccessAuthorization());
			}
			else
			{
				client.SendMessage(new ErrorAuhorization());
			}
		}


		public override string ToString()
		{
			return "AuthorizationControler";
		}
	}
}