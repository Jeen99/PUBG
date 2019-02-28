using System;
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
            throw new NotImplementedException();
        }

		
		public override string ToString()
		{
			return "QueueController";
		}
	}
}