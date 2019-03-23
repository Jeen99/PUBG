using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	public interface IBullet
	{
		float Distance { get; }
		float Damage { get; }
	}
}
