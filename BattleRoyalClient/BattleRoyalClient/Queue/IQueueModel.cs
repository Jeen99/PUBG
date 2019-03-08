namespace BattleRoyalClient
{
	interface IQueueModel
	{
		int PlaysersInQueue { get; }
		event ChangeModel QueueModelChange;
	}
}