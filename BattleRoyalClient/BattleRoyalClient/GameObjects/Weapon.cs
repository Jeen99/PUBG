using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Weapon : GameObject
	{
		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Weapon;
		public virtual TypesWeapon TypesWeapon { get; protected set; }

		public override string TextureName => TypesWeapon.ToString();

		public Weapon(TypesWeapon typesWeapon)
		{
			TypesWeapon = typesWeapon;
		}

		public Weapon()
		{
			TypesWeapon = TypesWeapon.Gun;
		}
	}
}
