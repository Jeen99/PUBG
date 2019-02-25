using System;
using System.Collections.Generic;
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

		public event ForHappenedGameEvent HappenedGameEvent;

        public RoyalRoomLogic()
        {
            //инициализация компонентов, создание таймера
            //список игроков нужен нам для создания ника
            //создаем карту 
        }
        //необходимо реализовать
        /// <summary>
        /// Создает список состяний игровых объектов
        /// </summary>
        public IList<GameObjectState> RoomState { get; }

		public IList<IPlayer> Players {
			get
			{
				return roomContext.Players;
			}
		}

		public void AddPlayer()
        {
            throw new NotImplementedException();
        }

        //вызывается при срабатывании таймера
        private void TickQuantTimer()
        {

        }

        public void EndGame()
        {
            throw new NotImplementedException();
        }

        public IList<GameObjectState> GetAllObjectStates()
        {
            throw new NotImplementedException();
        }

        public void GetRoomEvents()
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

		private void HandlerObjectEvent(IGameObjectEvent msg)
		{
			throw new System.NotImplementedException();
		}
	}
}
