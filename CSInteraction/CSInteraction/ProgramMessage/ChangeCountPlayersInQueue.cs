﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	class ChangeCountPlayersInQueue:IMessage
	{
		/// <summary>
		/// Игроков в очереди на момент подключения
		/// </summary>
		public int PlayersInQueue { get; private set; }

		public ChangeCountPlayersInQueue(int playersInQueue)
		{
			PlayersInQueue = playersInQueue;
		}

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ChangeCountPlayersInQueue;
	}
}