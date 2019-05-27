using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Collections.Concurrent;
using CommonLibrary.CommonElements;
using System.Windows.Media.Imaging;
using CommonLibrary;
using ObservalableExtended;

namespace BattleRoyalClient
{
	class BattleModel : IBattleModelForController, IBattleModelForView
	{
		public event BattleModelChangedHandler BattleModelChanged;
		public event GameObjectChangedHandler GameObjectChanged;
		public event ModelEndGame HappenedModelEndGame;
		public event ServerDisconnect HappenedDisconnectServer;
		public event ModelLoaded ModelLoaded;

		public Dictionary<ulong, GameObject> gameObjects = new Dictionary<ulong, GameObject>();
		public int CountPlayersInGame { get; private set; } = 0;
		public Size SizeMap { get; private set; } = Size.Empty;
		public bool ModelIsLoaded { get; private set; } = false;

		private Task taskForHandlerIncomingMsg;
		private DeathZone deathZone;
		private ObservableQueue<IMessage> queueIncomingMsg = new ObservableQueue<IMessage>();
		private PlayerCharacter character;
		private bool workingTaskForHandlerIncomingMsg = false;

		public ICharacterForView CharacterView { get => character; }
		public IDeathZoneForView DeathZone { get => deathZone; }
		public ulong IDPlayer { get => character.ID; }

		public void AddOutgoingMsg(IMessage msg)
		{
			queueIncomingMsg.Enqueue(msg);
		}

		public BattleModel()
		{
			
		}

		public void ClearModel()
		{
			CountPlayersInGame = 0;
			SizeMap = Size.Empty;
			ModelIsLoaded = false;
			workingTaskForHandlerIncomingMsg = false;
			queueIncomingMsg.Clear();
			gameObjects.Clear();
			character = null;

			BattleModelChanged = null;
			GameObjectChanged = null;
			HappenedModelEndGame = null;
			HappenedDisconnectServer = null;
		}

		public void Initialize(ulong id)
		{
			character = new PlayerCharacter(id, this);
			taskForHandlerIncomingMsg = new Task(Handler_IncomingMsg);
			workingTaskForHandlerIncomingMsg = true;
			taskForHandlerIncomingMsg.Start();
		}

		private void Handler_IncomingMsg()
		{
			while (workingTaskForHandlerIncomingMsg)
			{
				if (queueIncomingMsg.Count == 0)
					Thread.Sleep(1);
				else ChooseHandlerForMsg(queueIncomingMsg.Dequeue());
			}
		}

		private void ChooseHandlerForMsg(IMessage msg, IMessage msgParent = null)
		{
			switch (msg.TypeMessage)
			{
				case TypesMessage.RoomState:
					Handler_RoomState(msg);
					break;
				case TypesMessage.ObjectMoved:
					Handler_ObjectMoved(msg);
					break;
				case TypesMessage.ChangedValueHP:
					Handler_ChangedValueHP(msg);
					break;
				case TypesMessage.DeletedInMap:
					Handler_DeletedInMap(msg);
					break;
				case TypesMessage.EndGame:
					Handler_EndGame(msg);
					break;
				case TypesMessage.WeaponState:
				case TypesMessage.GameObjectState:
					Handler_GameObjectState(msg);
					break;
				case TypesMessage.ChangedTimeTillReduction:
					Handler_ChangedTimeTillReduction(msg);
					break;
				case TypesMessage.ChangedCurrentWeapon:
					Handler_ChangedCurrentWeapon(msg);
					break;
				case TypesMessage.ReloadWeapon:
					Handler_Reload(msg);
					break;
				case TypesMessage.PlayerTurn:
					Handler_PlayerTurned(msg);
					break;
				case TypesMessage.MakedShot:
					Handler_MakedShot(msg);
					break;
				case TypesMessage.AddWeapon:
					Handler_AddWeapon(msg);
					break;
				case TypesMessage.ChangeCountPayersInGame:
					Handler_ChangeCountPayersInGame(msg);
					break;
				case TypesMessage.ChangeBulletInWeapon:
					Handler_ChangeBulletInWeapon(msg);
					break;
				case TypesMessage.BodyState:
					Handler_BodyState(msgParent, msg);
					break;
				case TypesMessage.BodyZoneState:
					Handler_BodyZoneState(msg);
					break;
				case TypesMessage.CurrentWeaponState:
					Handler_CurrentWeaponState(msgParent, msg);
					break;
				case TypesMessage.CollectorState:
					Handler_CollectorState(msg);
					break;
				case TypesMessage.HealthyState:
					Handler_ChangeHP(msg);
					break;
				case TypesMessage.MagazinState:
					Handler_MagazinState(msgParent, msg);
					break;
				case TypesMessage.ConnectionBroken:
					Handler_ConnectionBroken(msg);
					break;
			}
		}

