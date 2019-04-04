
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

		PlayerChararcter Chararcter { get; }
		Size SizeMap { get; }

		void CreateChangeModel();
		void OnChangeGameObject(IModelObject model, StateObject state = StateObject.CHANGE);

	}

	enum StateObject { CHANGE, DELETE };
	/// <summary>
	/// Делегат события изменения модели
	/// </summary>
	delegate void BattleModelChangedHandler();
	delegate void GameObjectChangedHandler(IModelObject model, StateObject state);
}