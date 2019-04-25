using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CommonLibrary.GameMessages;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;

namespace BattleRoyalClient
{
	class GameActionController
	{
		private BattleModel model;
		public IBattleModel Model {
			get
			{
				return model;
			}
		}
		public bool Loaded { get; private set; } = false;
		private BaseClient<IMessage> client;
		private BattleView3d view;

		public GameActionController(ulong id, BaseClient<IMessage> client, BattleView3d view)
		{
			this.client = client;
			model = new BattleModel(id);
			this.view = view;
			client.EventEndSession += Client_EventEndSession;
			client.EventNewMessage += Client_EventNewMessage;
		}

		private void Client_EventNewMessage()
		{
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesMessage.RoomState:
					Handler_RoomState(msg);
					view.Dispatcher.Invoke(()=> { model.CreateChangeModel(); });			
					break;
				case TypesMessage.ObjectMoved:
					Handler_ObjectMoved(msg);
					break;
				case TypesMessage.ChangedValueHP:
					Handler_HealthyCharacter(msg);
					break;
				case TypesMessage.DeletedInMap:
					Handler_DeletedInMap(msg);
					break;
				case TypesMessage.EndGame:
					Handler_EndGame(msg as EndGame);
					break;
				case TypesMessage.GameObjectState:
					Handler_GameObjectState(msg);
					break;
				case TypesMessage.WeaponState:
					Handler_WeaponState(msg);
					break;
				case TypesMessage.ChangedTimeTillReduction:
					Handler_ChangedTimeTillReduction(msg);
					break;
				case TypesMessage.ChangedCurrentWeapon:
					Handler_ChangedCurrentWeapon(msg);
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
			}
		}
		private void Handler_ChangeCountPayersInGame(IMessage msg)
		{
			model.CountPlayersInGame = msg.Count;
		}

		private void Handler_AddWeapon(IMessage msg)
		{
			model.Chararcter.AddWeapon(msg.TypeWeapon);
			view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
		}

