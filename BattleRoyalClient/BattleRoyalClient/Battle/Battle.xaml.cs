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
using System.Windows.Shapes;
using CSInteraction.Client;
using CSInteraction.Common;
using System.IO;
using System.Drawing;
using Size = System.Drawing.Size;
using System.Windows.Media.Imaging;

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
		private Size sizeBattleScreen = new Size(1500, 1000);
		
		public bool Transition { get; set; }
		public Battle(ulong id, BaseClient client, string nickName, string password)
		{
			InitializeComponent();
			battleContoller = new GameActionController(id, client, nickName, password, this);
			battleContoller.Model.BattleChangeModel += Model_BattleChangeModel;
			userContoller = new UserActionController(client);
			background = new Bitmap(sizeBattleScreen.Width, sizeBattleScreen.Height);
		}

		private void Model_BattleChangeModel()
		{
			//клонируем основное изображение
			using (Graphics gr = Graphics.FromImage(background))
			{
				//создаем новый фон
				CreateBackground(gr);
				List<IGameObject> visibleObjects = battleContoller.Model.Chararcter.VisibleObjects;
				foreach (IGameObject gameObject in visibleObjects)
				{
					Painter.Draw(gameObject, gr);
				}
				gr.RotateTransform(180);
				Field.Source = BitmapToImageSource(background);
			}
				
			
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
		public void CreateBackground(Graphics frame)
		{
			//очищаем кадр от предыдущих изображений
			frame.Clear(System.Drawing.Color.FromArgb(128, 175,73));


			//определяем выходит ли область обзора за границу карты
			OverflowMap overflow = battleContoller.Model.Chararcter.GetOverflow;

			if (overflow.Left != 0 || overflow.Right != 0 || 
				overflow.Top != 0 || overflow.Bottom != 0)
			{
				using (Pen NewPen = new Pen(new SolidBrush(Color.Red), 3))
				{
					//настраиваем перо
					NewPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
					NewPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
					NewPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
					if (overflow.Left != 0)
					{
						float xBias = ConvertPosition.ConvertToViewAxis(overflow.Left);
						frame.DrawLine(NewPen, new PointF(xBias, 0), 
							new PointF(xBias, sizeBattleScreen.Height));
					}

					if (overflow.Top != 0)
					{
						float yBias = sizeBattleScreen.Height - ConvertPosition.ConvertToViewAxis(overflow.Top);
						frame.DrawLine(NewPen, new PointF(0, yBias),
							new PointF(sizeBattleScreen.Width, yBias));
					}

					if (overflow.Right != 0)
					{
						float xBias = sizeBattleScreen.Width -  ConvertPosition.ConvertToViewAxis(overflow.Right);
						frame.DrawLine(NewPen, new PointF(xBias, 0),
							new PointF(xBias, sizeBattleScreen.Height));
					}

					if (overflow.Bottom != 0)
					{
						float yBias = ConvertPosition.ConvertToViewAxis(overflow.Top);
						frame.DrawLine(NewPen, new PointF(0, yBias),
							new PointF(sizeBattleScreen.Width, yBias));
					}
				}
				
			}
			

		}
	}
}
