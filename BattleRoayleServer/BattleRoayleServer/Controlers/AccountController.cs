﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary.GameMessages;
using CommonLibrary;
using CSInteraction.Server;
using CommonLibrary.AccountMessages;

namespace BattleRoayleServer
{
    public class AccountController: IController<IMessage>
	{
		private ServerClient<IMessage> client;
		/// <summary>
		/// Модель игрового меню 
		/// </summary>
		private DataOfAccount data;

		public IController<IMessage> GetNewControler(ServerClient<IMessage> client)
        {
			return new AccountController(client);
		}

        public void HanlderNewMessage()
        {
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesMessage.RequestJoinToQueue:
					Handler_JoinToQueue();
					break;
				case TypesMessage.LoadedAccountForm:
					Handler_LoadedAccountForm();
					break;
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Log.Handler_ErrorHandlingClientMsg(this.ToString(), 
						msg.TypeMessage.ToString());
					break;
			}
        }

		private void Handler_LoadedAccountForm()
		{
			if (data != null)
			{
				//отправляем данные об аккаунте
				client.SendMessage(new DataAccount(data.QuantityKills, data.QuantityDeaths,
					data.QuantityBattles, data.GetTimeInGame()));
			}
		}
		/// <summary>
		/// Обработчик сообщения от клинта JoinToQueue
		/// </summary>
		private void Handler_JoinToQueue()
		{
			new QueueController(client, data);
		}

		public AccountController(ServerClient<IMessage> client)
		{
			this.client = client;
		}

		public AccountController(ServerClient<IMessage> client, string login, string password)
		{
			this.client = client;
			client.Controler = this;
			InitializeDataAboutClient(login, password);
		}

		/// <summary>
		/// получает данные о клиенте и записывает их в поле data
		/// </summary>
		private void InitializeDataAboutClient(string login,string password)
		{
			//получаем данные об аккаунте
			data = BDAccounts.GetDataOfAccount(login, password);
		}

		public AccountController(QueueGamer gamer)
		{
			client = gamer.Client;
			client.Controler = this;
			InitializeDataAboutClient(gamer.NickName, gamer.Password);
		}

		public override string ToString()
		{
			return "AccountController";
		}

		public void Dispose()
		{
			//ничего не делаем
		}
	}
}