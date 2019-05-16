using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.AccountMessages;

namespace BattleRoyalClient
{
	class AccountModel : IAccountModelForView, IAccountModelForController
	{
		public long Kills { get; set; } = 0;
		public long Deaths { get; set; } = 0;
		public long Battles { get; set; } = 0;

		public TimeSpan GameTime { get; set; } = TimeSpan.Zero;

		public event ChangeAccountModel AccountModelChange;

		public StatesAccountModel State { get; private set; } = StatesAccountModel.NoInitialize;
		
		public void CreateChangeModel(TypesChangeAccountModel type)
		{
			AccountModelChange?.Invoke(type);
		}

		public AccountModel()
		{

		}

		public void ClearModel()
		{
			Kills = 0;
			Deaths = 0;
			Battles = 0;
			GameTime = TimeSpan.Zero;
			AccountModelChange = null;
			State = StatesAccountModel.NoInitialize;
		}

		public void Update(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesMessage.InitializeAccount:
					Handler_DataAccount((DataAccount)msg);
					break;
				case TypesMessage.RequestJoinToQueue:
					Handler_JoinedToQueue();
					break;
			}
		}

		private void Handler_DataAccount(DataAccount msg)
		{
			Kills = msg.Kills;
			Deaths = msg.Deaths;
			Battles = msg.Battles;
			GameTime = msg.Time;
			State = StatesAccountModel.Initiliazed;

			CreateChangeModel(TypesChangeAccountModel.Data);
		}

		private void Handler_JoinedToQueue()
		{
			State = StatesAccountModel.SuccessJoinedToQueue;
			CreateChangeModel(TypesChangeAccountModel.State);
		}

		public void HappenedLossConnectToServer()
		{
			State =  StatesAccountModel.ErrorConnect;
			CreateChangeModel(TypesChangeAccountModel.State);
		}
	}

	public delegate void ChangeAccountModel(TypesChangeAccountModel type);

	public enum TypesChangeAccountModel
	{
		Data,
		State
	}

	public enum StatesAccountModel
	{
		NoInitialize,
		Initiliazed,
		ErrorConnect,
		SuccessJoinedToQueue
	}
}
