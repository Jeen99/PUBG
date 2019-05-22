namespace BattleRoyalClient
{
	interface IQueueModelForView
	{
		StatesQueueModel State { get; }
		ulong IDInBattle { get; }
		int PlayersInQueue { get; }
		event ChangeQueueModel QueueModelChange;
	}
}