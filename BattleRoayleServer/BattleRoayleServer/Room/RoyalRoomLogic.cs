﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CommonLibrary;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;
using System.Diagnostics;
using CommonLibrary.GameMessages;
using ObservalableExtended;


namespace BattleRoayleServer
{
    public class RoyalRoomLogic : IRoomLogic
    {
        private IGameModel roomContext;
        private Timer timerNewIteration;
		private QuantTimer quantTimer;

		private const int minValueGamerInBattle = 0;

		public event RoomLogicEndWork EventRoomLogicEndWork;

		public RoyalRoomLogic(int GamersInRoom)
        {
			roomContext = new RoyalGameModel(GamersInRoom);
			timerNewIteration = new Timer(16)
			{
				SynchronizingObject = null,
				AutoReset = true
			};
			timerNewIteration.Elapsed += TickQuantTimer;
			quantTimer = new QuantTimer();
			
        }

		private void EndWork()
		{
			timerNewIteration.Elapsed -= TickQuantTimer;
			timerNewIteration.Stop();
			EventRoomLogicEndWork?.Invoke(this);
		}

		/// <summary>
		/// Создает список состяний игровых объектов (только активных)
		/// </summary>
		public IMessage RoomState
		{
			get
			{
				List<IMessage> states = new List<IMessage>();
				foreach (var gameObject in roomContext.GameObjects)
				{
					if (gameObject.Value.TypesBehave == TypesBehaveObjects.Active)
					{
						IMessage msg = gameObject.Value.State;
						if (msg != null) states.Add(msg);
					}
				}
				return new RoomState(states);
			}
		}

		//возврашает состояние всех объектов (активных и неактивных)
		public IMessage GetInitializeData()
		{
			List<IMessage> states = new List<IMessage>();
			foreach (var gameObject in roomContext.GameObjects)
			{		
					IMessage msg = gameObject.Value.State;
					if (msg != null) states.Add(msg);	
			}

			states.Add(roomContext.State);
			
			return new RoomState(states);
		}

		/// <summary>
		/// Посредник между коллецией игроков игровой модели и внешней средой
		/// </summary>
		public IList<IPlayer> Players {
			get
			{
				return roomContext.Players;
			}
		}
		/// <summary>
		/// Посредник между коллецией событий игровой модели и внешней средой
		/// </summary>
		public ObservableQueue<IMessage> HappenedEvents {
			get {
				return roomContext.HappenedEvents;
			}
		}

		public Dictionary<ulong, IGameObject> GameObjects
		{
			get
			{
				return roomContext.GameObjects;
			}
		}

		public void AddPlayer()
        {
            throw new NotImplementedException();
        }

        //вызывается при срабатывании таймера
        private void TickQuantTimer(object sender, ElapsedEventArgs e)
        {
			quantTimer.Tick();
			roomContext.MakeStep(quantTimer.QuantValue);

			Debug.WriteLine("Прошло времени" + quantTimer.QuantValue);
			if (roomContext.Players.Count <= minValueGamerInBattle)
			{
				EndWork();
			}
		}
		
        public void RemovePlayer(IPlayer player)
        {
			if (player is Gamer)
			{
				roomContext.RemovePlayer(player as Gamer);
				if (roomContext.Players.Count <= minValueGamerInBattle)
					EndWork();
			}
		}

        public void Start()
        {
			timerNewIteration.Start();
        }

        public void Dispose()
        {
			timerNewIteration.Dispose();
			//осовобождение ресурсво модели
			roomContext.Dispose();
		}

	}
}
