using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	interface IBattleModelForView
	{
		event ModelLoaded ModelLoaded;
		event BattleModelChangedHandler BattleModelChanged;
		event GameObjectChangedHandler GameObjectChanged;
		event ModelEndGame HappenedModelEndGame;
		event ServerDisconnect HappenedDisconnectServer;
		ICharacterForView CharacterView { get; }
		IDeathZoneForView DeathZone { get; }
		int CountPlayersInGame { get; }
	}
}
