using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace BattleRoayleServer
{
    public class RoomNetwork:INetwork
    {
        /// <summary>
        /// Ссылка на игровую логику
        /// </summary>
        private IRoomLogic roomLogic;
		/// <summary>
		/// Список игроков подлюченных к данной комнате
		/// </summary>
		public IList<INetworkClient> Clients { get; private set; }
		/// <summary>
		/// Вызывается для отправки окружения клиентов в данный момент времени
		/// </summary>
		private Timer timerTotalSinch;

        public RoomNetwork(IList<QueueGamer> gamers, IRoomLogic roomLogic)
        {
			//создаем список игроков на основе списка gamers и rooomLogic(ID), создаем таймер 
			Clients = new List<INetworkClient>();
			this.roomLogic = roomLogic;
			roomLogic.HappenedEvents.CollectionChanged += HandlerGameEvent;
			timerTotalSinch = new Timer(3 * 1000);
			timerTotalSinch.SynchronizingObject = null;
			timerTotalSinch.AutoReset = true;
			timerTotalSinch.Elapsed += HandlerTotalSinch;
			CreateClients(gamers);
		}
		/// <summary>
		/// Вызывается на каждое срабатывание таймера
		/// </summary>
		private void HandlerTotalSinch(object sender, ElapsedEventArgs e)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Определяет тип сообщения и запускает соответствующий обработчик
		/// </summary>
		private void HandlerGameEvent(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

        public void Start()
        {
            //запускаем таймер
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Создает список клиентов
		/// </summary>
		private void CreateClients(IList<QueueGamer> gamers)
		{
			for(int i = 0; i < roomLogic.Players.Count; i++)
			{
				Clients.Add(new NetworkClient(roomLogic.Players[i], gamers[i].Client, 
					gamers[i].NickName, gamers[i].Password));
			}
		}
	}
}