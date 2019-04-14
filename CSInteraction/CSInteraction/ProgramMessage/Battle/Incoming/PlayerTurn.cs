using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class PlayerTurn : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.PlayerTurn;

		public float Angle { get; private set; }

		public PlayerTurn(float angle)
		{
			Angle = angle;
		}
	}
}
