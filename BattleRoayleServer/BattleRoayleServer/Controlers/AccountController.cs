using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    public class AccountController : IController
	{
		private ServerClient client;
		/// <summary>
		/// Модель игрового меню 
		/// </summary>
		private DataOfAccount data;

		public IController GetNewControler(ServerClient client)
        {
			return new AccountController(client);
		}

        public void HanlderNewMessage(IMessage msg)
        {
			switch (msg.TypeMessage)
			{
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Handler_StandartExceptions.Handler_ErrorHandlingClientMsg(this.ToString(), 
						msg.TypeMessage.ToString());
					break;
			}
        }


		public AccountController(ServerClient client)
		{
			this.client = client;
		}

		public AccountController(ServerClient client, string login, string password)
		{
			this.client = client;
			client.Controler = this;

			//получаем данные об аккаунте
			data = BDAccounts.GetDataOfAccount(login, password);
			if (data != null)
			{
				//отправляем данные об аккаунте
				client.SendMessage(new DataAccount(data.QuantityKills,data.QuentityBattles,
					data.QuentityDeaths, data.QuentityGameTime));
			}
		}

		public AccountController(QueueGamer gamer)
		{

		}

		public override string ToString()
		{
			return "AccountController";
		}
	}
}