using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CSInteraction.ProgramMessage;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
    class QueueRoyalBattle
    {
        private ObservableCollection<QueueGamer> queueOfGamer;
		/// <summary>
		/// Количество игроков необходимых для создания комнаты
		/// </summary>
		private const int _GamersInRoom = 1;
		/// <summary>
		/// true = идет процесс создания комнаты
		/// </summary>
		private bool CreatingRoom = false;

        public QueueRoyalBattle()
        {
			queueOfGamer = new ObservableCollection<QueueGamer>();
			queueOfGamer.CollectionChanged += QueueOfGamer_CollectionChanged;
		}
		/// <summary>
		/// Обрабатывает сообщение об измение количества игроков в очереди
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QueueOfGamer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//уведомляет игроков очереди об измение их числа в ней  
			foreach (QueueGamer gamer in queueOfGamer)
			{
				if (!gamer.AddInRoom)
				{
					gamer.Client.SendMessage(new ChangeCountPlayersInQueue(queueOfGamer.Count));
				}
			}
			//проверяем количество игроков в очереди
			if (queueOfGamer.Count >= _GamersInRoom && CreatingRoom)
			{
				//создаем новую комнату в отдельном потоке
				Task.Run(() => CreateNewRoom());
			}
		}
		/// <summary>
		/// Метод создающий новую комнату и удаляющий игроков из очереди
		/// </summary>
		private void CreateNewRoom()
		{
			CreatingRoom = true;
			List<QueueGamer> gamers = new List<QueueGamer>(_GamersInRoom);
			for (int i = 0; i < _GamersInRoom; i++)
			{
				queueOfGamer[0].AddInRoom = true;
				gamers.Add(queueOfGamer[0]);							
			}
			for (int i = 0; i < _GamersInRoom; i++)
			{
				queueOfGamer.RemoveAt(0);
			}
			CreatingRoom = false;

			//проверяем - хватает ли клиентов для создания еще одной комнаты
			QueueOfGamer_CollectionChanged(null, null);
		}
		/// <summary>
		/// Добавляет игрока в очередь
		/// </summary>
		/// <param name="gamer"></param>
		public void AddGamer(QueueGamer gamer)
        {
			queueOfGamer.Add(gamer);
        }

		/// <summary>
		/// Удаляет игрока из очереди
		/// </summary>
		/// <param name="gamer"></param>
		/// <returns></returns>
        public bool DeleteOfQueue(QueueGamer gamer)
        {
			if (!gamer.AddInRoom)
			{
				return queueOfGamer.Remove(gamer);
			}
			else return false;
        }
    }
}