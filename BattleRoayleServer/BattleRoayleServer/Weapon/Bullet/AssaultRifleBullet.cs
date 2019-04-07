using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	class AssaultRifleBullet : IBullet
	{
		public float Distance { get; } = 60;

		public float Damage { get; } = 10;
	}
}
