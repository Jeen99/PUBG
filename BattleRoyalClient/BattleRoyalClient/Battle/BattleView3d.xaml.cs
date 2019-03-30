using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CSInteraction.ProgramMessage;
using CSInteraction.Client;

namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для BattleViev3d.xaml
	/// </summary>
	public partial class BattleView3d : Window
	{
		public bool Transition { get; set; }

		private GameActionController battleContoller;
		private UserActionController userContoller;

		private DispatcherTimer timer;      // для обновления экрана

		public BattleView3d(ulong id, BaseClient client)
		{
			this.InitializeComponent();
			this.LayoutUpdated += MainWindow_LayoutUpdated;		// обработчик перериовки
			
			// УБРАТЬ НЕНУЖНЫЕ ПОЛЯ!!!!
			battleContoller = new GameActionController(id, client, this);
			//battleContoller.Model.BattleChangeModel += Model_BattleChangeModel;
			userContoller = new UserActionController(client);

			// обработчик клавишь
			this.KeyDown += userContoller.User_KeyDown;
			this.KeyUp += userContoller.User_KeyUp;
			this.MouseWheel += BattleView3d_MouseWheel;
			client.SendMessage(new LoadedBattleForm());

			// таймер перерисовки
			timer = new DispatcherTimer();
			timer.Tick += Repaint;
			timer.Interval = new TimeSpan(1_000_000 / 60);      // ~60fps update
			timer.Start();
		}

		private void BattleView3d_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			// меняем масштаб
			var cameraPos = camera.Position;

			cameraPos.Z += e.Delta;
			camera.Position = cameraPos;

			System.Diagnostics.Debug.WriteLine(cameraPos.Z);
		}

		private void Repaint(object sender, EventArgs e)
		{
			this.InvalidateArrange();
		}

		private void MainWindow_LayoutUpdated(object sender, EventArgs e)
		{
			var cameraPos = camera.Position;
			var character = battleContoller.Model.Chararcter;

			cameraPos.X = character.StartAxises.X;
			cameraPos.Y = character.StartAxises.Y;

			this.camera.Position = cameraPos;
		}
	}
}
