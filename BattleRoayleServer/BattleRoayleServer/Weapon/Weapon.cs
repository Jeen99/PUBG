using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoayleServer
{
	public abstract class Weapon : GameObject
	{
		protected Weapon() : base()
		{

		}
	}

	enum TypesWeapon
    {
        Gun,
        ShotGun,
        AssaultRifle,
        Grenade
    }

}
