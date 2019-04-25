using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CommonLibrary.AutorizationMessages;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    public class AuthorizationController:IController<IMessage>
    {
        private ServerClient<IMessage> client;
        public AuthorizationController()
        {
            client = null;
        }

        public AuthorizationController(ServerClient<IMessage> client)
        {
            this.client = client;
			client.Controler = this;
			//привязываем обработчик только один раз
			client.EventEndSession += Log.Handler_LostConnectServerClient;
        }

        public IController<IMessage> GetNewControler(ServerClient<IMessage> client)
        {
            return new AuthorizationController(client);
        }

		public void HanlderNewMessage()
		{
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesMessage.RequestOnAutorization:
					Handler_AuthorizationMsg(msg);
					break;
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Log.Handler_ErrorHandlingClientMsg(this.ToString(), msg.TypeMessage.ToString());
					break;
			}
		}

		public void Handler_AuthorizationMsg(IMessage msg)
		{
			if (BDAccounts.ExistAccount(msg.Login, msg.Password))
			{
				new AccountController(client, msg.Login, msg.Password);
				client.SendMessage(new ResultAuthorization(true));
			}
			else
			{
				client.SendMessage(new ResultAuthorization(false));
			}
		}


		public override string ToString()
		{
			return "AuthorizationController";
		}

		public void Dispose()
		{
			//ничего не делаем
		}
	}
}