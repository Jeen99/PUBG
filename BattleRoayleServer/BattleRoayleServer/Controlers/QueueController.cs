using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary.QueueMessages;
using CommonLibrary;
using CSInteraction.Server;

namespace BattleRoayleServer
{
	public class QueueController : IController<IMessage>
	{
		private ServerClient<IMessage> client;
		/// <summary>
		/// Модель игрока в игроовой очереди
		/// </summary>
		private QueueGamer gamer;

		public IController<IMessage> GetNewControler(ServerClient<IMessage> client)
		{
			throw new NotImplementedException();
		}

		public QueueController(ServerClient<IMessage> client, DataOfAccount data)
		{
			this.client = client;
			client.Controler = this;
			client.EventEndSession += Client_EventEndSession;
			client.SendMessage(new RequestJoinToQueue());
			gamer = new QueueGamer(client, data);
		}

		private void Client_EventEndSession(ServerClient<IMessage> Client)
		{
			Program.QueueOfServer.DeleteOfQueue(gamer);
		}

		public void HanlderNewMessage()
		{
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesMessage.RequestExitOfQueue:
					Handler_DeleteOfMessage();
					break;
				case TypesMessage.LoadedQueueForm:
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
				client.SendMessage(new ResultRequestExit(false));
				new AccountController(gamer);
			}
			else
			{
				client.SendMessage(new ResultRequestExit(false));
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