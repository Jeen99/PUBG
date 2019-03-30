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
			var gamer = model.GameObjects[moved.PlayerID];
			var shapeGamer = model.GameObjects[moved.PlayerID].Shape;
			shapeGamer.X = moved.NewLocation.X;
			shapeGamer.Y = moved.NewLocation.Y;

			gamer.Shape = shapeGamer;

			if (moved.PlayerID == model.Chararcter.ID)
				model.Chararcter.CharacterChange();

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
					model.GameObjects.AddOrUpdate(msg.ID,AddGamer(msg), (k,v) => UpdateGameObject(v, msg));
					if (msg.ID == model.Chararcter.ID)
					{
						model.Chararcter.CharacterChange();
					}
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
			//stone.Model3D = new Model3D(view.models, stone);
			CreateModel3d(stone);
			return stone;
		}

		private GameObject AddGamer(GameObjectState msg)
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
			//gamer.Model3D = new Model3D(view.models, gamer);
			CreateModel3d(gamer);
			return gamer;
		}

		private GameObject AddBox(GameObjectState msg)
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
			CreateModel3d(box);
			//box.Model3D = new Model3D(view.models, box);
			return box;
		}
		private GameObject UpdateGameObject(GameObject gameObject, GameObjectState newData)
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
			UpdateModel3d(gameObject);
			return gameObject;
		}

		private void CreateModel3d(GameObject gameObject)
		{
			//перенести обработку в View
			view.Dispatcher.Invoke(() =>
			{
				gameObject.Model3D = new Model3D(view.models, gameObject);
			});
		}

		private void UpdateModel3d(GameObject gameObject)
		{
			view.Dispatcher.Invoke(() =>
			{
				gameObject.Model3D.UpdatePosition();
			});
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
