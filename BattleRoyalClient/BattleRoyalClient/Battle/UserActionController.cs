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

		public UserActionController(BaseClient client)
		{
			this.client = client;
		}



		public void User_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Left:
					if (e.KeyboardDevice.GetKeyStates(Key.Up) == KeyStates.None ||
						e.KeyboardDevice.GetKeyStates(Key.Down) == KeyStates.None)
					{
						if (e.KeyboardDevice.GetKeyStates(Key.Up) == KeyStates.Down)
							client.SendMessage(new GoTo(Directions.left_bottom));
						else if (e.KeyboardDevice.GetKeyStates(Key.Down) == KeyStates.Down)
							client.SendMessage(new GoTo(Directions.left_top));
					}
					client.SendMessage(new GoTo(Directions.left));
					break;
				case Key.Right:
					if (e.KeyboardDevice.GetKeyStates(Key.Up) == KeyStates.None ||
						e.KeyboardDevice.GetKeyStates(Key.Down) == KeyStates.None)
					{
						if (e.KeyboardDevice.GetKeyStates(Key.Up) == KeyStates.Down)
							client.SendMessage(new GoTo(Directions.right_bottom));
						else if (e.KeyboardDevice.GetKeyStates(Key.Down) == KeyStates.Down)
							client.SendMessage(new GoTo(Directions.right_top));
					}
					client.SendMessage(new GoTo(Directions.right));
					break;
				case Key.Down:
					if (e.KeyboardDevice.GetKeyStates(Key.Left) == KeyStates.None ||
						e.KeyboardDevice.GetKeyStates(Key.Right) == KeyStates.None)
					{
						if (e.KeyboardDevice.GetKeyStates(Key.Left) == KeyStates.Down)
							client.SendMessage(new GoTo(Directions.left_bottom));
						else if (e.KeyboardDevice.GetKeyStates(Key.Right) == KeyStates.Down)
							client.SendMessage(new GoTo(Directions.right_bottom));
					}
					client.SendMessage(new GoTo(Directions.bottom));
					break;
				case Key.Up:
					if (e.KeyboardDevice.GetKeyStates(Key.Left) == KeyStates.None ||
						e.KeyboardDevice.GetKeyStates(Key.Right) == KeyStates.None)
					{
						if (e.KeyboardDevice.GetKeyStates(Key.Left) == KeyStates.Down)
							client.SendMessage(new GoTo(Directions.left_top));
						else if (e.KeyboardDevice.GetKeyStates(Key.Right) == KeyStates.Down)
							client.SendMessage(new GoTo(Directions.right_top));
					}
					client.SendMessage(new GoTo(Directions.top));
					break;
			}
		}

		public void User_KeyUp(object sender, KeyEventArgs e)
		{

			if (e.KeyboardDevice.GetKeyStates(Key.Left) == KeyStates.None ||
				e.KeyboardDevice.GetKeyStates(Key.Right) == KeyStates.None)
			{
				if (e.KeyboardDevice.GetKeyStates(Key.Left) == KeyStates.Down)
				{
					if (e.KeyboardDevice.GetKeyStates(Key.Up) == KeyStates.None ||
						e.KeyboardDevice.GetKeyStates(Key.Down) == KeyStates.None)
					{
						if (e.KeyboardDevice.GetKeyStates(Key.Down) == KeyStates.Down)
						{
							client.SendMessage(new GoTo(Directions.left_bottom));
						}
						else if (e.KeyboardDevice.GetKeyStates(Key.Up) == KeyStates.Down)
						{
							client.SendMessage(new GoTo(Directions.left_top));
						}
					}
					client.SendMessage(new GoTo(Directions.left));
				}
				else if (e.KeyboardDevice.GetKeyStates(Key.Right) == KeyStates.Down)
				{
					if (e.KeyboardDevice.GetKeyStates(Key.Up) == KeyStates.None ||
						e.KeyboardDevice.GetKeyStates(Key.Down) == KeyStates.None)
					{
						if (e.KeyboardDevice.GetKeyStates(Key.Down) == KeyStates.Down)
						{
							client.SendMessage(new GoTo(Directions.right_bottom));
						}
						else if (e.KeyboardDevice.GetKeyStates(Key.Up) == KeyStates.Down)
						{
							client.SendMessage(new GoTo(Directions.right_top));
						}
					}
					client.SendMessage(new GoTo(Directions.right));
				}

				client.SendMessage(new StopMove());
			}
		}

	}
}
