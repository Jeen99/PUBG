using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public delegate void ForHappenedGameEvent (IGameObjectEvent msg);
    public interface IRoomLogic
    {
        /// <summary>
        /// Уведомляет подписавшихся о том, что произолшло игровое событие
        /// </summary>
        /// <remarks>Передает необходимые данные в виде интерфейса IGameEventData</remarks>
        event ForHappenedGameEvent HappenedGameEvent;

        /// <summary>
        /// Возвращает состояние объектов в данный момент времени
        /// </summary>
        IList<GameObjectState> RoomState { get; }
		/// <summary>
		/// Содержит список живых игроков
		/// </summary>
		IList<IPlayer> Players { get; }

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
        /// Получаем описание событий, произошедшиз в комента
        /// </summary>
        /// <remarks>Например добавление игрока, убийство одно игрока другим и т.д.</remarks>
        void GetRoomEvents();
        /// <summary>
        /// Особождает ресурсы игровой комнаты
        /// </summary>
        void Dispose();
    }

}