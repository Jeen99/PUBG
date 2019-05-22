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
	public class ChangeCountPlayersInGame : Message
	{
		public override int Count { get; }

		public override ulong ID { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.ChangeCountPayersInGame;

		public ChangeCountPlayersInGame(ulong iD, int count)
		{
			ID = iD;
			Count = count;
		}
	}
}
