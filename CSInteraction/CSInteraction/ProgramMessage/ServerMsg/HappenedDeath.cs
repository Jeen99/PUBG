using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	public class HappenedDeath : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.HappenedDeath;

	}
}