		private void Handler_MakedShot(IMessage msg)
		{
			var gunMan = model.GameObjects[msg.ID];
			Traser traser = new Traser(ulong.MaxValue, gunMan.Location, 
				new SizeF(msg.Distance*2, msg.Distance*2), msg.Angle);
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(traser); });
		}

		private void Handler_PlayerTurned(IMessage msg)
		{
			if (!model.GameObjects.ContainsKey(msg.ID)) return;
			model.GameObjects[msg.ID]?.Update(msg.Angle);
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(model.GameObjects[msg.ID]); });
		}

		private void Handler_ChangedCurrentWeapon(IMessage msg)
		{
			if (!model.GameObjects.ContainsKey(msg.ID)) return;
			var gamer = (model.GameObjects[msg.ID] as Gamer);
			if(gamer != null)
			{
				gamer.CurrentWeapon = msg.TypeWeapon;
				view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gamer); });
				if (msg.ID == model.Chararcter.ID)
					view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
			}
		}

		private void Handler_ChangedTimeTillReduction(IMessage msg)
		{
			if (!model.GameObjects.ContainsKey(msg.ID)) return;
			var deathZone = (model.GameObjects[msg.ID] as DeathZone);
			if (deathZone != null)
			{
				deathZone.TimeToChange = msg.Time;
				view.Dispatcher.Invoke(() => { model.OnChangeGameObject(deathZone); });
			}
		}

		private void Handler_EndGame(IMessage msg)
		{
			view.Dispatcher.Invoke(() =>
			{
				client.EventEndSession -= this.Client_EventEndSession;
				client.EventNewMessage -= this.Client_EventNewMessage;
				Account formAccount = new Account(client, msg);
				formAccount.Show();
				view.Transition = true;
				view.Close();
			});
		}

		private void Handler_DeletedInMap(IMessage deleteInMap)
		{
			IModelObject modelObject;
			if (model.GameObjects.TryRemove(deleteInMap.ID, out modelObject))
				view.Dispatcher.Invoke(() => { model.OnChangeGameObject(modelObject, StateObject.Delete); });
			if (modelObject.Type == TypesGameObject.Grenade)
			{
				Explosion traser = new Explosion(ulong.MaxValue, modelObject.Location);
				view.Dispatcher.Invoke(() => { model.OnChangeGameObject(traser); });
			}
		}

		private void Handler_HealthyCharacter(IMessage changedValueHP)
		{
			model.Chararcter.HP = changedValueHP.HP;
			view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
		}

		private void Handler_ObjectMoved(IMessage moved)
		{
			IModelObject modelObject;
			if (!model.GameObjects.TryGetValue(moved.ID, out modelObject))
				return;

			modelObject.Update(moved.Location);

			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(modelObject); });

			if (modelObject.ID == model.Chararcter.ID)
			{
				view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
			}
		}

		private void Handler_RoomState(IMessage msg)
		{
			foreach (IMessage message in msg.InsertCollections[0])
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.GameObjectState:
						Handler_GameObjectState((GameObjectState)message);
						break;
					case TypesMessage.WeaponState:
						Handler_WeaponState((WeaponState)message);
						break;
				}
			}
			//обработано первое сообщение RoomState - значит комната загружена
			Loaded = true;
		}

		private void Handler_WeaponState(IMessage msg)
		{
			model.GameObjects.AddOrUpdate(msg.ID, AddWeapon(msg), (k, v) => Updateweapon(v, msg));
		}

		private GameObject AddWeapon(IMessage msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Weapon weapon = new Weapon(msg.ID, msg.TypeWeapon);
			foreach (IMessage message in msg.InsertCollections[0])
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.BodyState:
						Handler_BodyState(weapon, message as BodyState);
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(weapon); });
			return weapon;
		}

		private void Handler_GameObjectState(IMessage msg)
		{
			switch (msg.TypeGameObject)
			{
				case TypesGameObject.Player:
					model.GameObjects.AddOrUpdate(msg.ID,AddGamer(msg), (k,v) => UpdateGameObject(v, msg));
					break;
				case TypesGameObject.Box:
					model.GameObjects.AddOrUpdate(msg.ID, AddUniversal(msg), (k, v) => UpdateGameObject(v, msg));
					break;
				case TypesGameObject.Stone:
					model.GameObjects.AddOrUpdate(msg.ID, AddUniversal(msg), (k, v) => UpdateGameObject(v, msg));
					break;			
				case TypesGameObject.Field:
					model.GameObjects.AddOrUpdate(msg.ID, AddField(msg), (k, v) => UpdateGameObject(v, msg));
					break;
				case TypesGameObject.DeathZone:
					model.GameObjects.AddOrUpdate(msg.ID, AddZone(msg), (k, v) => UpdateGameObject(v, msg));
					break;
				case TypesGameObject.Bush:
					model.GameObjects.AddOrUpdate(msg.ID, AddUniversal(msg), (k, v) => UpdateGameObject(v, msg));
					break;
				case TypesGameObject.Tree:
					model.GameObjects.AddOrUpdate(msg.ID, AddUniversal(msg), (k, v) => UpdateGameObject(v, msg));
					break;
				case TypesGameObject.Grenade:
					model.GameObjects.AddOrUpdate(msg.ID, AddUniversal(msg), (k, v) => UpdateGameObject(v, msg));
					break;
			}
		}

		private GameObject AddUniversal(IMessage msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			GameObject gameObject = null;

			switch (msg.TypeGameObject)
			{
				case TypesGameObject.Box:
					gameObject = new Box(msg.ID);
					break;
				case TypesGameObject.Bush:
					gameObject = new Bush(msg.ID);
					break;
				case TypesGameObject.Tree:
					gameObject = new Tree(msg.ID);
					break;
				case TypesGameObject.Stone:
					gameObject = new Stone(msg.ID);
					break;
				case TypesGameObject.Grenade:
					gameObject = new Grenade(msg.ID);
					break;
			}

			foreach (IMessage message in msg.InsertCollections[0])
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.BodyState:
						Handler_BodyState(gameObject, message as BodyState);
						break;
				}
			}

			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gameObject); });
			return gameObject;
		}

		private GameObject AddZone(IMessage msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			DeathZone deathZone = new DeathZone(msg.ID);
			model.DeathZone = deathZone;

			foreach (IMessage message in msg.InsertCollections[0])
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.BodyZoneState:
						Handler_BodyZoneState(deathZone, (BodyZoneState)message);
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(deathZone); });
			return deathZone;
		}

		private void Handler_BodyZoneState(DeathZone deathZone, IMessage msg)
		{
			deathZone.Shape = new RectangleF(msg.Location, new SizeF(msg.Radius * 2, msg.Radius * 2));

		}

		private GameObject AddField(IMessage msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Field field = new Field(msg.ID);
			foreach (IMessage message in msg.InsertCollections[0])
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.FieldState:
						Handler_FieldState(field, (FieldState)message);						
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(field); });
			return field;
		}

		private void Handler_FieldState(Field field, FieldState msg)
		{
			field.Shape = new RectangleF(msg.Size.Width / 2, msg.Size.Height / 2,
							msg.Size.Width,msg.Size.Height);
		}

		
		private GameObject AddGamer(IMessage msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Gamer gamer = new Gamer(msg.ID);

			foreach (IMessage message in msg.InsertCollections[0])
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.BodyState:
						Handler_BodyState(gamer, message);
						break;
					case TypesMessage.CurrentWeaponState:
						Handler_CurrentWeaponState(gamer, message);
						break;
				}
			}

			if (msg.ID == model.Chararcter.ID)
			{
				model.Chararcter.Create(gamer);
				view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gamer); });
			return gamer;
		}

		private void Handler_BodyState(IModelObject gameObject, IMessage msg)
		{
			gameObject.Update(msg.Shape);
		}

		private IModelObject UpdateGameObject(IModelObject gameObject, IMessage newData)
		{
			foreach (IMessage message in newData.InsertCollections[0])
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.BodyState:
						Handler_BodyState(gameObject, message as BodyState);
						break;
					case TypesMessage.BodyZoneState:
						Handler_BodyZoneState(gameObject as DeathZone, message as BodyZoneState);
						break;
					case TypesMessage.CurrentWeaponState:
						Handler_CurrentWeaponState(gameObject as Gamer, message as CurrentWeaponState);
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gameObject); });
			return gameObject;
		}

		private IModelObject Updateweapon(IModelObject gameObject, IMessage newData)
		{
			foreach (IMessage message in newData.InsertCollections[0])
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.BodyState:
						Handler_BodyState(gameObject, message as BodyState);
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gameObject); });
			return gameObject;
		}

		private void Handler_CurrentWeaponState(Gamer gamer, IMessage msg)
		{
			gamer.CurrentWeapon = msg.TypeWeapon;
		}

		private void Client_EventEndSession()
		{
			view.Dispatcher.Invoke(() =>
			{
				client = null;
				Autorization authorization = new Autorization();
				authorization.Show();
				view.Transition = true;
				view.Close();
			});
		}
	}
}
