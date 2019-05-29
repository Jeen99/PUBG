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
	public class GoTo : Message
	{

		public override Direction Direction { get; }

		public override ulong ID { get; set; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.GoTo;

		public GoTo(Direction selectedDirection)
		{
			Direction = selectedDirection;
		}

		public GoTo(ulong iD, Direction selectedDirection)
		{
			ID = iD;
			Direction = selectedDirection;
		}
	}
}
