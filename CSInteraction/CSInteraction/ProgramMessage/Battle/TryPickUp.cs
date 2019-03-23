using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage.Battle
{
	[Serializable]
	public class TryPickUp : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.TryPickUp;
	}
}
