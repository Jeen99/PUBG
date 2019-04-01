using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

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
					Handler_RoomState((RoomState)msg);
					view.Dispatcher.Invoke(()=> { model.CreateChangeModel(); });			
					break;
				case TypesProgramMessage.PlayerMoved:
					Handler_PlayerMoved(msg as PlayerMoved);
					break;
				case TypesProgramMessage.HealthyState:
					Handler_HealthyCharacter(msg as HealthyState);
					break;
			}
		}

		private void Handler_HealthyCharacter(HealthyState changedValueHP)
		{
			var HP = model.Chararcter.HP;
			HP = changedValueHP.HP;
			model.Chararcter.OnChangeHP();
		}

		private void Handler_PlayerMoved(PlayerMoved moved)
		{
			IModelObject modelObject;
			if (!model.GameObjects.TryGetValue(moved.PlayerID, out modelObject))
				return;

			var gamer = modelObject as Gamer;
			gamer.Update(moved.NewLocation);

			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gamer); });

			if (gamer.ID == model.Chararcter.ID)
			{
				view.Dispatcher.Invoke(() => { model.Chararcter.OnChangePosition(); });
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
				}
			}
		}

		private void Handler_GameObjectState(GameObjectState msg)
		{
			switch (msg.Type)
			{
				case TypesGameObject.Player:
					model.GameObjects.AddOrUpdate(msg.ID,AddGamer(msg), (k,v) => UpdateGameObject(v, msg));
					break;
				case TypesGameObject.Box:
					model.GameObjects.AddOrUpdate(msg.ID, AddBox(msg), (k, v) => UpdateGameObject(v, msg));
					break;
				case TypesGameObject.Stone:
					model.GameObjects.AddOrUpdate(msg.ID, AddStone(msg), (k, v) => UpdateGameObject(v, msg));
					break;
			}
		}

		private GameObject AddStone(GameObjectState msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Stone stone = new Stone();
			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						stone.Shape = state.Shape;
						stone.ID = msg.ID;
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(stone); });
			return stone;
		}

		private GameObject AddGamer(GameObjectState msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Gamer gamer = new Gamer();

			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						gamer.Shape = state.Shape;
						gamer.ID = msg.ID;
						break;
				}
			}

			if (msg.ID == model.Chararcter.ID)
			{
				model.Chararcter.Create(gamer);
				view.Dispatcher.Invoke(() => { model.Chararcter.OnChangeHP(); });
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gamer); });
			return gamer;
		}

		private GameObject AddBox(GameObjectState msg)
		{
			if (model.GameObjects.Keys.Contains(msg.ID))
				return (GameObject)model.GameObjects[msg.ID];

			Box box = new Box();
			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						box.Shape = state.Shape;
						box.ID = msg.ID;
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(box); });
			return box;
		}
		private IModelObject UpdateGameObject(IModelObject gameObject, GameObjectState newData)
		{
			foreach (IMessage message in newData.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						gameObject.Shape = state.Shape;
						break;
				}
			}
			view.Dispatcher.Invoke(() => { model.OnChangeGameObject(gameObject); });
			return gameObject;
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
