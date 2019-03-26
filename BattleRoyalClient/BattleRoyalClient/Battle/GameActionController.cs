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
		private Battle view;
		public GameActionController(ulong id, BaseClient client, string nickName, string password, Battle view)
		{
			this.client = client;
			model = new BattleModel(id, nickName, password);
			this.view = view;
			client.EventEndSession += Client_EventEndSession;
			client.EventNewMessage += Client_EventNewMessage;
		}

		private void Client_EventNewMessage()
		{
			IMessage msg = client.ReceivedMsg.Dequeue();
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.RoomState:
					Handler_RoomState((RoomState)msg);
					view.Dispatcher.Invoke(()=> { model.CreateChangeModel(); });			
					break;
				case TypesProgramMessage.PlayerMoved:
					Handler_PlayerMoved(msg as PlayerMoved);
					break;
			}
		}

		private void Handler_PlayerMoved(PlayerMoved moved)
		{
			model.GameObjects[moved.PlayerID].Location = moved.NewLocation;
			if (moved.PlayerID == model.Chararcter.ID) model.Chararcter.CharacterChange();
			view.Dispatcher.Invoke(() => { model.CreateChangeModel(); });
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
					model.GameObjects.AddOrUpdate(msg.ID,AddGamer(msg), (k,v) => UpdateGamer(v, msg));
					if (msg.ID == model.Chararcter.ID)
					{
						model.Chararcter.CharacterChange();
					}
					break;
				case TypesGameObject.Box:
					model.GameObjects.AddOrUpdate(msg.ID, AddBox(msg), (k, v) => UpdateBox(v, msg));
					break;
				case TypesGameObject.Stone:
					model.GameObjects.AddOrUpdate(msg.ID, AddStone(msg), (k, v) => UpdateStone(v, msg));
					break;
			}
		}

		private IGameObject AddStone(GameObjectState msg)
		{
			Stone stone = new Stone();
			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						stone.Shape = state.Shape;
						break;
				}
			}
			return stone;
		}
		private IGameObject UpdateStone(IGameObject gameObject, GameObjectState newData)
		{
			Stone stone = (Stone)gameObject;
			foreach (IMessage message in newData.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						stone.Shape = state.Shape;
						break;
				}
			}
			return stone;
		}

		private IGameObject AddGamer(GameObjectState msg)
		{
			Gamer gamer = new Gamer();
			foreach (IMessage message in msg.StatesComponents)
			{
				
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						gamer.Shape = state.Shape;
						break;
				}
			}
			return gamer;
		}
		private IGameObject UpdateGamer(IGameObject gameObject, GameObjectState newData)
		{
			Gamer gamer = (Gamer)gameObject;
			foreach (IMessage message in newData.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						gamer.Shape = state.Shape;
						break;
				}
			}
			return gamer;
		}

		private IGameObject AddBox(GameObjectState msg)
		{
			Box box = new Box();
			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						box.Shape = state.Shape;
						break;
				}
			}
			return box;
		}
		private IGameObject UpdateBox(IGameObject gameObject, GameObjectState newData)
		{
			Box box = (Box)gameObject;
			foreach (IMessage message in newData.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.BodyState:
						BodyState state = (message as BodyState);
						box.Shape = state.Shape;
						break;
				}
			}
			return box;
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
