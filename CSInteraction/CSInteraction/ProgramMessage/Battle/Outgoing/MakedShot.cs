using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class MakedShot : IMessage, IOutgoing
	{
		public ulong ID { get; set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.MakedShot;

		public float Distance { get; private set; }

		public MakedShot(ulong iD, float distance)
		{
			ID = iD;
			Distance = distance;
		}
	}
}
