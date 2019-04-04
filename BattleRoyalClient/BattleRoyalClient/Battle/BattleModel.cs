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

		//пока задаем прямо в коде
		public Size SizeMap { get; } = new Size(500, 500);

		public PlayerChararcter Chararcter { get; private set; }
		
		public ConcurrentDictionary<ulong, IModelObject> GameObjects { get; } 
			= new ConcurrentDictionary<ulong, IModelObject>();

		
		public void CreateChangeModel()
		{
			BattleModelChanged?.Invoke();
		}

		public void OnChangeGameObject(IModelObject model, StateObject state = StateObject.CHANGE)
		{
			GameObjectChanged?.Invoke(model, state);
		}

		public BattleModel(ulong id)
		{
			Chararcter = new PlayerChararcter(id, this);
		}
	}
}
