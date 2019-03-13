using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	public class Diapason
	{
		public float Right { get; set; }
		public float Left { get; set;}

		public Diapason(float left = 0, float right = 0)
		{
			Left = left;
			Right = right;
			
		}
	}
}
