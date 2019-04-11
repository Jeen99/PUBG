using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class ChangedTimeTillReduction : IMessage, IOutgoing
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ChangedTimeTillReduction;

		public ulong ID { get; private set; }

		public TimeSpan NewTime { get; private set; }

		public ChangedTimeTillReduction(ulong iD, TimeSpan newTime)
		{
			ID = iD;
			NewTime = newTime;
		}
	}
}
