using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using System.Collections.Specialized;
using System.Drawing;

namespace BattleRoayleServer
{
    public class RoyalGameModel : IGameModel
    {
        //только на чтение
        public IList<IPlayer> Players { get; private set; }
        public ConcurrentDictionary<ulong,GameObject> GameObjects { get; private set; }
        public IField Field { get; private set; }
		/// <summary>
		/// Колекция событий произошедших в игре
		/// </summary>
		public ObservableQueue<IMessage> HappenedEvents { get; private set; }
		
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
			Stone stone = new Stone(this, new PointF(10, 10), new Size(14,14));
			GameObjects.AddOrUpdate(stone.ID, stone,(k,v)=> { return v; });
			stone = new Stone(this, new PointF(30, 28), new Size(12, 12));
			GameObjects.AddOrUpdate(stone.ID, stone, (k, v) => { return v; });
			stone = new Stone(this, new PointF(78, 30), new Size(15, 15));
			GameObjects.AddOrUpdate(stone.ID, stone, (k, v) => { return v; });
			Box box = new Box(this, new PointF(40, 100), new Size(12, 12));
			GameObjects.AddOrUpdate(box.ID, box, (k, v) => { return v; });
			box = new Box(this, new PointF(150, 10), new Size(12, 12));
			GameObjects.AddOrUpdate(box.ID, box, (k, v) => { return v; });
		}

		/// <summary>
		/// Создает игроков и добавляет их на карту в список игроков и список игрвовых объектов
		/// </summary>
		private void CreatePlayers(int gamersInRoom)
		{
			for (int i = 0; i < gamersInRoom; i++)
			{
				//создаем игрока
				Gamer gamer = new Gamer(CreatePlayerLocation(i), this);
				Players.Add(gamer);
				GameObjects.AddOrUpdate(gamer.ID,gamer, (k, v) => { return v; });
			}
		}
		private PointF CreatePlayerLocation(int index)
		{
			switch (index)
			{
				case 0:
					return new PointF(50, 70);
				case 1:
					return new PointF(100, 170);
				default:
					return new PointF(0, 0);
			}
		}
		public RoyalGameModel(int gamersInRoom)
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			GameObjects = new ConcurrentDictionary<ulong, GameObject>();
			Field = new RoyalField();
			HappenedEvents = new ObservableQueue<IMessage>();

			//создание и добавление в GameObjects и Field статических объектов карты
			CreateStaticGameObject();
			CreatePlayers(gamersInRoom);

		}

		public void RemoveGameObject(GameObject gameObject)
		{

		}

    }
}