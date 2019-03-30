using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CSInteraction.ProgramMessage;
using System.Collections.Specialized;

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
		public IList<INetworkClient> Clients { get; private set; }
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
				foreach (INetworkClient client in Clients)
				{
					client.Client.SendMessage(stateRoom);
				}
			}
		}

		/// <summary>
		/// Определяет тип сообщения и запускает соответствующий обработчик
		/// </summary>
		private void HandlerGameEvent(object sender, NotifyCollectionChangedEventArgs e)
		{
			//пока отправляем всем клиентам
			IMessage message = roomLogic.HappenedEvents.Dequeue();
			foreach (INetworkClient client in Clients)
			{
				client.Client.SendMessage(message);
			}
		}

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
				foreach (var player in Clients)
				{
					player.Dispose();
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
				for (int i = 0; i < roomLogic.Players.Count; i++)
				{
					var client = new NetworkClient(roomLogic.Players[i], gamers[i].Client,
					   gamers[i].NickName, gamers[i].Password);
					client.Event_GamerIsLoaded += HandlerEvent_GamerIsLoaded;
					client.EventNetworkClientEndWork += Client_EventNetworkClientEndWork;
					client.EventNetorkClientDisconnect += Client_EventNetorkClientDisconnect;
					Clients.Add(client);
				}
			}
		}

		private void Client_EventNetorkClientDisconnect(INetworkClient client)
		{
			lock (AccessSinchClients)
			{
				Clients.Remove(client);			
			}
			roomLogic.RemovePlayer(client.Player);
		}

		private void Client_EventNetworkClientEndWork(INetworkClient networkClient)
		{
			lock (AccessSinchClients)
			{
				Clients.Remove(networkClient);
			}
			networkClient.Dispose();
		}

		/// <summary>
		/// Отправляет загрузившимуся клиенту данные обо всех объектах на карте
		/// </summary>
		private void HandlerEvent_GamerIsLoaded(INetworkClient client)
		{
			client.Client.SendMessage(roomLogic.GetInitializeData());
		}

		public void Stop()
		{
			timerTotalSinch.Stop();
		}
	}
}