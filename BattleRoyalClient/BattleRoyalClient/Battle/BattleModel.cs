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
		public string Passwrod { get; private set; }
		public string NickName { get; private set; }

		//пока задаем прямо в коде
		public Size SizeMap { get; } = new Size(1000, 1000);

		public PlayerChararcter Chararcter { get; private set; }
		
		public ConcurrentDictionary<ulong, IGameObject> GameObjects { get; } 
			= new ConcurrentDictionary<ulong, IGameObject>();

		
		public void CreateChangeModel()
		{
			BattleChangeModel();		
		}
	
		public BattleModel(ulong id, string passwrod, string nickName)
		{
			Passwrod = passwrod;
			NickName = nickName;
			Chararcter = new PlayerChararcter(id, this);
		}
	}
}
