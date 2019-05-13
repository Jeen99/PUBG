using CSInteraction.Client;
using CommonLibrary;

namespace BattleRoyalClient
{
	public interface IAuthorizationModelForController
	{
		string NickName { get; set; }
		string Password { get; set; }
		bool SaveAutorizationData { get; }
		StatesAutorizationModel State { get; }

		void HappenedLossConnectToServer();
		void Update(IMessage msg);
		void Load();
		void SaveAndClearModel();
		void ChangeSaveAutorizationData();
	}
}