using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class Modifier : GameObject
	{
		public Modifier():base()
		{

		}

		public override TypesGameObject Type => throw new NotImplementedException();

        public override TypesBehaveObjects TypesBehave => throw new NotImplementedException();
    }
}