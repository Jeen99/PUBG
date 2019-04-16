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
		public ulong ID { get; private set; }

		public float Angle { get; private set;
		}
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.MakedShot;

		public float Distance { get; private set; }

		public MakedShot(ulong iD, float angle, float distance)
		{
			ID = iD;
			Angle = angle;
			Distance = distance;
		}
	}
}
