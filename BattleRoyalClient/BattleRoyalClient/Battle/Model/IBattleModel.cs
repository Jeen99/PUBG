
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;

namespace BattleRoyalClient
{
	interface IBattleModel
	{
		event BattleModelChangedHandler BattleModelChanged;
		event GameObjectChangedHandler GameObjectChanged;
		event ChangeCountPlayers EventChangeCountPlayers;

		PlayerChararcter Chararcter { get; }
		DeathZone DeathZone { get; }
		Size SizeMap { get; }
		int CountPlayersInGame { get; }


		void CreateChangeModel();
		void OnChangeGameObject(IModelObject model, StateObject state = StateObject.Change);

	}

	enum StateObject { Change, Delete };
	/// <summary>
	/// Делегат события изменения модели
	/// </summary>
	delegate void BattleModelChangedHandler();
	delegate void GameObjectChangedHandler(IModelObject model, StateObject state);
	delegate void ChangeCountPlayers();
}