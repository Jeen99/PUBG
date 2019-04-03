using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class GoTo : IMessage
	{
		public Direction DirectionMove { get; private set; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.GoTo;

		public GoTo(Direction direction)
		{
			DirectionMove = direction;
		}
	}
}
