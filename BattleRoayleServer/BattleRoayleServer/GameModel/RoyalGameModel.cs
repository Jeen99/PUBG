using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using CommonLibrary;
using CommonLibrary.GameMessages;
using System.Collections.Specialized;
using System.Drawing;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonLibrary.CommonElements;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using ObservalableExtended;

namespace BattleRoayleServer
{
	public class RoyalGameModel : IGameModel, IModelForComponents
	{
		public static readonly float LengthOfSideMap = 500;
		private bool roomClosing = false;
		private Task handlerIncomingMessages;
		private const int minValueGamerInBattle = 0;
		private Dictionary<ulong, IGameObject> gameObjects;

		public IList<IPlayer> Players { get; private set; }
		public GameObject DeathZone { get; private set; }
		public World Field { get; private set; }

		public ObservableQueue<IMessage> OutgoingMessages { get; private set; }
		public ObservableQueue<IMessage> IncomingMessages { get; private set; }

		public event HappenedEndGame Event_HappenedEndGame;

		private void CreateDinamicGameObject(List<RectangleF> occupiedArea)
		{
			//15 камней
			for (int i = 0; i < 20; i++)
			{
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeStone);
				var stone = BuilderGameObject.CreateStone(this, newShape.Location);
				gameObjects.Add(stone.ID, stone);
			}
			//12 коробок
			for (int i = 0; i < 15; i++)
			{
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeBox);
				var box = BuilderGameObject.CreateBox(this, newShape.Location);
				gameObjects.Add(box.ID, box);
			}

