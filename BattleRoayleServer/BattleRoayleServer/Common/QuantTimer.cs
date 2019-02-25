using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BattleRoayleServer
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

		public double QuantValue { get; private set; }

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
			QuantValue = counter.ElapsedMilliseconds;
			counter.Restart();
		}
	}
}