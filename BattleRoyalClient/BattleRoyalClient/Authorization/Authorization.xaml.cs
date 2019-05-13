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
		private IAuthorizationModerForView model = StorageModels.AutorizationModel;
		/// <summary>
		/// Если true, то происходит переход из формы в форму и приложение закрывать не надо
		/// </summary>
		public bool transition = false;

		public Autorization()
		{
			InitializeComponent();
			controller = new AuthorizationController(StorageModels.AutorizationModel);
			model.AutorizationModelChange += Model_AutorizationModelChange; ;
			SignIn.Click += SignIn_Click;
			Closed += Autorization_Closed;

			controller.ViewIsLoad();
		}

		private void Model_AutorizationModelChange(TypesChangeAutorizationModel type)
		{
			Dispatcher.Invoke(() =>
			{
				switch (type)
				{
					case TypesChangeAutorizationModel.NickName:
						ChangeNickName();
						break;
					case TypesChangeAutorizationModel.Password:
						ChangePassword();
						break;
					case TypesChangeAutorizationModel.State:
						ChangeState();
						break;
					case TypesChangeAutorizationModel.SaveAutorizationData:
						ChangeSaveAutorizationData();
						break;
					case TypesChangeAutorizationModel.All:
						ChangeNickName();
						ChangePassword();
						ChangeSaveAutorizationData();
						break;
				}
			});
		}

		private void ChangeNickName()
		{
			this.Login.Text = model.NickName;
		}

		private void ChangePassword()
		{
			this.Password.Text = model.Password;
		}

		private void ChangeSaveAutorizationData()
		{
			CheckSaveAccountData.IsChecked = model.SaveAutorizationData;
		}

		private void ChangeState()
		{
			switch (model.State)
			{
				case StatesAutorizationModel.IncorrectData:
					MessageBox.Show("Неправильный логин или пароль!");
					break;
				case StatesAutorizationModel.ErrorConnect:
					MessageBox.Show("Не удалось подключиться к серверу!");
					break;
				case StatesAutorizationModel.SuccessAutorization:			
					Account account = new Account(controller.Client);
					account.Show();
					transition = true;
					Close();
					break;
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
			controller.ChangeNickName(this.Login.Text);
			controller.ChangePassword(this.Password.Text);
			CheckSaveAccountData_Checked(sender, e);

			controller.SignIn();
		}

		private void Autorization_Closed(object sender, EventArgs e)
		{
			if (!transition)
				Environment.Exit(0);

			controller.ViewIsClose();
		}
	}
}
