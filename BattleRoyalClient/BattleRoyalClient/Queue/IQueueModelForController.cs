using CommonLibrary;
namespace BattleRoyalClient
{
	interface IQueueModelForController
	{
		void Update(IMessage msg);
		void HappenedLossConnectToServer();
		void ClearModel();
	}
}