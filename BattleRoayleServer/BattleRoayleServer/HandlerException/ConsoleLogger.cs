using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	class ConsoleLogger : ILogger
	{
		public void AddInLog(string header = "", string description = "")
		{
			Console.WriteLine("Произошла ошибка: ");
			Console.WriteLine(header);
			Console.WriteLine("Описание: ");
			Console.WriteLine(description);
			Console.WriteLine();
		}
	}
}
