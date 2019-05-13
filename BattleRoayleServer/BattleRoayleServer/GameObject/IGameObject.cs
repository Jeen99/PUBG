using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;

namespace BattleRoayleServer
{
	public interface IGameObject
	{
		DictionaryComponent Components { get; }
		IGameObject Parent { get; set; }
		bool Destroyed { get; }
		ulong ID { get; }
		IModelForComponents Model { get; }
		IMessage State { get; }
		TypesGameObject Type { get; }
		TypesBehaveObjects TypeBehave { get; }

		#region Cобытия для уведомления клиентов о получении нового сообытия
		event ReceivedMessage Received_ChoiceWeapon;
		event ReceivedMessage Received_GamerDied;
		event ReceivedMessage Received_GotDamage;
		event ReceivedMessage Received_GoTo;
		event ReceivedMessage Received_MakeShot;
		event ReceivedMessage Received_PlayerTurn;
		event ReceivedMessage Received_MakeReloadWeapon;
		event ReceivedMessage Received_TryPickUp;
		event ReceivedMessage Received_DeletedInMap;
		event ReceivedMessage Received_TimeQuantPassed;
		event ReceivedMessage Received_AddWeapon;
		event ReceivedMessage Received_MakedKill;
		#endregion

		void Setup();
		void Dispose();
		void SetDestroyed();
		void Update(IMessage msg);
	}
}