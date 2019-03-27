using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class GameObjectDestroy : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.GameObjectDestroy;

		public ulong ID { get; private set; }

		public GameObjectDestroy(ulong iD)
		{
			ID = iD;
		}
	}
}
