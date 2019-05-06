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
using System.Threading;

namespace BattleRoyalClient
{
	class GameActionController
	{
		private IBattleModelForControler model;
		private Task handlerMessages;
		private bool working = true;

		public IBattleModelForView Model {
			get
			{
				return (IBattleModelForView)model;
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
			handlerMessages = new Task(MethodForHandlerMessages);
			handlerMessages.Start();
		}

		private void MethodForHandlerMessages()
		{
			while (working)
			{
				
				if (client.GetCountReceivedMsg() == 0)
				{
					Thread.Sleep(1);
					continue;
				}
				Client_EventNewMessage();
			}
		}

		private void Client_EventNewMessage()
		{
			IMessage msg = client.GetRecievedMsg();
			switch (msg.TypeMessage)
			{
				case TypesMessage.RoomState:
					Handler_RoomState(msg);
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
			model.ChangeCountPlayersInGame(msg.Count);
			model.CreateChangeModel( TypesChange.CountPlyers);
		}

		private void Handler_AddWeapon(IMessage msg)
		{
			model.CharacterController.AddWeapon(msg.TypeWeapon);
		}

		private void Handler_MakedShot(IMessage msg)
		{		
			model.CreateTraser(msg.ID, msg.Distance, msg.Angle);
		}

		public void Handler_PlayerTurned(IMessage msg)
		{
			model.TurnedGameObject(msg.ID, msg.Angle);
			model.OnChangeGameObject(msg.ID);
		}

		private void Handler_ChangedCurrentWeapon(IMessage msg)
		{
			model.ChangeCurrentWeaponAtGamer(msg.ID, msg.TypeWeapon);
			if (msg.ID == model.CharacterController.ID)
				model.CharacterController.ChangeCurrentWeapon(msg.TypeWeapon);

			model.OnChangeGameObject(msg.ID);
		}

		private void Handler_ChangedTimeTillReduction(IMessage msg)
		{
			model.ChangeTimeTillReductionAtDeathZone(msg.Time);
			model.OnChangeGameObject(msg.ID);
		}

		private void Handler_EndGame(IMessage msg)
		{
			working = false;
			client.EventEndSession -= this.Client_EventEndSession;
			view.Dispatcher.Invoke(() =>
			{
				Account formAccount = new Account(client, msg);
				formAccount.Show();
				view.Transition = true;
				view.Close();
			});
		}

		private void Handler_DeletedInMap(IMessage deleteInMap)
		{

			IModelObject modelObject = model.RemoveObject(deleteInMap.ID);
			
			if (modelObject.Type == TypesGameObject.Grenade)
			{
				model.CreateExplosion(modelObject.Location);
			}

			model.OnChangeGameObject(modelObject, StateObject.Delete);
		}

		private void Handler_HealthyCharacter(IMessage changedValueHP)
		{
			model.CharacterController.ChangeHP(changedValueHP.HP);
		}

		private void Handler_ObjectMoved(IMessage moved)
		{
			model.ChangeLocationAtObject(moved.ID, moved.Location);
			model.OnChangeGameObject(moved.ID);
			if(moved.ID == model.CharacterController.ID)
			{
				model.CharacterController.ChangeLocation(moved.Location);
			}
		}

		private void Handler_RoomState(IMessage msg)
		{
			foreach (IMessage message in msg.InsertCollections)
			{
				switch (message.TypeMessage)
				{
					case TypesMessage.GameObjectState:
					case TypesMessage.WeaponState:
						Handler_GameObjectState(message);
						break;				
				}
			}
			//обработано первое сообщение RoomState - значит комната загружена
			Loaded = true;
		}

		private void Handler_GameObjectState(IMessage msg)
		{
			if (!model.ContainsObject(msg.ID))
			{
				GameObject addingObject = FactoryForCreateGameObjects.CreateGameObjects(msg);
				model.AddGameObject(addingObject);
				model.OnChangeGameObject(addingObject);
				if (msg.ID == model.CharacterController.ID)
				{
					model.CharacterController.Create(addingObject as Gamer);
				}
			}
			else //обновляем
			{
				foreach (IMessage message in msg.InsertCollections)
				{
					switch (message.TypeMessage)
					{
						case TypesMessage.BodyState:
							model.ChanageShapeAtObject(msg.ID, message.Shape);
							break;
						case TypesMessage.BodyZoneState:
							model.ChanageShapeAtZoneDeath(message.Location, message.Radius);
							break;					
						case TypesMessage.CurrentWeaponState:
							model.ChangeCurrentWeaponAtGamer(msg.ID, message.TypeWeapon);
							break;
					}
				}
				model.OnChangeGameObject(msg.ID);
			}
		}

		private void StopHandlerMessages()
		{
			working = false;
			handlerMessages.Wait();
			handlerMessages.Dispose();
		}

		private void Client_EventEndSession()
		{
			StopHandlerMessages();

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
