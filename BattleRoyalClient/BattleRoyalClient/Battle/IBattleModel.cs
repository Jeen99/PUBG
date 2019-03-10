
using System;
using System.Drawing;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;

namespace BattleRoyalClient
{
	interface IBattleModel
	{
		event ChangeModel BattleChangeModel;
		Bitmap GetBackground { get; }
		ConcurrentDictionary<ulong, IGameObject> GameObjects { get; }
		PointF CentreScreen { get; }
	}
}