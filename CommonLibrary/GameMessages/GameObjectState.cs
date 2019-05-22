using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class GameObjectState : Message
	{
		public override TypesGameObject TypeGameObject { get; }

		public override ulong ID { get; }

		public override List<IMessage> InsertCollections { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.GameObjectState;

		public GameObjectState(ulong id,TypesGameObject type, List<IMessage> statesComponents)
		{
			ID = id;
			TypeGameObject = type;
			InsertCollections = statesComponents;
		}
	}
}
