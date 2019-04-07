using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;
using CSInteraction.Client;
using CSInteraction.Common;
using System.Windows.Input;

namespace BattleRoyalClient
{
	class UserActionController
	{
		private BaseClient client;
		private Direction direction;

		public UserActionController(BaseClient client)
		{
			this.client = client;
			this.direction = new Direction();
		}

		public void User_KeyDown(object sender, KeyEventArgs e)
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
					client.SendMessage(new TryPickUp());
					break;
				case Key.Oem1:
					client.SendMessage(new ChoiceWeapon(TypesWeapon.Gun));
					break;
				case Key.Oem2:
					client.SendMessage(new ChoiceWeapon(TypesWeapon.ShotGun));
					break;
				case Key.Oem3:
					client.SendMessage(new ChoiceWeapon(TypesWeapon.AssaultRifle));
					break;
				case Key.Oem4:
					client.SendMessage(new ChoiceWeapon(TypesWeapon.GrenadeCollection));
					break;
				case Key.R:
					client.SendMessage(new MakeReloadWeapon());
					break;
				case Key.Escape:
					client.Close();
					break;
			}

			if (e.Key == Key.Up	  ||
				e.Key == Key.Down ||
				e.Key == Key.Left ||
				e.Key == Key.Right)
			{
				client.SendMessage(new GoTo(direction));
			}
		}

		public void User_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyboardDevice.IsKeyUp(Key.Up) &&
				e.KeyboardDevice.IsKeyUp(Key.Right))
			{
				direction.Horisontal = DirectionHorisontal.None;
				client.SendMessage(new GoTo(direction));
			}

			if (e.KeyboardDevice.IsKeyUp(Key.Up) &&
				e.KeyboardDevice.IsKeyUp(Key.Down))
			{
				direction.Vertical = DirectionVertical.None;
				client.SendMessage(new GoTo(direction));
			}
		}
	}
}
