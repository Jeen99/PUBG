
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;

namespace BattleRoyalClient
{
	interface IBattleModel
	{
		event ChangeModel BattleChangeModel;
		PlayerChararcter Chararcter { get; }
		Size SizeMap { get; }
	}

	/// <summary>
	/// Делегат события изменения модели
	/// </summary>
	public delegate void ChangeModel();
}