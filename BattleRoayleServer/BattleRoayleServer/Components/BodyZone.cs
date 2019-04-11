using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;
using System.Drawing;

namespace BattleRoayleServer
{
	public class BodyZone : Component, IBodyZone
	{
		PointF location;
		public PointF Location
		{
			get
			{
				return location;
			}
		}

		public float Radius { get; private set; }

		private TimeSpan timeTillReducton;
		private float sizeMap;
		private const int timeRound = 30;

		public BodyZone(IGameObject parent, float sizeMap) : base(parent)
		{
			this.sizeMap = sizeMap;
			Radius = sizeMap / 2;
			location = new PointF(Radius, Radius);
			//время до сужения зоны постоянно и равно 30 секундам
			timeTillReducton = new TimeSpan(0, 0, timeRound); 
		}

		public override void Setup()
		{

		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TimeQuantPassed:
					Handler_TimeQuantPassed((TimeQuantPassed)msg);
					break;
			}
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
		
			//сохраняем количество секунд
			int leftSecond = timeTillReducton.Seconds;
			//отнимаем прошедшее время

			timeTillReducton = timeTillReducton.Add(new TimeSpan(0, 0, 0, 0, - msg.QuantTime));
			if (timeTillReducton.Seconds != leftSecond)
			{
				Parent.Model?.AddEvent(new ChangedTimeTillReduction(Parent.ID, timeTillReducton));
			}

			if(timeTillReducton.Milliseconds < 0) CheckReduction();
		}

		private void CheckReduction()
		{	
			//пределяем новую позицию центра зоны
			Random rand = new Random();
			location.X = rand.Next(Convert.ToInt32(location.X - Radius), Convert.ToInt32(location.X + Radius));
			location.Y = rand.Next(Convert.ToInt32(location.Y - Radius), Convert.ToInt32(location.Y + Radius));

			//уменьшаем радиус на 40% процентов
			Radius = Radius * 0.6f;

			//устанавливаем новое время до сужения зоны
			timeTillReducton = new TimeSpan(0, 0, timeRound);

			//отпрвляем сообщение об изменение зоны
			Parent.Model?.AddEvent(Parent.State);
		}

		public override IMessage State
		{
			get
			{
				return new BodyZoneState(location, Radius);
			}
		}

	}
}
