using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer
{
	class ConsoleLogger : ILogger
	{
		private object sinchWrite = new object();

		public void AddInLog(string header = "", string description = "")
		{
			lock (sinchWrite)
			{
				Console.WriteLine("Произошла ошибка: ");
				Console.WriteLine(header);
				Console.WriteLine("Описание: ");
				Console.WriteLine(description);
				Console.WriteLine();
			}
		}
	}
}
