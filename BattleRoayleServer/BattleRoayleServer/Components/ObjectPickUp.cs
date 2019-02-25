using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class ObjectPickUp : IComponentMsg
	{

		public TypesComponentMsg Type { get; private set; }

		public GameObject NewParent { get; private set; }

	}
}