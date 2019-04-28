using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CommonLibrary;
using CommonLibrary.GameMessages;
using CommonLibrary.CommonElements;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using Timer = System.Timers.Timer;

namespace BattleRoayleServer
{
    public class RoomNetwork:INetwork
    {
		private object AccessSinchClients = new object();
		private bool roomClosing = false;
		private Task SenderMessage;
        /// <summary>
        /// Ссылка на игровую логику
        /// </summary>
        private IRoomLogic roomLogic;
		/// <summary>
		/// Список игроков подлюченных к данной комнате
		/// </summary>
		public Dictionary<ulong, INetworkClient> Clients { get; private set; }
		
		/// <summary>
		/// Вызывается для отправки окружения клиентов в данный момент времени
		/// </summary>
		private Timer timerTotalSinch;

        public RoomNetwork(IList<QueueGamer> gamers, IRoomLogic roomLogic)
        {
			//создаем список игроков на основе списка gamers и rooomLogic(ID), создаем таймер 
			Clients = new Dictionary<ulong, INetworkClient>();			
			this.roomLogic = roomLogic;
			CreateClients(gamers);
			timerTotalSinch = new Timer(3 * 1000)
			{
				SynchronizingObject = null,
				AutoReset = true
			};
			timerTotalSinch.Elapsed += HandlerTotalSinch;
			SenderMessage = new Task(MethodForSenderMessage);
			SenderMessage.Start();
		}
		/// <summary>
		/// Вызывается на каждое срабатывание таймера
		/// </summary>
		private void HandlerTotalSinch(object sender, ElapsedEventArgs e)
		{
			IMessage stateRoom = roomLogic.RoomModel.RoomState;
			lock (AccessSinchClients)
			{			
				foreach (var id in Clients.Keys)
				{
					//отсеиваем лишние данные и отправляем оставшееся
					Clients[id].Client.SendMessage(Filter_StateRoom(id, (RoomState)stateRoom));
				}
			}
		}

		private IMessage Filter_StateRoom(ulong ID, IMessage stateRoom)
		{
			List<IMessage> filterStates = new List<IMessage>();
			foreach (GameObjectState stateObject in stateRoom.InsertCollections)
			{
				if (stateObject.ID == ID)
				{
					filterStates.Add(stateObject);
				}
				else
				{
					//только данные визуальных компонентов
					List<IMessage> filterStatesComponents = new List<IMessage>();
					foreach (IMessage stateComponent in stateObject.InsertCollections)
					{
						switch (stateComponent.TypeMessage)
						{
							case TypesMessage.BodyState:
							case TypesMessage.CurrentWeaponState:
							case TypesMessage.FieldState:
							case TypesMessage.BodyZoneState:
								filterStatesComponents.Add(stateComponent);
								break;
						}
					}

					switch (stateObject.TypeMessage)
					{
						case  TypesMessage.WeaponState:
							var weaponState = (WeaponState)stateObject;
							filterStates.Add(new WeaponState(weaponState.ID, weaponState.TypeGameObject, weaponState.TypeWeapon, filterStatesComponents));
							break;
						default:
							filterStates.Add(new GameObjectState(stateObject.ID, stateObject.TypeGameObject, filterStatesComponents));
							break;
					}
					
				}
			}

			return new RoomState(filterStates);
		}

		/// <summary>
		/// Определяет тип сообщения и запускает соответствующий обработчик
		/// </summary>
		private void Handler_GameEvent(IMessage msg)
		{		
			#region Выбор типа обработчика
			switch (msg.TypeMessage)
			{
				//сообщения, которые отправляются только игроку создавшему это событие
				case TypesMessage.AddWeapon:
				case TypesMessage.ChangedValueHP:
				case TypesMessage.ReloadWeapon:
					Handler_PrivateMsg(msg);
					break;
				//сообщения которые отправляеются всем
				case TypesMessage.DeletedInMap:
				case TypesMessage.ChangedCurrentWeapon:
				case TypesMessage.GameObjectState:
				case TypesMessage.WeaponState:
				case TypesMessage.ChangedTimeTillReduction:
				case TypesMessage.ChangeCountPayersInGame:
					Handler_BroadcastMsg(msg);
					break;
				case TypesMessage.EndGame:
					Handler_EndGame(msg);
					break;
				case TypesMessage.PlayerTurn:
					Handler_PlayerTurn(msg);
					break;
					//все остальные события
				default:
					Handler_DefaulteMsg(msg);
					break;

			}
			#endregion
		}
		private void MethodForSenderMessage()
		{
			while (!roomClosing)
			{
				IMessage msg = roomLogic.RoomModel.GetOutgoingMessage();
				if (msg == null)
				{
					Thread.Sleep(1);
					continue;
				}
				Handler_GameEvent(msg);
			}
		}

