using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CommonLibrary.AutorizationMessages;
using CSInteraction;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    public class AuthorizationController:IController<IMessage>
    {
        private ConnectedClient<IMessage> client;
        public AuthorizationController()
        {
            client = null;
        }

        public AuthorizationController(ConnectedClient<IMessage> client)
        {
            this.client = client;
			client.Controler = this;
			//привязываем обработчик только один раз
			client.EventEndSession += Log.Handler_LostConnectServerClient;
        }

        IController<IMessage> IController<IMessage>.GetNewControler(ConnectedClient<IMessage> client)
        {
            return new AuthorizationController(client);
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

		void IController<IMessage>.Hanlder_NewMessage()
		{
			IMessage msg = client.GetRecievedMsg();
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

	}
}