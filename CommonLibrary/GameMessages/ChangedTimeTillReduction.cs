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
	public class ChangedTimeTillReduction : Message
	{
		public ChangedTimeTillReduction(ulong iD, TimeSpan time)
		{
			ID = iD;
			Time = time;
		}

		public override TimeSpan Time { get; }

		public override ulong ID { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.ChangedTimeTillReduction;
	}
}
