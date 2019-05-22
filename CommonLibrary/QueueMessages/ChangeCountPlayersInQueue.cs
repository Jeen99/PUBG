using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.QueueMessages
{
	[Serializable]
	public class ChangeCountPlayersInQueue: Message
	{
		/// <summary>
		/// Игроков в очереди на момент подключения
		/// </summary>
		public override int Count { get; }

		public ChangeCountPlayersInQueue(int playersInQueue)
		{
			Count = playersInQueue;
		}

		public override TypesMessage TypeMessage { get; } = TypesMessage.ChangeCountPlayersInQueue;
	}
}
