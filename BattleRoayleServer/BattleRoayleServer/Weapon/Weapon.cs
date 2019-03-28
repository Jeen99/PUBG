using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public abstract class Weapon : IGameObject
	{
		public Weapon(IGameModel model) : base(model)
		{

		}
		public IGameObject Holder { get; set; }

		public virtual TypesWeapon TypeWeapon { get; }
	}
}
