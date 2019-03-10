using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class PlayerMoved : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.PlayerMoved;
		public ulong PlayerID{ get; private set; }
		public PointF NewLocation { get; private set; }

		public PlayerMoved(ulong playerID, PointF newLocation)
		{
			PlayerID = playerID;
			NewLocation = newLocation;
		}
	}
}
