using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage.Queue
{	[Serializable]
	public class SuccessExitOfQueuecs : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.SuccessExitOfQueuecs;
	}
}
