using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer
{
	public class GunBullet : IBullet
	{
		public float Distance { get; } = 50;

		public float Damage { get; } = 8;
	
	}
}
