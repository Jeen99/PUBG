using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	public class ShotGunBullet : IBullet
	{
		public float Distance { get; } = 40;

		public float Damage { get; } = 20;
	}
}
