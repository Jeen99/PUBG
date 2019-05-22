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
	public class CurrentWeaponState : Message
	{		
		public CurrentWeaponState(TypesWeapon typeWeapon)
		{
			TypeWeapon = typeWeapon;
		}

		public override TypesWeapon TypeWeapon { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.CurrentWeaponState;
	}
}
