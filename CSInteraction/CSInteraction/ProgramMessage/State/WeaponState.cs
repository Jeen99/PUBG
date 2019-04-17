using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class WeaponState : GameObjectState
	{
		public WeaponState(ulong id, TypesGameObject type, TypesWeapon typeWeapon, IList<IMessage> statesComponents) : base(id, type, statesComponents)
		{
			TypeWeapon = typeWeapon;
		}

		public TypesWeapon TypeWeapon { get; private set; }
		public override TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.WeaponState;
	}
}
