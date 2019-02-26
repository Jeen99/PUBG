using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    class QueueControler : IControler
    {
        public IControler GetNewControler(ServerClient client)
        {
            throw new NotImplementedException();
        }

        public void HanlderNewMessage(IMessage msg)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Удаляем игрока из очереди
		/// </summary>
		/// <param name="client"></param>
		public void HandlerDisconnectClient(ServerClient client)
		{
			throw new System.NotImplementedException();
		}

		public override string ToString()
		{
			return "QueueControler";
		}
	}
}