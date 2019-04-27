using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using System.Drawing;

namespace BattleRoayleServer
{
	public class BodyZone : Component
	{
		PointF location;
		public PointF Location
		{
			get
			{
				return location;
			}
		}
		public bool Create { get; private set; } = false;
		public float Radius { get; private set; }

		private TimeSpan timeTillReducton;
		private float sizeMap;
		private const int timeRound = 30;

		public BodyZone(IGameObject parent, float sizeMap) : base(parent)
		{
			this.sizeMap = sizeMap;
			Radius = 0;
			location = new PointF();
			//время до сужения зоны постоянно и равно 30 секундам
			timeTillReducton = new TimeSpan(0, 0, timeRound); 
		}

		public override void Setup()
		{

		}

		public override void UpdateComponent(IMessage msg)
		{
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте Collector");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesMessage.TimeQuantPassed:
					Handler_TimeQuantPassed(msg);
					break;
			}
		}

		private void Handler_TimeQuantPassed(IMessage msg)
		{
		
			//сохраняем количество секунд
			int leftSecond = timeTillReducton.Seconds;
			//отнимаем прошедшее время

			timeTillReducton = timeTillReducton.Add(new TimeSpan(0, 0, 0, 0, - msg.TimePassed));
			if (timeTillReducton.Seconds != leftSecond)
			{
				Parent.Model?.AddOutgoingMessage(new ChangedTimeTillReduction(Parent.ID, timeTillReducton));
			}

			if(timeTillReducton.Milliseconds < 0) CheckReduction();
		}

		private void CheckReduction()
		{
			if (Create)
			{
				//пределяем новую позицию центра зоны
				Random rand = new Random();
				float HalfRadius = Radius / 2f;
				location.X = rand.Next(Convert.ToInt32(location.X - HalfRadius), Convert.ToInt32(location.X + HalfRadius));
				location.Y = rand.Next(Convert.ToInt32(location.Y - HalfRadius), Convert.ToInt32(location.Y + HalfRadius));

				//уменьшаем радиус на 40% процентов
				Radius = Radius * 0.6f;

				
			}
			else
			{
				Radius = sizeMap / 2f;
				location = new PointF(Radius, Radius);
				Create = true;
			}
			//устанавливаем новое время до сужения зоны
			timeTillReducton = new TimeSpan(0, 0, timeRound);
			//отпрвляем сообщение об изменение зоны
			Parent.Model?.AddOutgoingMessage(Parent.State);
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
