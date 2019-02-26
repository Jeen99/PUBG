﻿using System;
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
		/// <summary>
		/// Модель игрового меню 
		/// </summary>
		private DataOfAccount data;

		public IControler GetNewControler(ServerClient client)
        {
			return new AccountControler(client);
		}

        public void HanlderNewMessage(IMessage msg)
        {
			switch (msg.TypeMessage)
			{
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					break;
			}
        }

		private void HandlerDisconnectClient(ServerClient client)
		{
			//отвязваемся от обработки данного клиента
			client.Controler = null;
		}

		public AccountControler(ServerClient client)
		{
			this.client = client;
		}

		public AccountControler(ServerClient client, string login, string password)
		{
			this.client = client;
			client.Controler = this;
			client.EventEndSession += HandlerDisconnectClient;

			//получаем данные об аккаунте
			data = BDAccounts.GetDataOfAccount(login, password);
			if (data != null)
			{
				//отправляем данные об аккаунте
				client.SendMessage(new DataAccount(data.QuantityKills,data.QuentityBattles,
					data.QuentityDeaths, data.QuentityGameTime));
			}
		}

		public AccountControler(QueueGamer gamer)
		{

		}
	}
}