		private void Handler_EndGame(IMessage msg)
		{
			lock (AccessSinchClients)
			{			
				if(Clients.ContainsKey(msg.ID))
					Clients[msg.ID].SaveStatistics(msg);
				Handler_PrivateMsg(msg);
				//закрываем этого клиента
			
				INetworkClient client = Clients[msg.ID];
				Clients.Remove(msg.ID);
				client.Dispose();			
			}			
		}

		private void Handler_BroadcastMsg(IMessage msg)
		{		
			foreach (var id in Clients.Keys)
			{
				Clients[id].Client.SendMessage(msg);
			}	
		}

		private void Handler_PrivateMsg(IMessage msg)
		{		
			if (Clients.ContainsKey(msg.ID))
			{
				Clients[msg.ID].Client.SendMessage((IMessage)msg);
			}	
		}

		private void Handler_DefaulteMsg(IMessage msg)
		{
			
			if (!Clients.ContainsKey(msg.ID))
			{
				if (msg is ObjectMoved)
				{
					var message = (ObjectMoved)msg;
					foreach (var id in Clients.Keys)
					{
						//если область видимости одного игрока находит на другого отправляем ему сообщение
						if (Clients[id].VisibleArea.Contains(message.Location.X, message.Location.Y))
						{
							Clients[id].Client.SendMessage((IMessage)msg);
						}
					}
				}
			}
			else
			{
				RectangleF area = Clients[msg.ID].VisibleArea;
				foreach (var id in Clients.Keys)
				{
					//если область видимости одного игрока находит на другого отправляем ему сообщение
					if (area.IntersectsWith(Clients[id].VisibleArea))
					{
						Clients[id].Client.SendMessage(msg);
					}
				}
			}		
		}

		public void Start()
        {
			//запускаем таймер
			//timerTotalSinch.Start();
		}

        public void Dispose()
        {
			lock (AccessSinchClients)
			{
				roomClosing = true;
				timerTotalSinch.Elapsed -= HandlerTotalSinch;
				timerTotalSinch.Dispose();
				SenderMessage.Wait();
				SenderMessage.Dispose();
				while (true)
				{
					IMessage msg = roomLogic.RoomModel.GetOutgoingMessage();
					if (msg == null) break;
					Handler_GameEvent(msg);
				}

				foreach (var id in Clients.Keys)
				{
					Clients[id].Dispose();
				}
			}
        }

		/// <summary>
		/// Создает список клиентов
		/// </summary>
		private void CreateClients(IList<QueueGamer> gamers)
		{
			lock (AccessSinchClients)
			{
				for (int i = 0; i < roomLogic.RoomModel.Players.Count; i++)
				{
					var client = new NetworkClient(roomLogic.RoomModel, i, gamers[i].Client,
					   gamers[i].NickName, gamers[i].Password);
					client.Event_GamerIsLoaded += HandlerEvent_GamerIsLoaded;
					client.EventNetorkClientDisconnect += Client_EventNetorkClientDisconnect;
					Clients.Add(client.Player.ID, client);
				}
			}
		}

		
		private void Handler_PlayerTurn(IMessage msg)
		{
			if (!Clients.ContainsKey(msg.ID)) return;

			RectangleF area = Clients[msg.ID].VisibleArea;
			foreach (var id in Clients.Keys)
			{
				//если область видимости одного игрока находит на другого отправляем ему сообщение
				if (msg.ID != id)
				{
					if (area.IntersectsWith(Clients[id].VisibleArea))
					{
						Clients[id].Client.SendMessage(msg);
					}
				}
			}
		}

		private void Client_EventNetorkClientDisconnect(INetworkClient client)
		{
			lock (AccessSinchClients)
			{
				Clients.Remove(client.Player.ID);	
			}
		}

		/// <summary>
		/// Отправляет загрузившимуся клиенту данные обо всех объектах на карте
		/// </summary>
		private void HandlerEvent_GamerIsLoaded(INetworkClient client)
		{
			IMessage msg = roomLogic.RoomModel.FullRoomState;
			client.Client.SendMessage(Filter_StateRoom(client.Player.ID, msg));
		}

		public void Stop()
		{
			timerTotalSinch.Stop();
		}
	}
}