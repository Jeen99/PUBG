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
	public class AddInBattle : Message
	{
		public override ulong ID { get; set; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.AddInBattle;

		public AddInBattle() {}

		public AddInBattle(ulong iD)
		{
			ID = iD;
		}		
	}
}
