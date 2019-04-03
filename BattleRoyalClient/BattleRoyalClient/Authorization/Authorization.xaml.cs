using BattleRoyalClient.Authorization;
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

namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для Autorization.xaml
	/// </summary>
	public partial class Autorization : Window
	{
		private AuthorizationController controller;
		/// <summary>
		/// Если true, то происходит переход из формы в форму и приложение закрывать не надо
		/// </summary>
		public bool Transition { get; set; }
		public Autorization()
		{
			InitializeComponent();
			controller = new AuthorizationController(this);
			controller.Model.AutorizationModelChange += Model_AutorizationModelChange;
			SignIn.Click += SignIn_Click;
			this.Closed += Autorization_Closed;

			string login;
			string pass;

			if (AuthorizationData.LoadAuthorizationData(out login, out pass))
			{
				this.Login.Text = login;
				this.Password.Text = pass;
				CheckSaveAccountData.IsChecked = true;
			}
		}

		private void CheckSaveAccountData_Checked(object sender, RoutedEventArgs e)
		{
			if (CheckSaveAccountData.IsChecked == true)
			{
				AuthorizationData.SaveAuthorizationData(this.Login.Text, this.Password.Text);
			}
			else
			{
				AuthorizationData.DeleteAuthorizationData();
			}
		}

		private void SignIn_Click(object sender, RoutedEventArgs e)
		{
			controller.NickName = this.Login.Text;
			controller.Password = this.Password.Text;
			CheckSaveAccountData_Checked(sender, e);

			controller.SignIn();
		}

		private void Autorization_Closed(object sender, EventArgs e)
		{
			if(!Transition)
				Environment.Exit(0);
		}

		private void Model_AutorizationModelChange()
		{
			switch(controller.Model.State)
			{
				case StatesAutorizationModel.IncorrectData:
					MessageBox.Show("Неправильный логин или пароль!");
					break;
				case StatesAutorizationModel.NoAutorization:
					MessageBox.Show("Не удалось подключиться к серверу!");
					break;
			}
		}
	}
}
