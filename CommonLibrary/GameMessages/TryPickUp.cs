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
	public class TryPickUp : Message
	{
		public override ulong ID { get;  set;}

		public override TypesMessage TypeMessage { get; } = TypesMessage.TryPickUp;

		public TryPickUp()
		{
		}

		public TryPickUp(ulong iD)
		{
			ID = iD;
		}
	}
}
