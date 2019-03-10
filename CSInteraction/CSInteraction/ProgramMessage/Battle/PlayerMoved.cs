using System;
using System.Collections.Generic;
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
		public Tuple<double, double> NewLocation { get; private set; }

		public PlayerMoved(ulong playerID, Tuple<double, double> newLocation)
		{
			PlayerID = playerID;
			NewLocation = newLocation;
		}
	}
}
