using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class GameObjectDelete : IMessage
	{
		public ulong ID { get; private set; }
		public TypesProgramMessage TypeMessage { get; }  = TypesProgramMessage.GameObjectDelete;

		public GameObjectDelete(ulong iD)
		{
			ID = iD;
		}
	}
}
