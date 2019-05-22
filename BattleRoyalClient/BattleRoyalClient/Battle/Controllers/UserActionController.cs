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
		private IBattleModelForController model;

		public UserActionController(BaseClient<IMessage> client, IBattleModelForController model, BattleView3d view)
		{
			this.client = client;
			this.model = model;
			this.direction = new Direction();
		}

		public void User_KeyDown(object sender, KeyEventArgs e)
		{
			if (model.ModelIsLoaded)
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
						client.SendMessage(new TryPickUp(model.IDPlayer));
						break;
					case Key.Oem1:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.Gun));
						break;
					case Key.Oem2:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.ShotGun));
						break;
					case Key.Oem3:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.AssaultRifle));
						break;
					case Key.Oem4:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.GrenadeCollection));
						break;
					case Key.R:
						client.SendMessage(new MakeReloadWeapon(model.IDPlayer));
						break;
					case Key.Escape:
						client.Close();
						break;
					case Key.NumPad1:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.Gun));
						break;
					case Key.NumPad2:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.ShotGun));
						break;
					case Key.NumPad3:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.AssaultRifle));
						break;
					case Key.NumPad4:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.GrenadeCollection));
						break;
				}

				if (e.Key == Key.Up ||
					e.Key == Key.Down ||
					e.Key == Key.Left ||
					e.Key == Key.Right)
				{
					client.SendMessage(new GoTo(model.IDPlayer, direction));
				}
			}
		}

		public void User_KeyUp(object sender, KeyEventArgs e)
		{
			if (model.ModelIsLoaded)
			{
				if (e.KeyboardDevice.IsKeyUp(Key.Up) &&
				e.KeyboardDevice.IsKeyUp(Key.Right))
				{
					direction.Horisontal = DirectionHorisontal.None;
					client.SendMessage(new GoTo(model.IDPlayer, direction));
				}

				if (e.KeyboardDevice.IsKeyUp(Key.Up) &&
					e.KeyboardDevice.IsKeyUp(Key.Down))
				{
					direction.Vertical = DirectionVertical.None;
					client.SendMessage(new GoTo(model.IDPlayer, direction));
				}
			}
		}

		public void MakeShot(PointF pointOfClick)
		{
			if (model.ModelIsLoaded)
			{
				client.SendMessage(new MakeShot(model.IDPlayer, pointOfClick));
			}
		}

		public void UserTurn(float angle)
		{
			if (model.ModelIsLoaded)
			{
				//отправляем сообщение
				IMessage msg = new PlayerTurn(model.IDPlayer, angle);
				client.SendMessage(msg);
				model.AddOutgoingMsg(msg);
			}
		}

		public void Handler_BattleFormLoad()
		{			
			client.SendMessage(new LoadedBattleForm(model.IDPlayer));
		}

		public void ViewClose()
		{
			model.ClearModel();
		}

		public void InitializeModel(ulong idGamer)
		{
			model.Initialize(idGamer);
		}
	}
}