		private void MarkModelAsLoaded()
		{
			ModelIsLoaded = true;
			ModelLoaded?.Invoke();
		}

		private void Handler_RoomState(IMessage msg)
		{
			ProcessInternalMsg(msg);
			//обработано первое сообщение RoomState - значит комната загружена
			MarkModelAsLoaded();
		}

		private void Handler_ObjectMoved(IMessage msg)
		{
			ChangeLocationAtObject(msg.ID, msg.Location);
			CreateChangeGameObject(msg.ID);
			if (msg.ID == character.ID)
			{
				character.ChangeLocation(msg.Location);
			}
		}

		private void Handler_ChangedValueHP(IMessage changedValueHP)
		{
			character.ChangeHP(changedValueHP.HP);
		}

		private void Handler_DeletedInMap(IMessage deleteInMap)
		{
			IModelObject modelObject = RemoveObject(deleteInMap.ID);

			if (modelObject.Type == TypesGameObject.Grenade)
			{
				var exploison = CreateExplosion(modelObject.Location);
				CreateChangeGameObject(exploison);
			}

			CreateChangeGameObject(modelObject, StateObject.Delete);
		}

		private void Handler_EndGame(IMessage msg)
		{
			StopHandlerMessages();
			CreateEventEndGame(msg);
		}

		private void Handler_GameObjectState(IMessage msg)
		{
			if (!gameObjects.ContainsKey(msg.ID))
			{
				GameObject addingObject = FactoryForCreateGameObjects.CreateGameObjects(msg);
				AddGameObject(addingObject);
				CreateChangeGameObject(addingObject);
				if (msg.ID == character.ID)
				{
					character.Create(addingObject as Gamer);
				}
			}
			else //обновляем
			{
				ProcessInternalMsg(msg);
				CreateChangeGameObject(msg.ID);
			}
		}

		private void Handler_Reload(IMessage msg)
		{
			character.ChangeReloadState(msg.StartOrEnd);
		}


		private void Handler_MakedShot(IMessage msg)
		{
			var traser = CreateTraser(msg.ID, msg.Distance, msg.Angle);
			CreateChangeGameObject(traser);
		}

		private void Handler_PlayerTurned(IMessage msg)
		{
			TurnedGameObject(msg.ID, msg.Angle);
			CreateChangeGameObject(msg.ID);
		}

		private void Handler_ChangedCurrentWeapon(IMessage msg)
		{
			ChangeCurrentWeaponAtGamer(msg.ID, msg.TypeWeapon);
			if (msg.ID == character.ID)
				character.ChangeCurrentWeapon(msg.TypeWeapon);

			CreateChangeGameObject(msg.ID);
		}

		private void Handler_ChangedTimeTillReduction(IMessage msg)
		{
			ChangeTimeTillReductionAtDeathZone(msg.Time);
			CreateChangeGameObject(msg.ID);
		}

		private void Handler_AddWeapon(IMessage msg)
		{
			character.AddWeapon(msg.TypeWeapon);
			ProcessInternalMsg(msg);
		}

		private void Handler_ChangeCountPayersInGame(IMessage msg)
		{
			ChangeCountPlayersInGame(msg.Count);
			CreateChangeModel(TypesChange.CountPlyers);
		}

		private void Handler_BodyState(IMessage msgParent, IMessage msg)
		{
			ChanageShapeAtObject(msgParent.ID, msg.Shape);
		}

		private void Handler_ChangeBulletInWeapon(IMessage msg)
		{
			character.ChangeBulletInWeapon(msg.TypeWeapon, msg.Count);
		}

