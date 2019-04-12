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
using System.Collections.Concurrent;
using BattleRoyalClient.Battle;

namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для BattleViev3d.xaml
	/// </summary>
	public partial class BattleView3d : Window, IBattleView
	{
		public bool Transition { get; set; }

		private GameActionController battleContoller;
		private UserActionController userContoller;
		private BaseClient client;

		private DispatcherTimer timer;      // для обновления экрана

		private VisualConteyner visual;				// хранит 3Д модели

		public BattleView3d(ulong id, BaseClient client)
		{
			this.InitializeComponent();

			this.client = client;
			//this.LayoutUpdated += MainWindow_LayoutUpdated;     // обработчик перериовки

			this.visual = new VisualConteyner(this.models);
			
			// УБРАТЬ НЕНУЖНЫЕ ПОЛЯ!!!!
			battleContoller = new GameActionController(id, client, this);
			//battleContoller.Model.BattleChangeModel += Model_BattleChangeModel;
			userContoller = new UserActionController(client);

			battleContoller.Model.GameObjectChanged += Model_GameObjectChanged;
			battleContoller.Model.Chararcter.changeHP += Chararcter_changeHP;
			battleContoller.Model.Chararcter.changePosition += Chararcter_changePosition;

			// обработчик клавишь
			this.KeyDown += userContoller.User_KeyDown;
			this.KeyUp += userContoller.User_KeyUp;
			this.MouseWheel += BattleView3d_MouseWheel;
			this.Closed += Battle_Closed;
			client.SendMessage(new LoadedBattleForm());

			this.MouseDown += BattleView3d_MouseDown;

			// таймер перерисовки
			//timer = new DispatcherTimer();
			//timer.Tick += Repaint;
			//timer.Interval = new TimeSpan(1_000_000 / 60);      // ~60fps update
			//timer.Start();
		}

		private void BattleView3d_MouseDown(object sender, MouseButtonEventArgs e)
		{
			var p1 = e.GetPosition(null);
			var p2 = new Point(viewport.ActualWidth / 2, viewport.ActualHeight / 2);
			
			float angle = (float)(Math.Atan2(p1.Y - p2.Y, p1.X - p2.X) / Math.PI * 180);
			angle = (angle < 0) ? angle + 360 : angle;   //Без этого диапазон от 0...180 и -1...-180

			System.Diagnostics.Debug.WriteLine("Angle: " + angle);
			client.SendMessage(new MakeShot(angle));
		}

		private void Battle_Closed(object sender, EventArgs e)
		{
			if (!Transition)
			{
				Environment.Exit(0);
			}
		}

		private void Chararcter_changePosition(System.Drawing.PointF location)
		{
			var cameraPosition = camera.Position;
			cameraPosition.X = location.X;
			cameraPosition.Y = location.Y;

			camera.Position = cameraPosition;
		}

		private void Model_GameObjectChanged(IModelObject model, StateObject state)
		{
			switch (state)
			{
				case StateObject.CHANGE:
					visual.AddOrUpdate(model, model.ID);
					break;
				case StateObject.DELETE:
					visual.DeleteModel3d(model.ID);
					break;
			}
		}

		private void Chararcter_changeHP(float hp)
		{
			this.HP.Value = hp;
		}

		private void BattleView3d_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			// меняем масштаб
			var cameraPos = camera.Position;
			cameraPos.Z += e.Delta / 5;
			camera.Position = cameraPos;

			System.Diagnostics.Debug.WriteLine("Camera zoom:", cameraPos.Z);
		}
	}
}
