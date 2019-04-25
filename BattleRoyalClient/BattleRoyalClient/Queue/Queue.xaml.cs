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
		/// <summary>
		/// Если true, то происходит переход из формы в форму и приложение закрывать не надо
		/// </summary>
		public bool Transition { get; set; }
		public Queue(BaseClient<IMessage> client)
        {
            InitializeComponent();
			contoller = new QueueContoller(client, this);
			contoller.Model.QueueModelChange += Model_QueueModelChange;
			SignOut.Click += contoller.Handler_SignOutOfQueue;
			this.Closed += Queue_Closed;
        }

		private void Queue_Closed(object sender, EventArgs e)
		{
			if(!Transition)
			Environment.Exit(0);
		}

		private void Model_QueueModelChange()
		{
			Players.Text = contoller.Model.PlaysersInQueue.ToString();
		}
	}
}
