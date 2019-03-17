using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	interface IGameObject
	{
		TypesGameObject Type { get; }
		PointF Location { get; set; }
	}
}
