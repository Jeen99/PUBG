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

		private DateTime timeTillReducton;
		private float sizeMap;
		private const int timeRound = 30;

		public BodyZone(IGameObject parent, float sizeMap) : base(parent)
		{
			this.sizeMap = sizeMap;
			Radius = sizeMap / 2;
			location = new PointF(Radius, Radius);		
			//время до сужения зоны постоянно и равно 30 секундам
			timeTillReducton = new DateTime(1, 1, 1, 1, 0, timeRound, 0);
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
			int leftSecond = timeTillReducton.Second;
			//отнимаем прошедшее время
			timeTillReducton = timeTillReducton.AddMilliseconds(-msg.QuantTime);
			if (timeTillReducton.Second != leftSecond)
			{
				Parent.Model?.AddEvent(new ChangedTimeTillReduction(Parent.ID, timeTillReducton));
			}

			CheckReduction();
		}

		private void CheckReduction()
		{
			if (timeTillReducton.Second <= 0 && timeTillReducton.Millisecond <= 0)
			{
				//определяем новую позицию центра зоны
				Random rand = new Random();
				location.X = rand.Next(Convert.ToInt32(location.X - Radius), Convert.ToInt32(location.X + Radius));
				location.Y = rand.Next(Convert.ToInt32(location.Y - Radius), Convert.ToInt32(location.Y + Radius));

				//уменьшаем радиус на 40% процентов
				Radius = Radius * 0.6f;

				//устанавливаем новое время до сужения зоны
				timeTillReducton = timeTillReducton.AddSeconds(timeRound);

				//отпрвляем сообщение об изменение зоны
				Parent.Model?.AddEvent(Parent.State);
			}
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