		private void Handler_CollectorState(IMessage msg)
		{
			//к сожалению стандартный метод обработки не подходит
			//поэтому оставляю так
			foreach (IMessage message in msg.InsertCollections)
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.WeaponState:
						ProcessInternalMsg(message);
						break;
				}
			}
		}

		private void Handler_CurrentWeaponState(IMessage msgParent, IMessage msg)
		{
			ChangeCurrentWeaponAtGamer(msgParent.ID, msg.TypeWeapon);
			CreateChangeModel( TypesChange.CurrentWeapon);
		}

		private void Handler_BodyZoneState(IMessage msg)
		{
			ChangeShapeAtZoneDeath(msg.Location, msg.Radius);
			CreateChangeModel(TypesChange.ShapeAtZoneDeath);
		}

		private void Handler_ChangeHP(IMessage msg)
		{
			character.ChangeHP(msg.HP);
		}

		private void ProcessInternalMsg(IMessage msg)
		{
			foreach (IMessage message in msg.InsertCollections)
			{
				ChooseHandlerForMsg(message, msg);
			}
		}

		private void Handler_MagazinState(IMessage msgParent, IMessage msg)
		{
			character.ChangeBulletInWeapon(msgParent.TypeWeapon, msg.Count);
		}

		private void Handler_ConnectionBroken(IMessage msg)
		{
			HappenedDisconnectServer?.Invoke();
		}

		private void CreateChangeModel(TypesChange typeChange)
		{
			BattleModelChanged?.Invoke(typeChange);
		}

		private void CreateEventEndGame(IMessage msg)
		{
			HappenedModelEndGame?.Invoke(msg);
		}

		private void CreateChangeGameObject(ulong idObject, StateObject state = StateObject.Change)
		{
			if(gameObjects.ContainsKey(idObject))
			GameObjectChanged?.Invoke(gameObjects[idObject], state);
		}

		private void CreateChangeGameObject(IModelObject modelObject, StateObject state = StateObject.Change)
		{
			GameObjectChanged?.Invoke(modelObject, state);
		}

		private void ChangeCountPlayersInGame(int newCount)
		{
			CountPlayersInGame = newCount;
		}

		private Traser CreateTraser(ulong idPlayer, float distance, float angle)
		{
			var gunMan = gameObjects[idPlayer];
			Traser traser = new Traser(ulong.MaxValue, gunMan.Location,
				new SizeF(distance * 2, distance * 2), angle);
			return traser;
		}

		private void TurnedGameObject(ulong idObject, float angle)
		{
			if (!gameObjects.ContainsKey(idObject)) return;
			gameObjects[idObject].Update(angle);
		}

		private void ChangeCurrentWeaponAtGamer(ulong idGamer, TypesWeapon typeWeapon)
		{
			if (!gameObjects.ContainsKey(idGamer)) return;
			var gamer = gameObjects[idGamer] as Gamer;
			if (gamer != null)
			{
				gamer.CurrentWeapon = typeWeapon;
			}
		}

		private void ChangeTimeTillReductionAtDeathZone(TimeSpan time)
		{
			if (deathZone == null) return;
			deathZone.TimeToChange = time;
		}

		private IModelObject RemoveObject(ulong idObject)
		{
			if (gameObjects.ContainsKey(idObject))
			{
				IModelObject modelObject = gameObjects[idObject];
				if (gameObjects.Remove(idObject))
				{
					return modelObject;
				}
			}
			return null;
		}

		private Explosion CreateExplosion(PointF location)
		{
			Explosion explosion = new Explosion(ulong.MaxValue, location);
			return explosion;
		}

		private void ChangeLocationAtObject(ulong id, PointF location)
		{
			if (!gameObjects.ContainsKey(id)) return;
			gameObjects[id].Update(location);
		}

		private void AddGameObject(GameObject modelObject)
		{
			gameObjects.Add(modelObject.ID, modelObject);
			
			switch (modelObject.Type)
			{
				case TypesGameObject.DeathZone:
					deathZone = (DeathZone)modelObject;
					break;
			}
		}

		private void ChanageShapeAtObject(ulong idObject, RectangleF shape)
		{
			if (!gameObjects.ContainsKey(idObject)) return;
			gameObjects[idObject].Update(shape);
		}

		private void ChangeShapeAtZoneDeath(PointF location, float radius)
		{
			if (deathZone == null) return;
			deathZone.Update(new RectangleF(location, new SizeF(radius*2, radius*2)));
		}

		private void StopHandlerMessages()
		{
			workingTaskForHandlerIncomingMsg = false;
		}

	}

	public enum TypesChange
	{
		CountPlyers,
		CurrentWeapon,
		ShapeAtZoneDeath
	}
}
