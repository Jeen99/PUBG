using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Field : GameObject
	{
		public Field(ulong ID) : base(ID)
		{

		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Field;

	}
}
