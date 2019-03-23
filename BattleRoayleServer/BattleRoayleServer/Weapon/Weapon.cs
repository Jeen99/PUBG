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
		public Weapon(IGameModel model) : base(model)
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
