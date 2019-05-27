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
					case Key.A:
						direction.Horisontal = DirectionHorisontal.Left;
						client.SendMessage(new GoTo(model.IDPlayer, direction));
						break;
					case Key.Right:
					case Key.D:
						direction.Horisontal = DirectionHorisontal.Right;
						client.SendMessage(new GoTo(model.IDPlayer, direction));
						break;
					case Key.Up:
					case Key.W:
						direction.Vertical = DirectionVertical.Up;
						client.SendMessage(new GoTo(model.IDPlayer, direction));
						break;
					case Key.Down:
					case Key.S:
						direction.Vertical = DirectionVertical.Down;
						client.SendMessage(new GoTo(model.IDPlayer, direction));
						break;
					case Key.F:
						client.SendMessage(new TryPickUp(model.IDPlayer));
						break;
					case Key.D1:
					case Key.NumPad1:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.Gun));
						break;
					case Key.D2:
					case Key.NumPad2:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.ShotGun));
						break;
					case Key.D3:
					case Key.NumPad3:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.AssaultRifle));
						break;
					case Key.D4:
					case Key.NumPad4:
						client.SendMessage(new ChoiceWeapon(model.IDPlayer, TypesWeapon.GrenadeCollection));
						break;
					case Key.R:
						client.SendMessage(new MakeReloadWeapon(model.IDPlayer));
						break;
					case Key.Escape:
						client.Close();
						break;
				}
			}
		}

		public void User_KeyUp(object sender, KeyEventArgs e)
		{
			if (model.ModelIsLoaded)
			{
				if (e.KeyboardDevice.IsKeyUp(Key.Left) && e.KeyboardDevice.IsKeyUp(Key.A) 
				&& e.KeyboardDevice.IsKeyUp(Key.Right) && e.KeyboardDevice.IsKeyUp(Key.D))
				{
					direction.Horisontal = DirectionHorisontal.None;
					client.SendMessage(new GoTo(model.IDPlayer, direction));
				}

				if (e.KeyboardDevice.IsKeyUp(Key.Up)  && e.KeyboardDevice.IsKeyUp(Key.W)
				&& e.KeyboardDevice.IsKeyUp(Key.Down) && e.KeyboardDevice.IsKeyUp(Key.S))
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
