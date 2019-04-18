using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public enum TypesProgramMessage
	{
		Authorization,
		ErrorAuhorization,
		SuccessAuthorization,
		DataAccount,
		JoinToQueue,
		JoinedToQueue,
		ChangeCountPlayersInQueue,
		DeleteOfQueue,
		FailedExitOfQueue,
		SuccessExitOfQueue,
		LoadedAccountForm,
		AddInBattle,
		LoadedQueueForm,
		GoTo,
		ObjectMoved,
		BodyState,
		GameObjectState,
		RoomState,
		LoadedBattleForm,
		DeleteInMap,
		TryPickUp,
		AddWeapon,
		StartReloadWeapon,
		EndRelaodWeapon,
		MakeReloadWeapon,
		ChoiceWeapon,
		TimeQuantPassed,
		ChangedCurrentWeapon,
		MakeShot,
		GotDamage,
		MakedShot,
		ChangedValueHP,
		CurrentWeaponState,
		MagazinState,
		HealthyState,
		CollectorState,
		FieldState,
		EndGame,
		ChangedTimeTillReduction,
		BodyZoneState,
		MakedKill,
		GamerDied,
		PlayerTurn,
		PlayerTurned,
		WeaponState,

	}
}
