using System;
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
        public IGameModel RoomModel { get; private set; }
        private Timer timerNewIteration;
		private QuantTimer quantTimer;

		public event RoomLogicEndWork EventRoomLogicEndWork;

		public RoyalRoomLogic(int GamersInRoom)
        {
			RoomModel = new RoyalGameModel(GamersInRoom);
			RoomModel.Event_HappenedEndGame += RoomModel_Event_HappenedEndGame;
			timerNewIteration = new Timer(16)
			{
				SynchronizingObject = null,
				AutoReset = true
			};
			timerNewIteration.Elapsed += TickQuantTimer;
			quantTimer = new QuantTimer();
			
        }

		private void RoomModel_Event_HappenedEndGame()
		{
			timerNewIteration.Elapsed -= TickQuantTimer;
			timerNewIteration.Stop();
			EventRoomLogicEndWork?.Invoke(this);
		}

		//возврашает состояние всех объектов (активных и неактивных)
		public IMessage GetInitializeData()
		{
			List<IMessage> states = new List<IMessage>();
			foreach (var gameObject in RoomModel.GameObjects)
			{		
					IMessage msg = gameObject.Value.State;
					if (msg != null) states.Add(msg);	
			}

			states.Add(RoomModel.State);
			
			return new RoomState(states);
		}

        //вызывается при срабатывании таймера
        private void TickQuantTimer(object sender, ElapsedEventArgs e)
        {
			quantTimer.Tick();
			RoomModel.MakeStep(quantTimer.QuantValue);
		}
		
        public void Start()
        {
			timerNewIteration.Start();
        }

        public void Dispose()
        {
			timerNewIteration.Dispose();
			//осовобождение ресурсво модели
			RoomModel.Dispose();
		}

	}
}
