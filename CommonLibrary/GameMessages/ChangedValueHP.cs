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
	public class ChangedValueHP: Message
	{
		public ChangedValueHP(ulong iD, float newValueHP)
		{
			ID = iD;
			HP = newValueHP;
		}

		public override ulong ID { get; set; }

		public override float HP { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.ChangedValueHP;
	}
}
