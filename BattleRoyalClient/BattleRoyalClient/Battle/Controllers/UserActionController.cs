using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Windows.Input;
using System.Drawing;
using CommonLibrary.GameMessages;

namespace BattleRoyalClient
{
	class UserActionController
	{
		private BaseClient<IMessage> client;
		private Direction direction;
		private GameActionController gameController;
		private BattleView3d view;
		private ulong idPlayer;

		public UserActionController(BaseClient<IMessage> client, GameActionController controller, BattleView3d view)
		{
			this.client = client;
			gameController = controller;
			idPlayer = gameController.Model.Chararcter.ID;
			this.view = view;
			this.direction = new Direction();
		}

		public void User_KeyDown(object sender, KeyEventArgs e)
		{
			if (gameController.Loaded)
			{
				switch (e.Key)
				{
					case Key.Left:
						direction.Horisontal = DirectionHorisontal.Left;
						break;
					case Key.Right:
						direction.Horisontal = DirectionHorisontal.Right;
						break;
					case Key.Up:
						direction.Vertical = DirectionVertical.Up;
						break;
					case Key.Down:
						direction.Vertical = DirectionVertical.Down;
						break;
					case Key.F:
						client.SendMessage(new TryPickUp(idPlayer));
						break;
					case Key.Oem1:
						client.SendMessage(new ChoiceWeapon(idPlayer, TypesWeapon.Gun));
						break;
					case Key.Oem2:
						client.SendMessage(new ChoiceWeapon(idPlayer, TypesWeapon.ShotGun));
						break;
					case Key.Oem3:
						client.SendMessage(new ChoiceWeapon(idPlayer, TypesWeapon.AssaultRifle));
						break;
					case Key.Oem4:
						client.SendMessage(new ChoiceWeapon(idPlayer, TypesWeapon.GrenadeCollection));
						break;
					case Key.R:
						client.SendMessage(new MakeReloadWeapon(idPlayer));
						break;
					case Key.Escape:
						client.Close();
						break;
					case Key.NumPad1:
						client.SendMessage(new ChoiceWeapon(idPlayer, TypesWeapon.Gun));
						break;
					case Key.NumPad2:
						client.SendMessage(new ChoiceWeapon(idPlayer, TypesWeapon.ShotGun));
						break;
					case Key.NumPad3:
						client.SendMessage(new ChoiceWeapon(idPlayer, TypesWeapon.AssaultRifle));
						break;
					case Key.NumPad4:
						client.SendMessage(new ChoiceWeapon(idPlayer, TypesWeapon.GrenadeCollection));
						break;
				}

				if (e.Key == Key.Up ||
					e.Key == Key.Down ||
					e.Key == Key.Left ||
					e.Key == Key.Right)
				{
					client.SendMessage(new GoTo(idPlayer, direction));
				}
			}
		}

		public void User_KeyUp(object sender, KeyEventArgs e)
		{
			if (gameController.Loaded)
			{
				if (e.KeyboardDevice.IsKeyUp(Key.Up) &&
				e.KeyboardDevice.IsKeyUp(Key.Right))
				{
					direction.Horisontal = DirectionHorisontal.None;
					client.SendMessage(new GoTo(idPlayer, direction));
				}

				if (e.KeyboardDevice.IsKeyUp(Key.Up) &&
					e.KeyboardDevice.IsKeyUp(Key.Down))
				{
					direction.Vertical = DirectionVertical.None;
					client.SendMessage(new GoTo(idPlayer, direction));
				}
			}
		}

		public void MakeShot(PointF pointOfClick)
		{
			if (gameController.Loaded)
			{
				client.SendMessage(new MakeShot(idPlayer, pointOfClick));
			}
		}

		public void UserTurn(float angle)
		{
			if (gameController.Loaded)
			{
				//отправляем сообщение
				client.SendMessage(new PlayerTurn(idPlayer, angle));
				gameController.Model.Chararcter.Character.Update(angle);
				view.Dispatcher.Invoke(() => { gameController.Model.OnChangeGameObject(gameController.Model.Chararcter.Character); });
			}
		}
	}
}
