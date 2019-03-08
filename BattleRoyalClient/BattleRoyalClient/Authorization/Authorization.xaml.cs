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
			SignIn.Click += controller.SignIn;
			Login.TextChanged += Login_TextChanged;
			Password.TextChanged += Password_TextChanged;
			this.Closed += Autorization_Closed;
		}

		private void Autorization_Closed(object sender, EventArgs e)
		{
			if(!Transition)
			Environment.Exit(0);
		}

		private void Password_TextChanged(object sender, TextChangedEventArgs e)
		{
			controller.Password = Password.Text;
		}

		private void Login_TextChanged(object sender, TextChangedEventArgs e)
		{
			controller.NickName = Login.Text;
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
