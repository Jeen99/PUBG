using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class WeaponState : IMessage
	{
		public TypesWeapon TypeWeapon { get; private set; }
		public IList<IMessage> StatesComponents { get; private set; }
		public ulong ID { get; private set; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.WeaponState;

		public WeaponState(ulong id, TypesWeapon typeWeapon, IList<IMessage> statesComponents)
		{
			ID = id;
			StatesComponents = statesComponents;
			TypeWeapon = typeWeapon;
		}
	}
}
