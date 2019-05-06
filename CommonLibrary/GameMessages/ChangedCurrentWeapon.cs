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
	public class ChangedCurrentWeapon : Message
	{
		public ChangedCurrentWeapon(ulong iD, TypesWeapon newCurrentWeapon)
		{
			ID = iD;
			TypeWeapon = newCurrentWeapon;
		}

		public override ulong ID { get; }

		public override TypesWeapon TypeWeapon { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.ChangedCurrentWeapon;
	}
}
