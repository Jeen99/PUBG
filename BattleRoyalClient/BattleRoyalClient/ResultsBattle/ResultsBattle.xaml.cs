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
using CSInteraction.ProgramMessage;

namespace BattleRoyalClient
{
	/// <summary>
	/// Логика взаимодействия для ResultsBattle.xaml
	/// </summary>
	public partial class ResultsBattle : Window
	{
		public ResultsBattle(EndGame msg)
		{
			InitializeComponent();

			//заполняем поля
			if (msg.YouDied)
			{
				Reason.Text = "Вы проиграли!";
				Reason.Foreground = new SolidColorBrush(Colors.Red);
			}
			else
			{
				Reason.Text = "Вы победили!";
				Reason.Foreground = new SolidColorBrush(Colors.Green);
			}

			Kills.Text = msg.Kills.ToString();
			Minutes.Text = msg.TimeLife.Minutes.ToString();
			Seconds.Text = msg.TimeLife.Seconds.ToString();
		}
	}
}
