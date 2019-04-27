using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommonLibrary;
using ObservalableExtended;

namespace BattleRoayleServer
{
    public interface IRoomLogic
    {
		event RoomLogicEndWork EventRoomLogicEndWork;
		IGameModel RoomModel { get; }
		/// <summary>
		/// Запускает игровую комнату
		/// </summary>
		void Start();

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