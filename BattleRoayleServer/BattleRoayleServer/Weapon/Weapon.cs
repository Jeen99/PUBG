using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public abstract class Weapon : GameObject, IWeapon
	{
		public Weapon(IModelForComponents model) : base(model)
		{

		}

		public IGameObject Holder { get; set; }

		public TypesWeapon TypeWeapon { get; protected set; }
		
	}
}
