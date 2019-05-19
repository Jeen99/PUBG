using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BattleRoyalServer
{
	//отслеживает время прошедшее с предыдущего такта работы игры
	public class QuantTimer
	{
		private Stopwatch counter;

		public QuantTimer()
		{
			counter = new Stopwatch();
			QuantValue = 0;
		}

		public int QuantValue { get; private set; }

		public void Start()
		{
			counter.Start();
		}

		public void Stop()
		{
			counter.Stop();
		}

		public void Tick()
		{
			counter.Stop();
			QuantValue = (int)counter.ElapsedMilliseconds;
			counter.Restart();
		}
	}
}