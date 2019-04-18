using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
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
		private BaseClient client;
		private BattleView3d view;

		public GameActionController(ulong id, BaseClient client, BattleView3d view)
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
			System.Diagnostics.Debug.WriteLine(msg.TypeMessage);
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.RoomState:
					Handler_RoomState(msg as RoomState);
					view.Dispatcher.Invoke(()=> { model.CreateChangeModel(); });			
					break;
				case TypesProgramMessage.ObjectMoved:
					Handler_ObjectMoved(msg as ObjectMoved);
					break;
				case TypesProgramMessage.ChangedValueHP:
					Handler_HealthyCharacter(msg as ChangedValueHP);
					break;
				case TypesProgramMessage.DeleteInMap:
					Handler_DeleteInMap(msg as DeleteInMap);
					break;
				case TypesProgramMessage.EndGame:
					Handler_EndGame(msg as EndGame);
					break;
				case TypesProgramMessage.GameObjectState:
					Handler_GameObjectState(msg as GameObjectState);
					break;
				case TypesProgramMessage.WeaponState:
					Handler_WeaponState(msg as WeaponState);
					break;
				case TypesProgramMessage.ChangedTimeTillReduction:
					Handler_ChangedTimeTillReduction((ChangedTimeTillReduction) msg);
					break;
				case TypesProgramMessage.ChangedCurrentWeapon:
					Handler_ChangedCurrentWeapon((ChangedCurrentWeapon) msg);
					break;
				case TypesProgramMessage.PlayerTurned:
					Handler_PlayerTurned((PlayerTurned) msg);
					break;
				case TypesProgramMessage.MakedShot:
					Handler_MakedShot((MakedShot) msg);
					break;
				case TypesProgramMessage.AddWeapon:
					Handler_AddWeapon((AddWeapon) msg);
					break;
			}
		}

		private void Handler_AddWeapon(AddWeapon msg)
		{
			model.Chararcter.AddWeapon(msg.TypeWeapon);
			view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
		}

		private void Handler_MakedShot(MakedShot msg)
		{
			var gunMan = model.GameObjects[msg.ID];
			Traser traser = new Traser(ulong.MaxValue, gunMan.Location, 
				new SizeF(msg.Distance*2, msg.Distance*2), msg.Angle);
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(traser); });
		}

		private void Handler_PlayerTurned(PlayerTurned msg)
		{
			if (!model.GameObjects.ContainsKey(msg.ID)) return;
			model.GameObjects[msg.ID].Update(msg.Angle);
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(model.GameObjects[msg.ID]); });
		}

		private void Handler_ChangedCurrentWeapon(ChangedCurrentWeapon msg)
		{
			if (!model.GameObjects.ContainsKey(msg.ID)) return;
			var gamer = (model.GameObjects[msg.ID] as Gamer);
			gamer.CurrentWeapon = msg.NewCurrentWeapon;
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gamer); });
			if(msg.ID == model.Chararcter.ID)
				view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
		}

		private void Handler_ChangedTimeTillReduction(ChangedTimeTillReduction msg)
		{
			if (!model.GameObjects.ContainsKey(msg.ID)) return;
			var deathZone = (model.GameObjects[msg.ID] as DeathZone);
			deathZone.TimeToChange = msg.NewTime;
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(deathZone); });
		}

		private void Handler_EndGame(EndGame msg)
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

		private void Handler_DeleteInMap(DeleteInMap deleteInMap)
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

		private void Handler_HealthyCharacter(ChangedValueHP changedValueHP)
		{
			model.Chararcter.HP = changedValueHP.NewValueHP;
			view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
		}

		private void Handler_ObjectMoved(ObjectMoved moved)
		{
			IModelObject modelObject;
			if (!model.GameObjects.TryGetValue(moved.ID, out modelObject))
				return;

			modelObject.Update(moved.NewLocation);

			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(modelObject); });

			if (modelObject.ID == model.Chararcter.ID)
			{
				view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeCharacter(); });
			}
		}

		private void Handler_RoomState(RoomState msg)
		{
			foreach (IMessage message in msg.GameObjectsStates)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.GameObjectState:
						Handler_GameObjectState((GameObjectState)message);
						break;
					case TypesProgramMessage.WeaponState:
						Handler_WeaponState((WeaponState)message);
						break;
				}
			}
			//обработано первое сообщение RoomState - значит комната загружена
			Loaded = true;
		}

		private void Handler_WeaponState(WeaponState msg)
		{
			model.GameObjects.AddOrUpdate(msg.ID, AddWeapon(msg), (k, v) => Updateweapon(v, msg));
		}

		private GameObject AddWeapon(WeaponState msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Weapon weapon = new Weapon(msg.ID, msg.TypeWeapon);
			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						Handler_BodyState(weapon, message as BodyState);
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(weapon); });
			return weapon;
		}

		private void Handler_GameObjectState(GameObjectState msg)
		{
			switch (msg.Type)
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

		private GameObject AddUniversal(GameObjectState msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			GameObject gameObject = null;

			switch (msg.Type)
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

			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						Handler_BodyState(gameObject, message as BodyState);
						break;
				}
			}

			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gameObject); });
			return gameObject;
		}

		private GameObject AddZone(GameObjectState msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			DeathZone deathZone = new DeathZone(msg.ID);
			model.DeathZone = deathZone;

			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyZoneState:
						Handler_BodyZoneState(deathZone, (BodyZoneState)message);
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(deathZone); });
			return deathZone;
		}

		private void Handler_BodyZoneState(DeathZone deathZone, BodyZoneState msg)
		{
			deathZone.Shape = new RectangleF(msg.Location, new SizeF(msg.Radius * 2, msg.Radius * 2));

		}

		private GameObject AddField(GameObjectState msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Field field = new Field(msg.ID);
			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.FieldState:
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

		
		private GameObject AddGamer(GameObjectState msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Gamer gamer = new Gamer(msg.ID);

			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						Handler_BodyState(gamer, message as BodyState);
						break;
					case TypesProgramMessage.CurrentWeaponState:
						Handler_CurrentWeaponState(gamer, (CurrentWeaponState)message);
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

		private void Handler_BodyState(IModelObject gameObject, BodyState msg)
		{
			gameObject.Update(msg.Shape);
		}

		private IModelObject UpdateGameObject(IModelObject gameObject, GameObjectState newData)
		{
			foreach (IMessage message in newData.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						Handler_BodyState(gameObject, message as BodyState);
						break;
					case TypesProgramMessage.BodyZoneState:
						Handler_BodyZoneState(gameObject as DeathZone, message as BodyZoneState);
						break;
					case TypesProgramMessage.CurrentWeaponState:
						Handler_CurrentWeaponState(gameObject as Gamer, message as CurrentWeaponState);
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gameObject); });
			return gameObject;
		}

		private IModelObject Updateweapon(IModelObject gameObject, WeaponState newData)
		{
			foreach (IMessage message in newData.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						Handler_BodyState(gameObject, message as BodyState);
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gameObject); });
			return gameObject;
		}

		private void Handler_CurrentWeaponState(Gamer gamer, CurrentWeaponState msg)
		{
			gamer.CurrentWeapon = msg.TypeCurrentWeapon;
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
