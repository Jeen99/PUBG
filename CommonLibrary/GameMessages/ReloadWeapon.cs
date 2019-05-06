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
	public class ReloadWeapon : Message
	{
		public override ulong ID { get; }

		public override bool StartOrEnd { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.ReloadWeapon;

		public ReloadWeapon(ulong iD, bool startOrEnd)
		{
			ID = iD;
			StartOrEnd = startOrEnd;
		}

		
	}
}
