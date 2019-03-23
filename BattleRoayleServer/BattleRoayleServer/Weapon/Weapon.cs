using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public abstract class Weapon : GameObject
	{
		public Weapon(IGameModel model) : base(model)
		{

		}

		public virtual TypesWeapon TypeWeapon { get; }
	}
}
