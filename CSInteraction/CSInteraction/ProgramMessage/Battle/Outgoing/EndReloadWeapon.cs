using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class EndReloadWeapon : IMessage, IOutgoing 
	{
		public ulong ID { get; private set; }

		public EndReloadWeapon(ulong iD)
		{
			ID = iD;
		}

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.EndRelaodWeapon;
	}
}
