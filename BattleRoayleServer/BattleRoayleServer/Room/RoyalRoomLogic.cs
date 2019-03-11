using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
    public class RoyalRoomLogic : IRoomLogic
    {
        private IGameModel roomContext;
        private Timer timerNewIteration;
		private QuantTimer quantTimer;

        public RoyalRoomLogic(int GamersInRoom)
        {
			roomContext = new RoyalGameModel(GamersInRoom);
			timerNewIteration = new Timer(50)
			{
				SynchronizingObject = null,
				AutoReset = true
			};
			timerNewIteration.Elapsed += TickQuantTimer;
			quantTimer = new QuantTimer();
        }

		/// <summary>
		/// Создает список состяний игровых объектов
		/// </summary>
		public IMessage RoomState
		{
			get
			{
				List<IMessage> states = new List<IMessage>();
				foreach (var gameObject in roomContext.GameObjects)
				{
					IMessage msg = gameObject.Value.State;
					if (msg!=null)states.Add(msg);
				}
				return new RoomState(states);
			}
		}

		/// <summary>
		/// Посредник между коллецией игроков игровой модели и внешней средой
		/// </summary>
		public IList<IPlayer> Players {
			get
			{
				return roomContext.Players;
			}
		}
		/// <summary>
		/// Посредник между коллецией событий игровой модели и внешней средой
		/// </summary>
		public ObservableQueue<IMessage> HappenedEvents {
			get {
				return roomContext.HappenedEvents;
			}
		}

		public void AddPlayer()
        {
            throw new NotImplementedException();
        }

        //вызывается при срабатывании таймера
        private void TickQuantTimer(object sender, ElapsedEventArgs e)
        {
			quantTimer.Tick();
			TimeQuantPassed msg = new TimeQuantPassed(quantTimer.QuantValue);
			foreach (var gameObject in roomContext.GameObjects)
			{
				if (!gameObject.Value.Destroyed)
				{
					if(gameObject.Value.TypesBehave == TypesBehaveObjects.Active)
					gameObject.Value.SendMessage(msg);
				}
				else roomContext.RemoveGameObject(gameObject.Value);
			}
        }

        public void EndGame()
        {
			timerNewIteration.Stop();
		}

        public void RemovePlayer()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
			timerNewIteration.Start();
        }

        public void Dispose()
        {
			timerNewIteration.Close();
			//осовобождение ресурсво модели
		}

	}
}
