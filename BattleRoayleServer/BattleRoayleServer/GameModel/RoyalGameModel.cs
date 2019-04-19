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
		private object sinchDelitePlayer = new object();
        //только на чтение
        public IList<IPlayer> Players { get; private set; }
		/// <summary>
		/// Коллекция всех игровых объектов в игре
		/// </summary>
        public Dictionary<ulong,IGameObject> GameObjects { get; private set; }
		public DeathZone Zone { get; private set; }
        public World Field { get; private set; }
		/// <summary>
		/// Колекция событий произошедших в игре
		/// </summary>
		public ObservableQueue<IMessage> HappenedEvents { get; private set; }

		public GameObjectState State
		{
			get
			{
				var listState = new List<IMessage>();
				//отправляем характеристики карты
				listState.Add(new FieldState(new SizeF(lengthOfSide, lengthOfSide)));

				return new GameObjectState(0, TypesGameObject.Field, listState);
			}
		}

		/// <summary>
		/// Содержит алгоритм наполнения карты игровыми объектами
		/// </summary>
		/// <remarks>
		/// По умолчанию создает список объектов.
		/// Должен создавать объекты, добавлять их в список и на карту.
		/// </remarks>
		private void CreateStaticGameObject()
		{
			//статическое задание
			//скрипт создания игровых объектов
			//15 камней
			#region 
			Stone stone = new Stone(this, new PointF(10, 10), new Size(14,14));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(15, 28), new Size(9, 9));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(56, 210), new Size(15, 15));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(89, 360), new Size(10, 10));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(110, 480), new Size(12, 12));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(130, 240), new Size(15, 15));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(178, 102), new Size(8, 8));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(202, 42), new Size(16, 16));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(248, 208), new Size(11, 11));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(299, 409), new Size(14, 14));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(329, 78), new Size(12, 12));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(347, 289), new Size(15, 15));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(378, 480), new Size(14, 14));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(410, 16), new Size(12, 12));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(436, 247), new Size(10, 10));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(487, 100), new Size(14, 14));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(250, 250), new Size(13, 13));
			GameObjects.Add(stone.ID, stone);

			stone = new Stone(this, new PointF(127, 30), new Size(15, 15));
			GameObjects.Add(stone.ID, stone);
			#endregion

			//12 коробок
			#region
			Box box = new Box(this, new PointF(40, 100));
			GameObjects.Add(box.ID, box);
			
			box = new Box(this, new PointF(150, 300));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(86, 423));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(162, 300));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(174, 300));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(300, 425));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(300, 413));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(300, 401));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(368, 148));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(312, 172));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(218, 178));
			GameObjects.Add(box.ID, box);

			box = new Box(this, new PointF(278, 132));
			GameObjects.Add(box.ID, box);

			#endregion

			//2 автомата
			#region
			AssaultRifle assaultRifle = new AssaultRifle(this, new PointF(250, 210));
			GameObjects.Add(assaultRifle.ID, assaultRifle);

			assaultRifle = new AssaultRifle(this, new PointF(300, 150));
			GameObjects.Add(assaultRifle.ID, assaultRifle);
			#endregion

			//3 шот-гана
			#region
			ShotGun shotGun = new ShotGun(this, new PointF(400, 300));
			GameObjects.Add(shotGun.ID, shotGun);

			shotGun = new ShotGun(this, new PointF(90, 485));
			GameObjects.Add(shotGun.ID, shotGun);

			shotGun = new ShotGun(this, new PointF(300, 10));
			GameObjects.Add(shotGun.ID, shotGun);
			#endregion

			//3 гранат
			#region
			GrenadeCollection grenade = new GrenadeCollection(this, new PointF(80, 344));
			GameObjects.Add(grenade.ID, grenade);

			grenade = new GrenadeCollection(this, new PointF(3, 269));
			GameObjects.Add(grenade.ID, grenade);

			grenade = new GrenadeCollection(this, new PointF(45, 156));
			GameObjects.Add(grenade.ID, grenade);
			#endregion

			//4 пистолета
			#region
			Gun gun = new Gun(this, new PointF(10, 10));
			GameObjects.Add(gun.ID, gun);

			gun = new Gun(this, new PointF(21, 480));
			GameObjects.Add(gun.ID, gun);

			gun = new Gun(this, new PointF(476, 486));
			GameObjects.Add(gun.ID, gun);

			gun = new Gun(this, new PointF(490, 5));
			GameObjects.Add(gun.ID, gun);
			#endregion

			//12 кустов
			#region
			Bush bush = new Bush(this, new PointF(80, 90));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(200, 100));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(100, 200));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(45, 400));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(156, 312));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(420, 118));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(300, 250));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(243, 450));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(245, 341));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(262, 40));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(410,323));
			GameObjects.Add(bush.ID, bush);

			bush = new Bush(this, new PointF(489, 90));
			GameObjects.Add(bush.ID, bush);

			#endregion
			//10 деревьев
			#region
			Tree tree = new Tree(this, new PointF(100, 250));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(43, 100));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(89, 363));
			GameObjects.Add(tree.ID, tree);
			
			tree = new Tree(this, new PointF(129, 403));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(168, 60));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(210, 80));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(289, 261));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(320, 56));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(378, 470));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(430, 241));
			GameObjects.Add(tree.ID, tree);

			tree = new Tree(this, new PointF(476, 412));
			GameObjects.Add(tree.ID, tree);

			#endregion

			DeathZone deathZone = new DeathZone(this, lengthOfSide);
			GameObjects.Add(deathZone.ID, deathZone);
			Zone = deathZone;
		}
		private void CreateDinamicGameObject()
		{
			//15 камней
			for (int i = 0; i < 20; i++)
			{
				Stone stone = new Stone(this, VectorMethod.CreateRandPosition(lengthOfSide), new Size(16, 16));
				GameObjects.Add(stone.ID, stone);
			}
			//12 коробок
			for (int i = 0; i < 15; i++)
			{
				Box box = new Box(this, VectorMethod.CreateRandPosition(lengthOfSide));
				GameObjects.Add(box.ID, box);
			}

			//4 автомата
			for (int i = 0; i < 4; i++)
			{
				AssaultRifle assaultRifle = new AssaultRifle(this, VectorMethod.CreateRandPosition(lengthOfSide));
				GameObjects.Add(assaultRifle.ID, assaultRifle);
			}

			//5 шот-ганов
			for (int i = 0; i < 5; i++)
			{
				ShotGun shotGun = new ShotGun(this, VectorMethod.CreateRandPosition(lengthOfSide));
				GameObjects.Add(shotGun.ID, shotGun);
			}

			//5 гранат
			for (int i = 0; i < 5; i++)
			{
				GrenadeCollection grenade = new GrenadeCollection(this, VectorMethod.CreateRandPosition(lengthOfSide));
				GameObjects.Add(grenade.ID, grenade);
			}

			//8 пистолетов
			for (int i = 0; i < 8; i++)
			{
				Gun gun = new Gun(this, VectorMethod.CreateRandPosition(lengthOfSide));
				GameObjects.Add(gun.ID, gun);
			}

			//12 кустов
			for (int i = 0; i < 25; i++)
			{
				Bush bush = new Bush(this, VectorMethod.CreateRandPosition(lengthOfSide));
				GameObjects.Add(bush.ID, bush);
			}

			//10 деревьев
			for (int i = 0; i < 20; i++)
			{
				Tree tree = new Tree(this, VectorMethod.CreateRandPosition(lengthOfSide));
				GameObjects.Add(tree.ID, tree);
			}

			DeathZone deathZone = new DeathZone(this, lengthOfSide);
			GameObjects.Add(deathZone.ID, deathZone);
			Zone = deathZone;
		}

		private void Model_EventGameObjectDeleted(IGameObject gameObject)
		{
			GameObjects.Remove(gameObject.ID);
		}

		/// <summary>
		/// Создает игроков и добавляет их на карту в список игроков и список игрвовых объектов
		/// </summary>
		private void CreatePlayers(int gamersInRoom)
		{
			for (int i = 0; i < gamersInRoom; i++)
			{
				//создаем игрока
				Gamer gamer = new Gamer(this,VectorMethod.CreateRandPosition(lengthOfSide));
				Players.Add(gamer);
				GameObjects.Add(gamer.ID,gamer);
			}
			HappenedEvents.Enqueue(new ChangeCountPlayersInGame(0, Players.Count));
		}

		public void RemovePlayer(Gamer player)
		{
			lock (sinchDelitePlayer)
			{
				Players.Remove(player);
				player.SetDestroyed();
				HappenedEvents.Enqueue(new ChangeCountPlayersInGame(0, Players.Count));
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

			AABB frameField = new AABB();
			frameField.LowerBound.Set(0,0);
			frameField.UpperBound.Set(lengthOfSide, lengthOfSide);
			Field = new World(frameField, new Vec2(0, 0), false);
			var solver = new RoomContactListener();
			Field.SetContactListener(solver);
			CreateFrame();


			//создание и добавление в GameObjects и Field статических объектов карты
			//CreateStaticGameObject();
			CreateDinamicGameObject();
			CreatePlayers(gamersInRoom);

			//настраиваем игровые объекты
			foreach (var key in GameObjects.Keys)
			{
				GameObjects[key].Setup();
			}

		}

		//только для тестов
		public RoyalGameModel()
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			GameObjects = new Dictionary<ulong, IGameObject>();
			HappenedEvents = new ObservableQueue<IMessage>();

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
			pDefBottom.Restitution = 0f;
			pDefBottom.Friction = 0;
			pDefBottom.Density = 0;
			pDefBottom.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefBottom.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefBottom.SetAsBox(lengthOfSide, 1);

			var frame  = Field.CreateBody(bDefBottom);
			frame.CreateShape(pDefBottom);

			//left
			BodyDef bDefLeft = new BodyDef();
			bDefLeft.Position.Set(0, 0);
			bDefLeft.Angle = 0;

			PolygonDef pDefLeft = new PolygonDef();
			pDefLeft.Restitution = 0;
			pDefLeft.Friction = 0;
			pDefLeft.Density = 0;
			pDefLeft.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefLeft.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefLeft.SetAsBox(1, lengthOfSide);

			frame = Field.CreateBody(bDefLeft);
			frame.CreateShape(pDefLeft);

			//top
			BodyDef bDefTop = new BodyDef();
			bDefTop.Position.Set(0, lengthOfSide - 1);
			bDefTop.Angle = 0;

			PolygonDef pDefTop = new PolygonDef();
			pDefTop.Restitution = 0;
			pDefTop.Friction = 0;
			pDefTop.Density = 0;
			pDefTop.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefTop.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefTop.SetAsBox(lengthOfSide, 1);

			frame = Field.CreateBody(bDefTop);
			frame.CreateShape(pDefTop);

			//right
			BodyDef bDefRight = new BodyDef();
			bDefRight.Position.Set(lengthOfSide - 1, 0);
			bDefRight.Angle = 0;

			PolygonDef pDefRight = new PolygonDef();
			pDefRight.Restitution = 0;
			pDefRight.Friction = 0;
			pDefRight.Density = 0;
			pDefRight.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefRight.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefRight.SetAsBox(1, lengthOfSide);

			frame = Field.CreateBody(bDefRight);
			frame.CreateShape(pDefRight);
		}

		public void AddOrUpdateGameObject(IGameObject gameObject)
		{
			if (!GameObjects.ContainsKey(gameObject.ID))
			{
				GameObjects.Add(gameObject.ID, gameObject);
			}
			
			gameObject.Setup();
			//посылваем сообщение об добавлении нового объекта
			//отправляем просто состояние объекта(не вижу смысла создавать специальное сообщение для этого)
			HappenedEvents.Enqueue(gameObject.State);
		}

		public void RemoveGameObject(IGameObject gameObject)
		{
			gameObject.Dispose();
			GameObjects.Remove(gameObject.ID);
		}

		public void Dispose()
		{
			lock (sinchDelitePlayer)
			{
				//выполняем все необходимые действия при уничтожении для всех оставшихся игроков
				for (; Players.Count != 0;)
				{
					(Players[0] as GameObject).Dispose();
					Players.RemoveAt(0);
				}

				GameObjects.Clear();
				Field.Dispose();
				HappenedEvents.Clear();
			}
		}

		public void AddEvent(IMessage message)
		{
			HappenedEvents.Enqueue(message);
		}

	}
}