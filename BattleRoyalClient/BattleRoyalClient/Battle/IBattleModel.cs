
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

		//void AddOrUpdateGameObject(IModelObject model, ulong ID);
		//void UpdateGameObject(IModelObject model, ulong ID);
	}

	/// <summary>
	/// Делегат события изменения модели
	/// </summary>
	delegate void BattleModelChangedHandler();
	delegate void GameObjectChangedHandler(IModelObject model, ulong ID);
}