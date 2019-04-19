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
		public event BattleModelChangedHandler BattleModelChanged;
		public event GameObjectChangedHandler GameObjectChanged;
		public event ChangeCountPlayers EventChangeCountPlayers;

		//пока задаем прямо в коде
		public Size SizeMap { get; } = new Size(500, 500);

		public PlayerChararcter Chararcter { get; private set; }
		public DeathZone DeathZone { get; set; }
		
		public ConcurrentDictionary<ulong, IModelObject> GameObjects { get; } 
			= new ConcurrentDictionary<ulong, IModelObject>();

		private int countPlayersInGame = 0;
		public int CountPlayersInGame
		{
			get { return countPlayersInGame; }
			set
			{
				countPlayersInGame = value;
				EventChangeCountPlayers?.Invoke();
			}
		}


		public void CreateChangeModel()
		{
			BattleModelChanged?.Invoke();
		}

		public void OnChangeGameObject(IModelObject model, StateObject state = StateObject.Change)
		{
			GameObjectChanged?.Invoke(model, state);
		}

		public BattleModel(ulong id)
		{
			Chararcter = new PlayerChararcter(id, this);
		}
	}

	
}
