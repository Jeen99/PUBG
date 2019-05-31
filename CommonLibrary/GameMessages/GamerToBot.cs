using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class GamerToBot : Message
	{
		public override ulong ID { get; set; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.GamerToBot;

		public GamerToBot(ulong ID)
		{
			this.ID = ID;
		}

	}
}
