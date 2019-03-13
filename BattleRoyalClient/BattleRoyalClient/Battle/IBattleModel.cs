
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
		List<IGameObject> VisibleObjects { get; }
		Tuple<float, float> DiapasonX { get ; }
		Tuple<float, float> DiapasonY { get ; }
	}
}