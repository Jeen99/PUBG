using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	class Diapason
	{
		public float Right { get; set; }
		public float Left { get; set;}

		public Diapason(float left, float right)
		{
			Left = left;
			Right = right;
			
		}
	}
}
