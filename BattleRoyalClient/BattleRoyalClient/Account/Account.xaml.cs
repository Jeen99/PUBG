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

namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для Account.xaml
	/// </summary>
	public partial class Account : Window
	{
		private AccountController controller;
		/// <summary>
		/// Если true, то происходит переход из формы в форму и приложение закрывать не надо
		/// </summary>
		public bool Transition { get; set; }

		public Account(BaseClient client)
		{
			InitializeComponent();
			controller = new AccountController(client, this);
			InQueue.Click += controller.InQueue;
			controller.Model.AutorizationModelChange += Model_AutorizationModelChange;
			this.Closed += Account_Closed;
		}

		private void Account_Closed(object sender, EventArgs e)
		{
			if (!Transition)
			{
				Environment.Exit(0);
			}
		}

		private void Model_AutorizationModelChange()
		{
			Kills.Text = controller.Model.Kills.ToString();
			Deaths.Text = controller.Model.Deaths.ToString();
			Battles.Text = controller.Model.Battles.ToString();
			GameTime.Text = controller.Model.GameTime.ToString();
		}
	}
}
