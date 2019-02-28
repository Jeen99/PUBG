using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	class JoinToQueue : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.JoinToQueue;
	}
}
