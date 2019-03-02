using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace BattleRoayleServer
{
    public class RoyalGameModel : IGameModel
    {
        //только на чтение
        public IList<IPlayer> Players { get; private set; }
        public IList<GameObject> GameObjects { get; private set; }
        public IField Field { get; private set; }
		/// <summary>
		/// Колекция событий произошедших в игре
		/// </summary>
		public ObservableCollection<IComponentEvent> HappenedEvents { get; private set; }

		
		/// <summary>
		/// Содержит алгоритм наполнения карты игровыми объектами
		/// </summary>
		/// <remarks>
		/// По умолчанию создает список объектов.
		/// Должен создавать объекты, добавлять их в список и на карту.
		/// </remarks>
		private void CreateStaticGameObject()
		{
			//скрипт создания игровых объектов
		}

		/// <summary>
		/// Создает игроков и добавляет их на карту в список игроков и список игрвовых объектов
		/// </summary>
		private void CreatePlayers(int gamersInRoom)
		{
			for (int i = 0; i < gamersInRoom; i++)
			{
				//создаем игрока
				Gamer gamer = new Gamer();
				Players.Add(gamer);
				GameObjects.Add(gamer);
				Field.Put(gamer);
			}
		}

		public RoyalGameModel(int gamersInRoom)
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			GameObjects = new List<GameObject>();
			Field = new RoyalField();
			HappenedEvents = new ObservableCollection<IComponentEvent>();

			//создание и добавление в GameObjects и Field статических объектов карты
			CreateStaticGameObject();
			CreatePlayers(gamersInRoom);

		}

		public void RemoveGameObject(GameObject gameObject)
		{

		}

    }
}