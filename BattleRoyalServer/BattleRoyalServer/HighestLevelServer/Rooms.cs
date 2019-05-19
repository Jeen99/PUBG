using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace BattleRoyalServer
{
    public class Rooms
    {
        public ObservableCollection<IRoom> CollectionRooms { get; private set; }

        public Rooms()
        {
			CollectionRooms = new ObservableCollection<IRoom>();
        }

		//выполнеят действия по созданию комнаты
        public void AddRoom(List<QueueGamer> gamers)
        {
			IRoom room = new RoyalRoom(gamers);
			CollectionRooms.Add(room);
			room.StartRoom();
			room.EventRoomEndWork += DeliteRoom;
        }

		public void DeliteRoom(IRoom room)
        {		
			if(CollectionRooms.Remove(room))
			{
				room.Dispose();
				Log.AddNewRecord("Произошло завершение работы игровой комнаты");
			}
			
        }
    }
}