using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class GamerDied : Message
	{
		public override ulong ID { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.GamerDied;

		public GamerDied(ulong iD)
		{
			ID = iD;
		}
	}
}
