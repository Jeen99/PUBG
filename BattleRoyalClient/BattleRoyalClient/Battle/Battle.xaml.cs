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
using Size = System.Drawing.Size;

namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для Battle.xaml
	/// </summary>
	public partial class Battle : Window
	{
		private GameActionController battleContoller;
		private UserActionController userContoller;
		private Bitmap background;
		
		public bool Transition { get; set; }
		public Battle(ulong id, BaseClient client, string nickName, string password)
		{
			InitializeComponent();
			battleContoller = new GameActionController(id, client, nickName, password, this);
			battleContoller.Model.BattleChangeModel += Model_BattleChangeModel;
			userContoller = new UserActionController(client);
			background = CreateBackGround();
		}

		private void Model_BattleChangeModel()
		{
			//клонируем основное изображение
			Bitmap frame = (Bitmap)background.Clone();
			
			using (Graphics gr = Graphics.FromImage(frame))
			{
				foreach (var gameObject in battleContoller.Model.VisibleObjects)
				{
					gameObject.Draw(gr);
				}
			}

			PointF Location = new PointF();
			if (battleContoller.Model.DiapasonX.Item1 < 0) Location.X = 0;
			else Location.X = battleContoller.Model.DiapasonX.Item1;

			if (battleContoller.Model.DiapasonY.Item1 < 0) Location.Y = 0;
			else Location.Y = battleContoller.Model.DiapasonY.Item1;

			Bitmap bit = frame.Clone(new RectangleF(Location, new Size(300,200)), frame.PixelFormat);
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
		/// <summary>
		/// Рисует статическую картнику карту 
		/// </summary>
		public Bitmap CreateBackGround()
		{
			Bitmap bitmap = new Bitmap(Properties.Recources.Field);
			using (Graphics gr = Graphics.FromImage(bitmap))
			{
				Painter.DrawStone(gr, new PointF(10, 10), new System.Drawing.Size(160,160));
				Painter.DrawStone(gr, new PointF(30, 28), new System.Drawing.Size(20, 20));
				Painter.DrawStone(gr, new PointF(78, 30), new System.Drawing.Size(40, 40));
				Painter.DrawBox(gr, new PointF(40, 100));
				Painter.DrawBox(gr, new PointF(150, 10));
			}
			

			return bitmap;

		}
	}
}
