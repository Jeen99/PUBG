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
	public class EndGame : Message
	{
		public override ulong ID { get;  set;}

		public override long Kills { get; }

		public override TimeSpan Time { get; }

		public override bool Result { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.EndGame;

		public EndGame(ulong iD, bool youDied, int kills, TimeSpan timeLife)
		{
			ID = iD;
			Result = youDied;
			Kills = kills;
			Time = timeLife;
		}
	}
}
