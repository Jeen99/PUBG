using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	interface ILogger
	{
		void AddInLog(string header = "", string description = "");
	}
}
