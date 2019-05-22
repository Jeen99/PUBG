using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.QueueMessages;

namespace BattleRoyalClient
{
	class QueueModel : IQueueModelForView, IQueueModelForController
	{
		public int PlayersInQueue { get; private set; } = 0;
		public ulong IDInBattle { get; private set; } = ulong.MinValue;
		public StatesQueueModel State { get; private set; } = StatesQueueModel.Initiliazed;

		public event ChangeQueueModel QueueModelChange;

		public QueueModel()
		{
			
		}

		public void Update(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesMessage.ChangeCountPlayersInQueue:
					Handler_ChangeCountPlayersInQueue(msg);
					break;
				case TypesMessage.ResultRequestExit:
					Handler_ResultRequestExit(msg);
					break;
				case TypesMessage.AddInBattle:
					Handler_AddInBattle(msg);
					break;
			}
		}

		private void Handler_ChangeCountPlayersInQueue(IMessage msg)
		{
			PlayersInQueue = msg.Count;
			CreateChangeModel( TypesChangeQueueModel.CountPlayersInQueue);
		}

		private void Handler_ResultRequestExit(IMessage msg)
		{
			if (msg.Result)
			{
				State = StatesQueueModel.ExitedOfQueue;
				CreateChangeModel( TypesChangeQueueModel.State);
			}
		}

		private void Handler_AddInBattle(IMessage msg)
		{
			State = StatesQueueModel.SuccessJoinedToBattle;
			IDInBattle = msg.ID;
			CreateChangeModel( TypesChangeQueueModel.State);
		}

		public void CreateChangeModel(TypesChangeQueueModel type)
		{
			QueueModelChange?.Invoke(type);
		}

		public void HappenedLossConnectToServer()
		{
			State = StatesQueueModel.ErrorConnect;
			CreateChangeModel( TypesChangeQueueModel.State);
		}

		public void ClearModel()
		{
			PlayersInQueue = 0;
			IDInBattle = ulong.MinValue;
			State = StatesQueueModel.Initiliazed;
			QueueModelChange = null;
		}
	}

	public delegate void ChangeQueueModel(TypesChangeQueueModel type);

	public enum TypesChangeQueueModel
	{
		CountPlayersInQueue,
		State
	}

	public enum StatesQueueModel
	{
		Initiliazed,
		ErrorConnect,
		SuccessJoinedToBattle,
		ExitedOfQueue
	}
}
