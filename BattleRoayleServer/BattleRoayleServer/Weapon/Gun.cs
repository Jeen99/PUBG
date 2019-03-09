using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class Gun : Weapon
	{
		public Gun() : base()
		{

		}

		public override TypesGameObject Type => throw new NotImplementedException();

        public override TypesBehaveObjects TypesBehave => throw new NotImplementedException();
    }
}