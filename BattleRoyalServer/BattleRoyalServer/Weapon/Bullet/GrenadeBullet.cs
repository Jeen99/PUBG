using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer
{
	public class GrenadeBullet : IBullet
	{
		//здесь - радиус поражения
		public float Distance { get; } = 10;

		public float Damage { get; } = 50;
	}
}
