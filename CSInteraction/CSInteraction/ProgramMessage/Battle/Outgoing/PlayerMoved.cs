using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class PlayerMoved : IMessage, IOutgoing
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.PlayerMoved;
		public ulong ID{ get; private set; }
		public PointF NewLocation { get; private set; }

		public PlayerMoved(ulong iD, PointF newLocation)
		{
			ID = iD;
			NewLocation = newLocation;
		}
	}
}
