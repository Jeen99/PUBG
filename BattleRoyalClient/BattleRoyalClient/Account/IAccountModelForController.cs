using System;
using CommonLibrary;

namespace BattleRoyalClient
{
	interface IAccountModelForController
	{
		long Battles { get; }
		long Deaths { get; }
		TimeSpan GameTime { get; }
		long Kills { get; }
		StatesAccountModel State { get; }

		void HappenedLossConnectToServer();
		void Update(IMessage msg);
		void ClearModel();
	}
}