using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	interface IBattleView
	{
		bool Transition { get; set; }
		//void UpdateVisual(IModelObject model, ulong ID);
	}
}
