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
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using CommonLibrary;
using CSInteraction.Client;
using System.Collections.Concurrent;
using BattleRoyalClient.Battle;
using System.Diagnostics;
using System.Drawing;
using Point = System.Windows.Point;
using Brushes = System.Windows.Media.Brushes;

namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для BattleViev3d.xaml
	/// </summary>
	public partial class BattleView3d : Window, IBattleView
	{
		public bool Transition { get; set; }
		protected static readonly string pathResources = "/Resources/";
		private GameActionController battleContoller;
		private UserActionController userContoller;

		private VisualConteyner visual;// хранит 3Д модели

		public BattleView3d(ulong id, BaseClient<IMessage> client)
		{
			this.InitializeComponent();

			this.visual = new VisualConteyner(this.models);

			battleContoller = new GameActionController(id, client, this);
			userContoller = new UserActionController(client, battleContoller, this);

			battleContoller.Model.GameObjectChanged += Model_GameObjectChanged;
			battleContoller.Model.CharacterView.Event_CharacterChange += Handler_ChangeCharacter;
			battleContoller.Model.BattleModelChanged += Handler_BattleModelChanged; ;
			// обработчик клавишь
			this.KeyDown += userContoller.User_KeyDown;
			this.KeyUp += userContoller.User_KeyUp;
			this.MouseDown += BattleView3d_MouseDown;

			//viewport.MouseWheel += BattleView3d_MouseWheel;
			viewport.MouseMove += BattleView3d_MouseMove;
			this.Closed += Battle_Closed;
			client.SendMessage(new LoadedBattleForm(id));

		}

		private void Handler_BattleModelChanged(TypesChange typeChange)
		{
			this.Dispatcher.Invoke(() =>
			{
				switch (typeChange)
				{
					case TypesChange.CountPlyers:
						Handler_ChangeCountPlyers();
						break;
				}
			});
		}

		private void Handler_ChangeCountPlyers()
		{
			CountPlayers.Text = battleContoller.Model.CountPlayersInGame.ToString();
		}

		private void Handler_ChangeCharacter(TypesChangeCharacter typeChange)
		{
			this.Dispatcher.Invoke(() =>
			{
				switch (typeChange)
				{
					case TypesChangeCharacter.AddWeapon:
						Handler_CharacterChangeAddWeapon();
						break;
					case TypesChangeCharacter.All:
						Handler_CharacterChangeHP();
						Handler_CharacterChangeLocation();
						Handler_CharacterChangeCurrentWeapon();
						Handler_IndicatorDeadZone();
						break;
					case TypesChangeCharacter.CurrentWepon:
						Handler_CharacterChangeCurrentWeapon();
						break;
					case TypesChangeCharacter.HP:
						Handler_CharacterChangeHP();
						break;
					case TypesChangeCharacter.Location:
						Handler_CharacterChangeLocation();
						Handler_IndicatorDeadZone();
						break;
				}
			});
		}

		private void Handler_CharacterChangeLocation()
		{
			var cameraPosition = camera.Position;
			//меняем положение камеры
			var character = battleContoller.Model.CharacterView;
			cameraPosition.X = character.Location.X;
			cameraPosition.Y = character.Location.Y;
			camera.Position = cameraPosition;
		}

		private void Handler_CharacterChangeHP()
		{
			this.HP.Value = battleContoller.Model.CharacterView.HP;
		}

		private void Handler_CharacterChangeCurrentWeapon()
		{
			//снимаем выделение
			BorderGun.BorderBrush = Brushes.Black;
			BorderShotGun.BorderBrush = Brushes.Black;
			BorderAssaultRifle.BorderBrush = Brushes.Black;
			BorderGrenadeCollection.BorderBrush = Brushes.Black;
			switch (battleContoller.Model.CharacterView.Character.CurrentWeapon)
			{
				case TypesWeapon.Gun:
					BorderGun.BorderBrush = Brushes.Green;
					break;
				case TypesWeapon.ShotGun:
					BorderShotGun.BorderBrush = Brushes.Green;
					break;
				case TypesWeapon.AssaultRifle:
					BorderAssaultRifle.BorderBrush = Brushes.Green;
					break;
				case TypesWeapon.GrenadeCollection:
					BorderGrenadeCollection.BorderBrush = Brushes.Green;
					break;
			}
		}

		private void Handler_CharacterChangeAddWeapon()
		{
			for (int i = 0; i < battleContoller.Model.CharacterView.Weapons.Length; i++)
			{
				switch (battleContoller.Model.CharacterView.Weapons[i])
				{
					case TypesWeapon.Gun:
						Gun.Source = new BitmapImage(new Uri(pathResources + TypesWeapon.Gun.ToString() + "ForInventory.png", UriKind.Relative));
						break;
					case TypesWeapon.ShotGun:
						ShotGun.Source = new BitmapImage(new Uri(pathResources + TypesWeapon.ShotGun.ToString() + "ForInventory.png", UriKind.Relative));
						break;
					case TypesWeapon.AssaultRifle:
						AssaultRifle.Source = new BitmapImage(new Uri(pathResources + TypesWeapon.AssaultRifle.ToString() + "ForInventory.png", UriKind.Relative));
						break;
					case TypesWeapon.GrenadeCollection:
						GrenadeCollection.Source = new BitmapImage(new Uri(pathResources + TypesWeapon.GrenadeCollection.ToString() + "ForInventory.png", UriKind.Relative));
						break;
				}
			}
		}

		private void BattleView3d_MouseMove(object sender, MouseEventArgs e)
		{	
			   //определяем угол
			var angle = DefineAngle(e);
			userContoller.UserTurn(angle);
		}

		private void Model_GameObjectChanged(IModelObject model, StateObject state)
		{
			this.Dispatcher.Invoke(() =>
			{
				switch (state)
				{
					case StateObject.Change:
						Hanler_ChangeGameObject(model);
						break;
					case StateObject.Delete:
						visual.DeleteModel3d(model.ID);
						break;
				}
			});
		}

		private void Hanler_ChangeGameObject(IModelObject gameObject)
		{
			switch (gameObject.Type)
			{
				case TypesGameObject.DeathZone:
					if (!IndicatorDeadZone.IsEnabled && battleContoller.Model.DeathZone.Location != PointF.Empty)	// включаем индикатор
					{
						IndicatorDeadZone.IsEnabled = true;
					}
					Handler_ChangeDeathZone((DeathZone)gameObject);
					break;
				case TypesGameObject.Player:
					break;
			}
			if (gameObject.Type == TypesGameObject.Indefinitely)
				visual.AddOnlyVisual(gameObject);
			else
				visual.AddOrUpdate(gameObject, gameObject.ID);
		}

		private void Handler_ChangeDeathZone(DeathZone deathZone)
		{
			TimeDeathZone.Text = $"{deathZone.TimeToChange.Minutes.ToString("D2")}:{deathZone.TimeToChange.Seconds.ToString("D2")}";
			Handler_IndicatorDeadZone();
		}

		private void BattleView3d_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			// меняем масштаб
			var cameraPos = camera.Position;
			cameraPos.Z += e.Delta / 5;
			camera.Position = cameraPos;

			System.Diagnostics.Debug.WriteLine("Camera zoom:", cameraPos.Z);
		}

		private float DefineAngle(MouseEventArgs e)
		{
			var mousePosition = e.GetPosition(null);
			//центр карты
			var centre = new Point(viewport.ActualHeight / 2, viewport.ActualHeight / 2);
			//позиция мыши		
			float angle = (float)(Math.Atan2(mousePosition.Y - centre.Y, mousePosition.X - centre.X) / Math.PI * 180);
			angle = (angle < 0) ? angle + 360 : angle;   //Без этого диапазон от 0...180 и -1...-180
			return -angle;
		}

		private PointF DefinePositionClick(MouseEventArgs e)
		{
			Point point = e.GetPosition(null);
			//центр карты
			var centre = new Point(viewport.ActualHeight / 2, viewport.ActualHeight / 2);
			double scale = (30000 / camera.Position.Z) / 50;
			var inScale = (point - centre) / scale;
			return new PointF((float)(camera.Position.X + inScale.X), (float)(camera.Position.Y + inScale.Y));
		}

		private void BattleView3d_MouseDown(object sender, MouseButtonEventArgs e)
		{
			userContoller.MakeShot(DefinePositionClick(e));
		}

		private void Battle_Closed(object sender, EventArgs e)
		{
			if (!Transition)
			{
				Environment.Exit(0);
			}
		}

		private void Handler_IndicatorDeadZone()
		{
			if (battleContoller.Model.CharacterView.Character == null) return;

			var centre = battleContoller.Model.CharacterView.Location;
			var zone = battleContoller.Model.DeathZone.Location;
			//угол между игроком и зоной	
			float angle = (float)(Math.Atan2(zone.X - centre.X, zone.Y - centre.Y) / Math.PI * 180);

			var RotateTransform = IndicatorDeadZone.RenderTransform as RotateTransform;
			var transform = new RotateTransform(angle);
			IndicatorDeadZone.RenderTransform = transform;

		}
	}
}
