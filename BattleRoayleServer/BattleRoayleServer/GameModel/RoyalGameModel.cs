using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public class RoyalGameModel : IGameModel
    {
        //только на чтение
        public IList<IPlayer> Players { get; private set; }
        public IList<GameObject> GameObjects { get; private set; }
        public IField Field { get; private set; }

		/// <summary>
		/// Содержит алгоритм наполнения карты игровыми объектами
		/// </summary>
		/// <remarks>
		/// По умолчанию создает список объектов.
		/// Должен создавать объекты, добавлять их в список и на карту.
		/// </remarks>
		private void CreateStaticGameObject()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Создает игроков и добавляет их на карту в список игроков и список игрвовых объектов
		/// </summary>
		private void CreatePlayers()
		{
			throw new System.NotImplementedException();
		}

		public RoyalGameModel()
        {
            //инициализируем поля
            //создание и добавление в GameObjects и Field статических объектов карты

        }
    }
}