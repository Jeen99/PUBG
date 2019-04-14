using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class PlayerTurned : IMessage, IOutgoing
	{
		public ulong ID { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.PlayerTurned;

		public float Angle { get; private set; }

		public PlayerTurned(ulong iD, float angle)
		{
			ID = iD;
			Angle = angle;
		}
	}
}
