using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class EndGame : IMessage, IOutgoing
	{
		public ulong ID { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.EndGame;

		public EndGame(ulong iD)
		{
			ID = iD;
		}
	}
}
