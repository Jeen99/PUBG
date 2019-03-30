﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using System.Collections.Specialized;
using System.Drawing;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;

namespace BattleRoayleServer
{
    public class RoyalGameModel : IGameModel, IModelForComponents
	{
		/// <summary>
		///ширина одной стороны игровой карты 
		/// </summary>
		private const float lengthOfSide = 500;
        //только на чтение
        public IList<IPlayer> Players { get; private set; }
		/// <summary>
		/// Коллекция всех игровых объектов в игре
		/// </summary>
        public Dictionary<ulong,IGameObject> GameObjects { get; private set; }
		/// <summary>
		/// Коллекция для объектов, которые не участвуют в физическом взаимодействии
		/// </summary>
		public List<IGameObject> Loot { get; private set; }

        public World Field { get; private set; }
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
			GameObjects.Add(stone.ID, stone);
			stone = new Stone(this, new PointF(30, 28), new Size(12, 12));
			GameObjects.Add(stone.ID, stone);
			stone = new Stone(this, new PointF(78, 30), new Size(15, 15));
			GameObjects.Add(stone.ID, stone);
			Box box = new Box(this, new PointF(40, 100), new Size(12, 12));
			GameObjects.Add(box.ID, box);
			box = new Box(this, new PointF(150, 10), new Size(12, 12));
			GameObjects.Add(box.ID, box);
			Gun gun = new Gun(this, new PointF(50, 70));
			GameObjects.Add(gun.ID, gun);
		}

		/// <summary>
		/// Создает игроков и добавляет их на карту в список игроков и список игрвовых объектов
		/// </summary>
		private void CreatePlayers(int gamersInRoom)
		{
			for (int i = 0; i < gamersInRoom; i++)
			{
				//создаем игрока
				Gamer gamer = new Gamer(this, CreatePlayerLocation(i));
				Players.Add(gamer);
				GameObjects.Add(gamer.ID,gamer);
			}
		}

		private PointF CreatePlayerLocation(int index)
		{
			switch (index)
			{
				case 0:
					return new PointF(50, 70);
				case 1:
					return new PointF(50, 85);
				default:
					return new PointF(0, 0);
			}
		}

		public RoyalGameModel(int gamersInRoom)
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			GameObjects = new Dictionary<ulong, IGameObject>();
			HappenedEvents = new ObservableQueue<IMessage>();
			Loot = new List<IGameObject>();

			AABB frameField = new AABB();
			frameField.LowerBound.Set(0,0);
			frameField.UpperBound.Set(lengthOfSide, lengthOfSide);
			Field = new World(frameField, new Vec2(0, 0), false);
			var solver = new RoomContactListener();
			Field.SetContactListener(solver);
			CreateFrame();


			//создание и добавление в GameObjects и Field статических объектов карты
			CreateStaticGameObject();
			CreatePlayers(gamersInRoom);

		}

		//только для тестов
		public RoyalGameModel()
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			GameObjects = new Dictionary<ulong, IGameObject>();
			HappenedEvents = new ObservableQueue<IMessage>();
			Loot = new List<IGameObject>();

			AABB frameField = new AABB();
			frameField.LowerBound.Set(0, 0);
			frameField.UpperBound.Set(lengthOfSide, lengthOfSide);
			Field = new World(frameField, new Vec2(0, 0), false);
			var solver = new RoomContactListener();
			Field.SetContactListener(solver);
			CreateFrame();
		}

		private void CreateFrame()
		{
			//bottom
			BodyDef bDefBottom = new BodyDef();
			bDefBottom.Position.Set(0, 0);
			bDefBottom.Angle = 0;

			PolygonDef pDefBottom = new PolygonDef();
			pDefBottom.Restitution = 0.3f;
			pDefBottom.Friction = 0.2f;
			pDefBottom.Density = 0;
			pDefBottom.SetAsBox(lengthOfSide, 1);

			var frame  = Field.CreateBody(bDefBottom);
			frame.CreateShape(pDefBottom);

			//left
			BodyDef bDefLeft = new BodyDef();
			bDefLeft.Position.Set(0, 0);
			bDefLeft.Angle = 0;

			PolygonDef pDefLeft = new PolygonDef();
			pDefLeft.Restitution = 0.3f;
			pDefLeft.Friction = 0.2f;
			pDefLeft.Density = 0;
			pDefLeft.SetAsBox(1, lengthOfSide);

			frame = Field.CreateBody(bDefLeft);
			frame.CreateShape(pDefLeft);

			//top
			BodyDef bDefTop = new BodyDef();
			bDefTop.Position.Set(0, lengthOfSide - 1);
			bDefTop.Angle = 0;

			PolygonDef pDefTop = new PolygonDef();
			pDefTop.Restitution = 0.3f;
			pDefTop.Friction = 0.2f;
			pDefTop.Density = 0;
			pDefTop.SetAsBox(lengthOfSide, 1);

			frame = Field.CreateBody(bDefTop);
			frame.CreateShape(pDefTop);

			//right
			BodyDef bDefRight = new BodyDef();
			bDefRight.Position.Set(lengthOfSide - 1, 0);
			bDefRight.Angle = 0;

			PolygonDef pDefRight = new PolygonDef();
			pDefRight.Restitution = 0.3f;
			pDefRight.Friction = 0.2f;
			pDefRight.Density = 0;
			pDefRight.SetAsBox(1, lengthOfSide);

			frame = Field.CreateBody(bDefRight);
			frame.CreateShape(pDefRight);
		}

		public void AddGameObject(IGameObject gameObject)
		{
			GameObjects.Add(gameObject.ID, gameObject);
			//посылваем сообщение об добавлении нового объекта
			//отправляем просто состояние объекта(не вижу смысла создавать специальное сообщение для этого)
			HappenedEvents.Enqueue(gameObject.State);
		}

		public void RemoveGameObject(IGameObject gameObject)
		{
			GameObjects.Remove(gameObject.ID);
		}

		public void Dispose()
		{
			GameObjects.Clear();
			Field.Dispose();
			HappenedEvents.Clear();
		}

		//возвращает коллекцию объектов, которые можно поднять
		public List<IGameObject> GetPickUpObjects(RectangleF shapePlayer)
		{
			List<IGameObject> pickUpObjects = new List<IGameObject>();

			foreach (var gameObject in Loot)
			{

				var shapeLoot = gameObject.Components.GetComponent<TransparentBody>().Shape;
				if (shapeLoot.IntersectsWith(shapePlayer))
				{
					pickUpObjects.Add(gameObject);
				}
				break;

			}
			return pickUpObjects;
		}

		public void AddLoot(IGameObject gameObject)
		{
			Loot.Add(gameObject);
		}

		public void AddEvent(IMessage message)
		{
			HappenedEvents.Enqueue(message);
		}

		public void RemoveLoot(IGameObject gameObject)
		{
			Loot.Remove(gameObject);
		}
	}
}