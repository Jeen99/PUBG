using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
     abstract class Bullet
    {
        protected Directions Direction { get; set; }
        protected int Damage { get; set; }
        protected int Range { get; set; }

        public abstract TypesBullet GetTypesBullet();
    }


    enum TypesBullet
    {
        Grenade,
        BulletOfGan,
        BulletOfShotGun,
        BulletOfAssaultRifle
    }

    enum Directions
    {
        right,
        left,
        bottom,
        top,
        right_bottom,
        right_top,
        left_bottom,
        left_top
    }
}
