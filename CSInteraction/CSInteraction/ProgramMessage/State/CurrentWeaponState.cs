using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class CurrentWeaponState : IMessage
	{
		public TypesWeapon TypeCurrentWeapon { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.CurrentWeaponState;

		public CurrentWeaponState(TypesWeapon typeCurrentWeapon)
		{
			TypeCurrentWeapon = typeCurrentWeapon;
		}
	}
}
