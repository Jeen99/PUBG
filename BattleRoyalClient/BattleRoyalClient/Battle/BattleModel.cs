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
		
		/// <summary>
		/// Диапазон видимых координат по X
		/// </summary>
		private Tuple<float, float> diapasonX;
		private Tuple<float, float> diapasonY;
		private const float VisibleWidth = 300; 
		private const float VisibleHeight = 200;
		/// <summary>
		/// Идентификатор объекта игрока которым управляет клиент
		/// </summary>
		private ulong id;

		public ConcurrentDictionary<ulong, IGameObject> GameObjects { get; } 
			= new ConcurrentDictionary<ulong, IGameObject>();

		
		public List<IGameObject> VisibleObjects {
			get {
				if (diapasonX == null || diapasonY == null)
				{
					CalculateVisibleRegion(); 
				}
				List<IGameObject> visibleObjects = new List<IGameObject>();
				foreach (var keyValue in GameObjects)
				{
					IGameObject gameObject = keyValue.Value;
					if (diapasonX.Item1 <= gameObject.Location.X && gameObject.Location.X <= diapasonX.Item2 &&
						diapasonY.Item1 <= gameObject.Location.Y && gameObject.Location.Y <= diapasonY.Item2)
					{
						visibleObjects.Add(gameObject);
					}
				}
				return visibleObjects;
			}
		}

		public Tuple<float, float> DiapasonX { get => diapasonX; }
		public Tuple<float, float> DiapasonY { get => diapasonY; }

		public void GamerMoved()
		{
			CalculateVisibleRegion();
		}

		public void CalculateVisibleRegion()
		{
			PointF location = GameObjects[id].Location;
			diapasonX = new Tuple<float, float>(location.X - VisibleWidth / 2, location.X + VisibleWidth / 2);
			diapasonY = new Tuple<float, float>(location.Y - VisibleHeight / 2, location.Y + VisibleHeight / 2);
		}

		public void CreateChangeModel()
		{
			BattleChangeModel();		
		}
	
		public BattleModel(ulong id, string passwrod, string nickName)
		{
			Passwrod = passwrod;
			NickName = nickName;
			this.id = id;
		}
	}
}
