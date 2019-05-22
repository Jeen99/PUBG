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
using CommonLibrary;


namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для Account.xaml
	/// </summary>
	public partial class Account : Window
	{
		private AccountController controller;
		private IAccountModelForView model;

		/// <summary>
		/// Если true, то происходит переход из формы в форму и приложение закрывать не надо
		/// </summary>
		public bool transition;

		public Account(BaseClient<IMessage> client)
		{
			BaseInitialize(client);
		}

		private void BaseInitialize(BaseClient<IMessage> client)
		{
			InitializeComponent();
			model = StorageModels.AccountModel;
			controller = new AccountController(client, StorageModels.AccountModel);
			InQueue.Click += controller.InQueue;
			model.AccountModelChange += Model_AutorizationModelChange; ;
			this.Closed += Account_Closed;
		}

		private void Model_AutorizationModelChange(TypesChangeAccountModel type)
		{
			Dispatcher.Invoke(() =>
			{
				switch (type)
				{
					case TypesChangeAccountModel.Data:
						Handler_ChangeData();
						break;
					case TypesChangeAccountModel.State:
						Handle_ChangeState();
						break;
				}
			});
		}

		private void Handler_ChangeData()
		{
			Kills.Text = model.Kills.ToString();
			Deaths.Text = model.Deaths.ToString();
			Battles.Text = model.Battles.ToString();
			Days.Text = model.GameTime.Days.ToString();
			Hours.Text = model.GameTime.Hours.ToString();
			Minutes.Text = model.GameTime.Minutes.ToString();
			Seconds.Text = model.GameTime.Seconds.ToString();
		}

		private void Handle_ChangeState()
		{
			switch (model.State)
			{
				case StatesAccountModel.ErrorConnect:
					MessageBox.Show("Потеря соединения с сервером");
					Autorization authorization = new Autorization();
					authorization.Show();
					transition = true;
					Close();
					break;
				case StatesAccountModel.SuccessJoinedToQueue:
					Queue formQueue = new Queue(controller.Client);
					formQueue.Show();
					transition = true;
					Close();
					break;
			}
		}

		public Account(BaseClient<IMessage> client, IMessage results)
		{
			BaseInitialize(client);
			//отображаем форму с итогами битвы
			ResultsBattle formStatistics = new ResultsBattle(results);
			formStatistics.ShowDialog();
		}

		private void Account_Closed(object sender, EventArgs e)
		{
			if (!transition)
			{
				Environment.Exit(0);
			}
			controller.ViewIsClose();
		}
	}
}
