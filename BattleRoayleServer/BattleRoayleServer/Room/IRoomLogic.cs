using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
    public interface IRoomLogic
    {
        /// <summary>
        /// Возвращает состояние объектов в данный момент времени
        /// </summary>
       IMessage RoomState { get; }
		/// <summary>
		/// Содержит список живых игроков
		/// </summary>
		IList<IPlayer> Players { get; }

		event RoomLogicEndWork EventRoomLogicEndWork;

		ObservableQueue<IMessage> HappenedEvents { get; }

		/// <summary>
		/// Запускает игровую комнату
		/// </summary>
		void Start();
        /// <summary>
        /// Закрывает игровую комнату, освобождает все ресурсы
        /// </summary>
        void EndGame();
        /// <summary>
        /// Добавляет игрока в игровую комнату
        /// </summary>
        void AddPlayer();
        /// <summary>
        /// Удаляет игрока из игровой комнаты
        /// </summary>
        void RemovePlayer();
        /// <summary>
        /// Особождает ресурсы игровой комнаты
        /// </summary>
        void Dispose();
		/// <summary>
		/// Возвращает информацию обо всех объектов в игровой комнате
		/// </summary>
		IMessage GetInitializeData();

	}

}