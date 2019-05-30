using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer.Common
{
	public static class GetRandom
	{
		private static readonly Random _random = new Random();

		public static Random Random { get { return _random; } }
	}
}
