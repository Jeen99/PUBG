using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommonLibrary;
using CommonLibrary.QueueMessages;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
    class QueueRoyalBattle
    {
        private ObservableCollection<QueueGamer> queueOfGamer;
		/// <summary>
		/// Количество игроков необходимых для создания комнаты
		/// </summary>
		private const int _gamersInRoom = 3;
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
			if (queueOfGamer.Count >= _gamersInRoom && !CreatingRoom)
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
			List<QueueGamer> gamers = new List<QueueGamer>(_gamersInRoom);
			for (int i = 0; i < _gamersInRoom; i++)
			{
				queueOfGamer[i].AddInRoom = true;
				gamers.Add(queueOfGamer[i]);							
			}
			for (int i = 0; i < _gamersInRoom; i++)
			{
				queueOfGamer[0].Client.Controler.Dispose();
				queueOfGamer.RemoveAt(0);				
			}
			Program.RoomsOfRoyaleBattle.AddRoom(gamers);			
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
				for (int i = 0; i < queueOfGamer.Count; i++)
				{
					if (queueOfGamer[i].NickName == gamer.NickName &&
						queueOfGamer[i].Password == gamer.Password)
					{
						queueOfGamer.RemoveAt(i);
						return true;
					}
				}
				
			}
			 return false;
        }
    }
}