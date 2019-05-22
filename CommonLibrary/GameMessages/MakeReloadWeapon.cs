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
	public class MakeReloadWeapon : Message
	{
		public override ulong ID { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.MakeReloadWeapon;

		public MakeReloadWeapon(ulong iD)
		{
			ID = iD;
		}
	}
}
