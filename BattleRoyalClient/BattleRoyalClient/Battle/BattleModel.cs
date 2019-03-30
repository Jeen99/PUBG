using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;

namespace BattleRoyalClient
{
	class BattleModel : IBattleModel
	{
		public event ChangeModel BattleChangeModel;

		//пока задаем прямо в коде
		public Size SizeMap { get; } = new Size(500, 500);

		public PlayerChararcter Chararcter { get; private set; }
		
		public ConcurrentDictionary<ulong, GameObject> GameObjects { get; } 
			= new ConcurrentDictionary<ulong, GameObject>();

		
		public void CreateChangeModel()
		{
			BattleChangeModel?.Invoke();
		}
	
		public BattleModel(ulong id)
		{
			Chararcter = new PlayerChararcter(id, this);
		}
	}
}
