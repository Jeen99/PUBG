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
        /// Вызывается для отправки окружения клиентов в данный момент времени
        /// </summary>
        private Timer timerTotalSinch;

        public RoomNetwork(IList<QueueGamer> gamers, IRoomLogic roomLogic)
        {
            //создаем список игроков на основе списка gamers и rooomLogic(ID), создаем таймер 
        }

        public IList<INetworkClient> Clients { get; private set; }

        /// <summary>
        /// Определяет тип сообщения и запускает соответствующий обработчик
        /// </summary>
        private void HandlerGameEvent()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Вызывается на каждое срабатывание таймера
        /// </summary>
        private void HandlerTotalSinch()
        {
            throw new System.NotImplementedException();
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
		private void CreateClients(IList<gamers> gamers)
		{
			throw new System.NotImplementedException();
		}
	}
}