using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CSInteraction.ProgramMessage;
using System.Collections.Specialized;
using System.Drawing;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
    public class RoomNetwork:INetwork
    {
		private object AccessSinchClients = new object();
        /// <summary>
        /// Ссылка на игровую логику
        /// </summary>
        private IRoomLogic roomLogic;
		/// <summary>
		/// Список игроков подлюченных к данной комнате
		/// </summary>
		//public Dictionary<ulong, INetworkClient> Clients { get; private set; }
		public List<INetworkClient> Clients { get; private set; }
		/// <summary>
		/// Вызывается для отправки окружения клиентов в данный момент времени
		/// </summary>
		private Timer timerTotalSinch;

        public RoomNetwork(IList<QueueGamer> gamers, IRoomLogic roomLogic)
        {
			//создаем список игроков на основе списка gamers и rooomLogic(ID), создаем таймер 
			Clients = new List<INetworkClient>();			
			this.roomLogic = roomLogic;
			CreateClients(gamers);
			roomLogic.HappenedEvents.CollectionChanged += HandlerGameEvent;
			timerTotalSinch = new Timer(3 * 1000)
			{
				SynchronizingObject = null,
				AutoReset = true
			};
			timerTotalSinch.Elapsed += HandlerTotalSinch;
			//отправляем данные для загрузки комнаты
		}
		/// <summary>
		/// Вызывается на каждое срабатывание таймера
		/// </summary>
		private void HandlerTotalSinch(object sender, ElapsedEventArgs e)
		{
			IMessage stateRoom = roomLogic.RoomState;
			lock (AccessSinchClients)
			{
				foreach (var item in Clients)
				{
					item.Client.SendMessage(stateRoom);
				}
				/*foreach (var id in Clients.Keys)
				{
					Clients[id].Client.SendMessage(stateRoom);
				}*/
			}
		}

		/// <summary>
		/// Определяет тип сообщения и запускает соответствующий обработчик
		/// </summary>
		private void HandlerGameEvent(object sender, NotifyCollectionChangedEventArgs e)
		{
			//возможно стоит отправлять в отдельном потоке
			IMessage message = roomLogic.HappenedEvents.Dequeue();
			foreach (var item in Clients)
			{
				item.Client.SendMessage(message);
			}
			/*foreach (var id in Clients.Keys)
			{
				Clients[id].Client.SendMessage(message);
			}*/
			/*Task.Run(() =>
			{
				switch (message.TypeMessage)
				{
					//сообщения, которые отправляются только игроку создавшему это событие
					case TypesProgramMessage.AddWeapon:
					case TypesProgramMessage.ChangedCurrentWeapon:
					case TypesProgramMessage.ChangedValueHP:
					case TypesProgramMessage.StartReloadWeapon:
					case TypesProgramMessage.EndRelaodWeapon:
						Handler_PrivateMsg((IOutgoing)message);
						break;
					//сообщения которые отправляеются всем
					case TypesProgramMessage.DeleteInMap:
					case TypesProgramMessage.GameObjectDestroy:
						Handler_BroadcastMsg(message);
						break;
					//все остальные события
					default:
						Handler_DefaulteMsg((IOutgoing)message);
						break;

				}
			});*/

		}

		/*private void Handler_BroadcastMsg(IMessage msg)
		{
			foreach (var id in Clients.Keys)
			{
				Clients[id].Client.SendMessage(msg);
			}
		}

		private void Handler_PrivateMsg(IOutgoing msg)
		{
			Clients[msg.ID].Client.SendMessage((IMessage)msg);
		}

		private void Handler_DefaulteMsg(IOutgoing msg)
		{
			RectangleF area = Clients[msg.ID].VisibleArea;
			foreach(var id in Clients.Keys)
			{
				//если область видимости одного игрока находит на другого отправляем ему сообщение
				if (area.IntersectsWith(Clients[id].VisibleArea))
				{
					Clients[msg.ID].Client.SendMessage((IMessage)msg);
				}
			}
		}*/

		

		public void Start()
        {
			//запускаем таймер
			timerTotalSinch.Start();
		}

        public void Dispose()
        {
			lock (AccessSinchClients)
			{
				timerTotalSinch.Dispose();
				foreach (var item in Clients)
				{
					item.Dispose();
				}
				/*foreach (var id in Clients.Keys)
				{
					Clients[id].Dispose();
				}*/
			}
        }

		/// <summary>
		/// Создает список клиентов
		/// </summary>
		private void CreateClients(IList<QueueGamer> gamers)
		{
			lock (AccessSinchClients)
			{
				for (int i = 0; i < roomLogic.Players.Count; i++)
				{
					var client = new NetworkClient(roomLogic.Players[i], gamers[i].Client,
					   gamers[i].NickName, gamers[i].Password);
					client.Event_GamerIsLoaded += HandlerEvent_GamerIsLoaded;
					client.EventNetworkClientEndWork += Client_EventNetworkClientEndWork;
					client.EventNetorkClientDisconnect += Client_EventNetorkClientDisconnect;
					//Clients.Add(client.Player.ID, client);
					Clients.Add(client);
				}
			}
		}

		private void Client_EventNetorkClientDisconnect(INetworkClient client)
		{
			lock (AccessSinchClients)
			{
				//Clients.Remove(client.Player.ID);
				Clients.Remove(client);
			}
			roomLogic.RemovePlayer(client.Player);
		}

		private void Client_EventNetworkClientEndWork(INetworkClient client)
		{
			lock (AccessSinchClients)
			{
				//Clients.Remove(сlient.Player.ID);
				Clients.Remove(client);
			}
			client.Dispose();
		}

		/// <summary>
		/// Отправляет загрузившимуся клиенту данные обо всех объектах на карте
		/// </summary>
		private void HandlerEvent_GamerIsLoaded(INetworkClient client)
		{
			var msg = roomLogic.GetInitializeData();
			client.Client.SendMessage(msg);
		}

		public void Stop()
		{
			timerTotalSinch.Stop();
		}
	}
}