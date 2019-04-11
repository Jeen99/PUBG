using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	public class PlayerBattleStatistics : IMessage, IOutgoing
	{
		public int Kills { get; private set; }

		public TimeSpan TimeLife { get; private set; }
		
		public bool YouDied { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.PlayerBattleStatistics;

		public ulong ID { get; private set; }

		public PlayerBattleStatistics(ulong iD, bool youDied, int kills, TimeSpan timeLife)
		{
			ID = iD;
			YouDied = youDied;
			Kills = kills;
			TimeLife = timeLife;
		}
	}
}
