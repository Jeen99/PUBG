using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer
{
	class AssaultRifleBullet : IBullet
	{
		public float Distance { get; } = 80;

		public float Damage { get; } = 10;
	}
}
