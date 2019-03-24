using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class RoomState : IMessage
	{
		public List<IMessage> GameObjectsStates { get; private set; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.RoomState;

		public RoomState(List<IMessage> gameObjectsStates)
		{
			GameObjectsStates = gameObjectsStates;
		}
	}
}
