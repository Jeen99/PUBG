using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    class AuthorizationController:IController
    {
        private ServerClient client;
        public AuthorizationController()
        {
            client = null;
        }

        public AuthorizationController(ServerClient client)
        {
            this.client = client;
			client.Controler = this;
			//привязываем обработчик только один раз
			client.EventEndSession += Handler_StandartExceptions.Handler_LostConnectServerClient;
        }

        public IController GetNewControler(ServerClient client)
        {
            return new AuthorizationController(client);
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
				new AccountController(client, msg.Login, msg.Password);
				client.SendMessage(new SuccessAuthorization());
			}
			else
			{
				client.SendMessage(new ErrorAuhorization());
			}
		}


		public override string ToString()
		{
			return "AuthorizationController";
		}
	}
}