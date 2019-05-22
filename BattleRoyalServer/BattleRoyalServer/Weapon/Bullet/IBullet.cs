using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer
{
	public interface IBullet
	{
		float Distance { get; }
		float Damage { get; }
	}
}
