using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CSInteraction.Client;
using CSInteraction.Common;
using System.IO;
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;

namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для Battle.xaml
	/// </summary>
	public partial class Battle : Window
	{
		private GameActionController battleContoller;
		private UserActionController userContoller;

		public bool Transition { get; set; }
		public Battle(ulong id, BaseClient client, string nickName, string password)
		{
			InitializeComponent();
			battleContoller = new GameActionController(id, client, nickName, password, this);
			battleContoller.Model.BattleChangeModel += Model_BattleChangeModel;
			userContoller = new UserActionController(client);
		}

		private void Model_BattleChangeModel()
		{
			//клонируем основное изображение
			Bitmap frame = (Bitmap)battleContoller.Model.GetBackground;
			//получаем центр экрана
			Tuple<double, double> CentreScreen = battleContoller.Model.CentreScreen;
			//вычисляем дипазоны
			Tuple<double, double> DiapasonX = new Tuple<double, double>(CentreScreen.Item1 - Field.Width / 2, CentreScreen.Item1 + Field.Width / 2);
			Tuple<double, double> DiapasonY = new Tuple<double, double>(CentreScreen.Item2 - Field.Height / 2, CentreScreen.Item1 + Field.Height / 2);
			using (Graphics gr = Graphics.FromImage(frame))
			{
				foreach (var gameObject in battleContoller.Model.GameObjects)
				{
					if(DiapasonX.Item1 <= gameObject.Value.Location.Item1 && DiapasonX.Item2 >= gameObject.Value.Location.Item1 &&
						DiapasonY.Item1 <= gameObject.Value.Location.Item2 && DiapasonY.Item2 >= gameObject.Value.Location.Item2)
						gameObject.Value.Draw(gr);
					
				}
			}
			
			Bitmap bit = frame.Clone(new Rectangle(0, 0,
				120, 60), frame.PixelFormat);
			Field.Source = BitmapToImageSource(bit);

		}
		BitmapImage BitmapToImageSource(Bitmap bitmap)
		{
			using (MemoryStream memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				BitmapImage bitmapimage = new BitmapImage();
				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();

				return bitmapimage;
			}
		}
	}
}