			//4 автомата
			for (int i = 0; i < 4; i++)
			{
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeAssaulRiffle);
				var assaultRifle = BuilderGameObject.CreateAssaultRiffle(this, newShape.Location);
				gameObjects.Add(assaultRifle.ID, assaultRifle);
			}

			//5 шот-ганов
			for (int i = 0; i < 5; i++)
			{
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeShotGun);
				var shotGun = BuilderGameObject.CreateShotGun(this, newShape.Location);
				gameObjects.Add(shotGun.ID, shotGun);
			}

			//5 гранат
			for (int i = 0; i < 5; i++)
			{
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeGrenadeCollection);
				var grenade = BuilderGameObject.CreateGrenadeCollection(this, newShape.Location);
				gameObjects.Add(grenade.ID, grenade);
			}

			//8 пистолетов
			for (int i = 0; i < 8; i++)
			{
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeGun);
				var gun = BuilderGameObject.CreateGun(this, newShape.Location);
				gameObjects.Add(gun.ID, gun);
			}

			//12 кустов
			for (int i = 0; i < 25; i++)
			{
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeBush);
				var bush = BuilderGameObject.CreateBush(this, newShape.Location);
				gameObjects.Add(bush.ID, bush);
			}

			//10 деревьев
			for (int i = 0; i < 20; i++)
			{
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeTree);
				var tree = BuilderGameObject.CreateTree(this, newShape.Location);
				gameObjects.Add(tree.ID, tree);
			}
		}

		private RectangleF CreateAndAddNewUniqueShape(List<RectangleF> occupiedArea, SizeF sizeObject)
		{
			RectangleF newShape;
			bool haveIntersection;
			do
			{
				haveIntersection = false;
				newShape = new RectangleF(VectorMethod.CreateRandPosition(LengthOfSideMap), sizeObject);

				foreach (var shape in occupiedArea)
				{
					if (shape.IntersectsWith(newShape))
					{
						haveIntersection = true;
						break;
					}
				}
			} while (haveIntersection);

			occupiedArea.Add(newShape);

			return newShape;
		}


		private void Model_EventGameObjectDeleted(IGameObject gameObject)
		{
			gameObjects.Remove(gameObject.ID);
		}

		/// <summary>
		/// Создает игроков и добавляет их на карту в список игроков и список игрвовых объектов
		/// </summary>
		private void CreatePlayers(int gamersInRoom, List<RectangleF> occupiedArea)
		{
			for (int i = 0; i < gamersInRoom; i++)
			{
				//создаем игрока
				RectangleF newShape = CreateAndAddNewUniqueShape(occupiedArea, BuilderGameObject.SizeGamer);
				var gamer = BuilderGameObject.CreateGamer(this, newShape.Location);
				Players.Add(gamer);
				gameObjects.Add(gamer.ID,gamer);
			}
			OutgoingMessages.Enqueue(new ChangeCountPlayersInGame(0, Players.Count));
		}

		private void RemovePlayer(Gamer player)
		{
			Players.Remove(player);
			player.SetDestroyed();
			OutgoingMessages.Enqueue(new ChangeCountPlayersInGame(0, Players.Count));
			if( Players.Count <= minValueGamerInBattle)
			{
				Event_HappenedEndGame?.Invoke();
			}
		}
			
		public RoyalGameModel(int gamersInRoom)
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			gameObjects = new Dictionary<ulong, IGameObject>();
			OutgoingMessages = new ObservableQueue<IMessage>();
			IncomingMessages = new ObservableQueue<IMessage>();

			AABB frameField = new AABB();
			frameField.LowerBound.Set(0,0);
			frameField.UpperBound.Set(LengthOfSideMap, LengthOfSideMap);
			Field = new World(frameField, new Vec2(0, 0), false);
			var solver = new RoomContactListener();
			Field.SetContactListener(solver);
			CreateFrame();

			DeathZone = BuilderGameObject.CreateDeathZone(this);
			gameObjects.Add(DeathZone.ID, DeathZone);

			//создание и добавление в GameObjects и Field статических объектов карты
			List<RectangleF> occupiedArea = new List<RectangleF>();
			CreateDinamicGameObject(occupiedArea);
			CreatePlayers(gamersInRoom, occupiedArea);

			//настраиваем игровые объекты
			foreach (var key in gameObjects.Keys)
			{
				gameObjects[key].Setup();
			}

			handlerIncomingMessages = new Task(Handler_IncomingMessages);
			handlerIncomingMessages.Start();
		}

		//только для тестов
		public RoyalGameModel()
		{
			//инициализируем полей
			Players = new List<IPlayer>();
			gameObjects = new Dictionary<ulong, IGameObject>();
			OutgoingMessages = new ObservableQueue<IMessage>();
			IncomingMessages = new ObservableQueue<IMessage>();

			AABB frameField = new AABB();
			frameField.LowerBound.Set(0, 0);
			frameField.UpperBound.Set(LengthOfSideMap, LengthOfSideMap);
			Field = new World(frameField, new Vec2(0, 0), false);
			var solver = new RoomContactListener();
			Field.SetContactListener(solver);
			CreateFrame();

			DeathZone = BuilderGameObject.CreateDeathZone(this);
			gameObjects.Add(DeathZone.ID, DeathZone);

			handlerIncomingMessages = new Task(Handler_IncomingMessages);
			handlerIncomingMessages.Start();
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
			pDefBottom.SetAsBox(LengthOfSideMap, 1);

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
			pDefLeft.SetAsBox(1, LengthOfSideMap);

			frame = Field.CreateBody(bDefLeft);
			frame.CreateShape(pDefLeft);

			//top
			BodyDef bDefTop = new BodyDef();
			bDefTop.Position.Set(0, LengthOfSideMap - 1);
			bDefTop.Angle = 0;

			PolygonDef pDefTop = new PolygonDef();
			pDefTop.Restitution = 0;
			pDefTop.Friction = 0;
			pDefTop.Density = 0;
			pDefTop.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefTop.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefTop.SetAsBox(LengthOfSideMap, 1);

			frame = Field.CreateBody(bDefTop);
			frame.CreateShape(pDefTop);

			//right
			BodyDef bDefRight = new BodyDef();
			bDefRight.Position.Set(LengthOfSideMap - 1, 0);
			bDefRight.Angle = 0;

			PolygonDef pDefRight = new PolygonDef();
			pDefRight.Restitution = 0;
			pDefRight.Friction = 0;
			pDefRight.Density = 0;
			pDefRight.Filter.CategoryBits = (ushort)CollideCategory.Box;
			pDefRight.Filter.MaskBits = (ushort)CollideCategory.Player;
			pDefRight.SetAsBox(1, LengthOfSideMap);

			frame = Field.CreateBody(bDefRight);
			frame.CreateShape(pDefRight);
		}

		public void AddOrUpdateGameObject(IGameObject gameObject)
		{
			if (!gameObjects.ContainsKey(gameObject.ID))
			{
				gameObjects.Add(gameObject.ID, gameObject);
			}
			
			gameObject.Setup();
			//посылваем сообщение об добавлении нового объекта
			//отправляем просто состояние объекта(не вижу смысла создавать специальное сообщение для этого)
			OutgoingMessages.Enqueue(gameObject.State);
		}

		public void RemoveGameObject(IGameObject gameObject)
		{
			gameObject.Dispose();
			gameObjects.Remove(gameObject.ID);
		}

		public void Dispose()
		{
			roomClosing = true;
			//выполняем все необходимые действия при уничтожении для всех оставшихся игроков
			for (; Players.Count != 0;)
			{
				(Players[0] as GameObject).Dispose();
				Players.RemoveAt(0);
			}

			handlerIncomingMessages.Wait();
			handlerIncomingMessages.Dispose();
			gameObjects.Clear();
			Field.Dispose();
		}

		public void AddOutgoingMessage(IMessage message)
		{
			OutgoingMessages.Enqueue(message);
		}

		public IMessage GetOutgoingMessage()
		{
			if (OutgoingMessages.Count == 0)
			{
				return null;
			}
			else
				return OutgoingMessages.Dequeue();
		}

		public void AddIncomingMessage(IMessage message)
		{
			IncomingMessages.Enqueue(message);
		}

		/// <summary>
		/// Делаем шаг игровой карты
		/// </summary>
		/// <param name="passedTime">в миллесекундах</param>
		public void MakeStep(int passedTime)
		{
			Field.Step((float)passedTime / 1000, 8, 3);
			IMessage msg = new TimeQuantPassed(passedTime);

			for (Body list = Field.GetBodyList(); list != null; list = list.GetNext())
			{
				if (list.GetUserData() != null)
				{
					SolidBody solidBody = (SolidBody)list.GetUserData();
					if (!solidBody.Parent.Destroyed)
					{
						if (solidBody.Parent.TypeBehave == TypesBehaveObjects.Active)
						{
							//запускаем  обработку всех событий на этом объекте
							solidBody.Parent.Update(msg);
						}
					}
					//если объект уничтожен удаляем его
					else
					{
						RemoveGameObject(solidBody.Parent);
						if (solidBody.Parent is Gamer)
						{
							RemovePlayer(solidBody.Parent as Gamer);
						}
					}
				}
			}
			//обновляем игровую зону
			DeathZone.Update(msg);
		}

		private void Handler_IncomingMessages()
		{
			while (!roomClosing)
			{
				if (IncomingMessages.Count > 0)
				{
					IMessage msg = IncomingMessages.Dequeue();
					if(gameObjects.ContainsKey(msg.ID))
						gameObjects[msg.ID]?.Update(msg);
				}
				else Thread.Sleep(1);
			}
		}

		/// <summary>
		/// Создает список состяний игровых объектов (только активных)
		/// </summary>
		public IMessage RoomState
		{
			get
			{
				List<IMessage> states = new List<IMessage>();
				foreach (var gameObject in gameObjects)
				{
					if (gameObject.Value.TypeBehave == TypesBehaveObjects.Active)
					{
						IMessage msg = gameObject.Value.State;
						if (msg != null) states.Add(msg);
					}
				}
				return new RoomState(states);
			}
		}

		public IMessage FullRoomState
		{
			get
			{
				List<IMessage> fullStates = new List<IMessage>();
				foreach (var gameObject in gameObjects)
				{
					IMessage msg = gameObject.Value.State;
					if (msg != null) fullStates.Add(msg);
				}

				fullStates.Add(new GameObjectState(0, TypesGameObject.Field,
					new List<IMessage> { new FieldState(new SizeF(LengthOfSideMap, LengthOfSideMap)) }));
				return new RoomState(fullStates);
			}
		}

		public List<SolidBody> GetMetedObjects(Segment ray)
		{
			//получаем только первый встетившийся на пути пули объект
			Shape[] metedShapes = new Shape[10];
			Field.Raycast(ray, metedShapes, metedShapes.Length, true, null);

			List<SolidBody> metedObects = new List<SolidBody>();
			for (int i = 0; i < metedShapes.Length; i++)
			{
				if (metedShapes[i] != null)
				{
					var gameObject = (SolidBody)metedShapes[i].GetBody()?.GetUserData();
					if (gameObject != null)
					{
						metedObects.Add(gameObject);
					}
				}
				else break;
			}

			return metedObects;
		}
	}
}