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
    /// Логика взаимодействия для Queue.xaml
    /// </summary>
    public partial class Queue : Window
    {
		private QueueContoller contoller;
		private IQueueModelForView model;
		/// <summary>
		/// Если true, то происходит переход из формы в форму и приложение закрывать не надо
		/// </summary>
		public bool transition;
		public Queue(BaseClient<IMessage> client)
        {
            InitializeComponent();
			model = StorageModels.QueueModel;
			model.QueueModelChange += Model_QueueModelChange;
			contoller = new QueueContoller(client, StorageModels.QueueModel);
			
			SignOut.Click += contoller.Handler_SignOutOfQueue;
			this.Closed += Queue_Closed;
        }

		private void Model_QueueModelChange(TypesChangeQueueModel type)
		{
			Dispatcher.Invoke(() =>
			{
				switch (type)
				{
					case TypesChangeQueueModel.CountPlayersInQueue:
						Handler_ChangeCountPlayersInQueue();
						break;
					case TypesChangeQueueModel.State:
						Handler_ChangeState();
						break;
				}
			});
		}

		private void Queue_Closed(object sender, EventArgs e)
		{
			if(!transition)
				Environment.Exit(0);
			contoller.ViewClose();
		}

		private void Handler_ChangeState()
		{
			switch (model.State)
			{
				case StatesQueueModel.ExitedOfQueue:
					Handler_ChangeStateIntoExitedOfQueue();
					break;
				case StatesQueueModel.ErrorConnect:
					Handler_ChangeStateIntoErrorConnect();
					break;
				case StatesQueueModel.SuccessJoinedToBattle:
					Handler_ChangeStateIntoSuccessJoinedToBattle();
					break;
			}
		}

		private void Handler_ChangeStateIntoExitedOfQueue()
		{
			Account formAccount = new Account(contoller.Client);
			formAccount.Show();
			transition = true;
			Close();
		}

		private void Handler_ChangeStateIntoErrorConnect()
		{
			Autorization authorization = new Autorization();
			authorization.Show();
			transition = true;
			Close();
		}

		private void Handler_ChangeStateIntoSuccessJoinedToBattle()
		{
			BattleView3d battleForm = new BattleView3d(model.IDInBattle, contoller.Client);
			battleForm.Show();
			transition = true;
			Close();
		}

		private void Handler_ChangeCountPlayersInQueue()
		{
			Players.Text = model.PlayersInQueue.ToString();
		}
	}
}
