using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
	public class QueueController : IController
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
			client.EventEndSession += Client_EventEndSession;
			client.SendMessage(new JoinedToQueue());
			gamer = new QueueGamer(client, data);
		}

		private void Client_EventEndSession(ServerClient Client)
		{
			Program.QueueOfServer.DeleteOfQueue(gamer);
		}

		public void HanlderNewMessage()
		{
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.DeleteOfQueue:
					Handler_DeleteOfMessage();
					break;
				case TypesProgramMessage.LoadedQueueForm:
					Handler_LoadedQueueForm();
					break;
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Log.Handler_ErrorHandlingClientMsg(this.ToString(),
						msg.TypeMessage.ToString());
					break;
			}
		}
		public void Handler_LoadedQueueForm()
		{
			//добавляем игрока в очередь
			Program.QueueOfServer.AddGamer(gamer);
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

		public void Dispose()
		{
			//отвязываем дополнительный обраотчик
			client.EventEndSession += Client_EventEndSession;
		}
	}
}