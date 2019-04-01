using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class ChangedValueHP:IMessage, IOutgoing
	{
		public ChangedValueHP(ulong iD, float newValueHP)
		{
			ID = iD;
			NewValueHP = newValueHP;
		}

		public ulong ID { get; private set; }

		public float NewValueHP { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ChangedValueHP;

		
	}
}
