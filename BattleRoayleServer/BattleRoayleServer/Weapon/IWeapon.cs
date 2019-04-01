using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public interface IWeapon : IGameObject
	{	
		 IGameObject Holder { get; set; }

		 TypesWeapon TypeWeapon { get; }
	}
}
