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
		private ulong id;
		private const double Height = 400;
		private const double Width = 800;

		public ConcurrentDictionary<ulong, IGameObject> GameObjects { get; private set; }

		private Bitmap background;
		public Bitmap GetBackground
		{
			get
			{
				return (Bitmap)background;
			}
		}

		public PointF CentreScreen {
			get {
				return GameObjects[id].Location;
			}
		}

		public void CreateChangeModel()
		{
			BattleChangeModel();		
		}

		/// <summary>
		/// Рисует статическую картнику карту 
		/// </summary>
		public Bitmap CreateBackGround()
		{
			Bitmap bitmap = new Bitmap(1000, 1000);
			using (Graphics gr = Graphics.FromImage(bitmap))
			{
				gr.Clear(Color.Green);
				Painter.DrawStone(gr, new Tuple<double, double>(10, 10), 8);
				Painter.DrawStone(gr, new Tuple<double, double>(30,  28), 1);
				Painter.DrawStone(gr, new Tuple<double, double>(78, 30), 2);
				Painter.DrawBox(gr, new Tuple<double, double>(40, 100));
				Painter.DrawBox(gr, new Tuple<double, double>(150, 10));
			}

			return bitmap;
			
		}

		public BattleModel(ulong id, string passwrod, string nickName)
		{
			Passwrod = passwrod;
			NickName = nickName;
			this.id = id;
			background = CreateBackGround();
			GameObjects = new ConcurrentDictionary<ulong, IGameObject>();
		}
	}
}
