using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class WeaponState : GameObjectState
	{
		public WeaponState(ulong id, TypesGameObject type, TypesWeapon typeWeapon, List<IMessage> statesComponents) : base(id, type, statesComponents)
		{
			TypeWeapon = typeWeapon;
		}

		public override TypesWeapon TypeWeapon { get; }
		public override TypesMessage TypeMessage { get; } = TypesMessage.WeaponState;
	}
}
