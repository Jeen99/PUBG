using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	class JoinedToQueue : IMessage
	{
		/// <summary>
		/// Игроков в очереди на момент подключения
		/// </summary>
		public int PlayersInQueue { get; private set; }
		public TypesProgramMessage TypeMessage { get; private set; } = TypesProgramMessage.JoinedToQueue;
		public JoinedToQueue(int countPlayers)
		{
			PlayersInQueue = countPlayers;
		}

	}
}
