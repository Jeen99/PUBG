using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

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
			timerNewIteration = new Timer(50);
			timerNewIteration.SynchronizingObject = null;
			timerNewIteration.AutoReset = true;
			timerNewIteration.Elapsed += TickQuantTimer;
			quantTimer = new QuantTimer();
        }


		//необходимо реализовать
		/// <summary>
		/// Создает список состяний игровых объектов
		/// </summary>
		public IList<GameObjectState> RoomState { get; }

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
		public ObservableCollection<IComponentEvent> HappenedEvents {
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
				TimeQuantPassed msg = new TimeQuantPassed();
				foreach (GameObject gameObject in roomContext.GameObjects)
				{
					if (!gameObject.Destroyed)
						gameObject.SendMessage(new TimeQuantPassed(quantTimer.QuantValue));
					else roomContext.RemoveGameObject(gameObject);
				}
        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }

        public IList<GameObjectState> GetAllObjectStates()
        {
            throw new NotImplementedException();
        }


        public void RemovePlayer()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

	}
}
