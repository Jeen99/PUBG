using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class GameObjectState : IMessage, IOutgoing
	{
		public TypesGameObject Type { get; private set; }
		public IList<IMessage> StatesComponents { get; private set; }
		public ulong ID { get; private set; }
		public virtual TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.GameObjectState;

		public GameObjectState(ulong id,TypesGameObject type, IList<IMessage> statesComponents)
		{
			ID = id;
			Type = type;
			StatesComponents = statesComponents;
		}
	}
}
