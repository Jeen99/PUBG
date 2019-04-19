using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class ChangeCountPlayersInGame : IMessage,IOutgoing
	{
		public int Count { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ChangeCountPlayersInGame;

		public ulong ID { get; private set; }

		public ChangeCountPlayersInGame(ulong iD, int count)
		{
			ID = iD;
			Count = count;
		}
	}
}
