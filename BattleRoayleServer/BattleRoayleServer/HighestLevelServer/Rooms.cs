using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace BattleRoayleServer
{
    public class Rooms
    {
        ObservableCollection<IRoom> rooms;

        public Rooms()
        {
            rooms = new ObservableCollection<IRoom>();
        }

		//выполнеят действия по созданию комнаты
        public void AddRoom(List<QueueGamer> gamers)
        {
			rooms.Add(new RoyalRoom(gamers));
        }

        public void DeliteRoom(IRoom room )
        {
			
			if(rooms.Remove(room))
			{
				room.Dispose();
			}
        }
    }
}