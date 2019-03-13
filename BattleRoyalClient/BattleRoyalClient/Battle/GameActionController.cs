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
					model.GameObjects.AddOrUpdate(msg.ID,AddGamer(msg), (k,v) => UpdateGamer(v, msg));
					break;
			}
		}
		private IGameObject AddGamer(GameObjectState msg)
		{
			Gamer gamer = new Gamer();
			foreach (IMessage message in msg.StatesComponents)
			{
				switch (message.TypeMessage)
				{
					case TypesProgramMessage.Location:
						gamer.Location = ConvertPosition.ConvertToViewLocation((message as Location).LocationBody);
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
					case TypesProgramMessage.Location:
						gamer.Location = ConvertPosition.ConvertToViewLocation((message as Location).LocationBody);
						break;
				}
			}
			return gamer;
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
