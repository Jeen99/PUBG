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
	public class DeletedInMap : Message
	{
		public override ulong ID { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.DeletedInMap;

		public DeletedInMap(ulong iD)
		{
			ID = iD;
		}

	}
}
