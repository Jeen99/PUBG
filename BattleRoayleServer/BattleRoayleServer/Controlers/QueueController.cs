﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    class QueueController : IController
    {
		private ServerClient client;
		/// <summary>
		/// Модель игрока в игроовой очереди
		/// </summary>
		private QueueGamer gamer;

        public IController GetNewControler(ServerClient client)
        {
            throw new NotImplementedException();
        }

		public QueueController(ServerClient client, DataOfAccount data)
		{
			this.client = client;
			client.Controler = this;
			gamer = new QueueGamer(client, data);
			//добавляем игрока в очередь
			Program.QueueOfServer.AddGamer(gamer);
		}


		public void HanlderNewMessage(IMessage msg)
        {
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.DeleteOfQueue:
					Handler_DeleteOfMessage();
					break;
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Handler_StandartExceptions.Handler_ErrorHandlingClientMsg(this.ToString(),
						msg.TypeMessage.ToString());
					break;
			}
		}

		/// <summary>
		/// Обрабатывает удаление игрока из очереди
		/// </summary>
		private void Handler_DeleteOfMessage()
		{
			if (Program.QueueOfServer.DeleteOfQueue(gamer))
			{
				//удаление прошло успешно
				client.SendMessage(new SuccessExitOfQueue());
				new AccountController(gamer);
			}
			else
			{
				client.SendMessage(new FailedExitOfQueue());
			}
		}

		public override string ToString()
		{
			return "QueueController";
		}
	}
}