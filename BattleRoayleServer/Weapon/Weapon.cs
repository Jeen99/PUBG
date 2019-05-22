using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoayleServer
{
    abstract class Weapon: IPlaced
    {
        protected List<Bullet> magazine;
        public Point Location { get; set; }
        protected bool IsShooting;
        protected bool IsReloading;

        public TypesObject GetTypesObject()
        {
            return TypesObject.Weapon;
        }

        public abstract void MakeShot(Directions direction);
        public abstract void ReloadMagazin();

        public abstract TypesWeapon GetTypeWeapon();
        
    }

    enum TypesWeapon
    {
        Gun,
        ShotGun,
        AssaultRifle,
        Grenade
    }

}
