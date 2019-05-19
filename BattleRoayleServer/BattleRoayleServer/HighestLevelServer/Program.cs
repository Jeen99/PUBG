using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Server;
using CommonLibrary;

namespace BattleRoayleServer
{
    class Program
    {
        public static Server<IMessage> Server { get; private set; }
        public static QueueRoyalBattle QueueOfServer { get; private set; }
        public static Rooms RoomsOfRoyaleBattle { get; private set; }

#if DEBUG
		public static int MAX_PLAYERS_IN_ROOM = 1;
		public static int COUNT_PLAYERS_FOR_DISPOSE_ROOM = 1;
#endif

		static void Main(string[] args)
        {
#if DEBUG
			Console.WriteLine("Кол-во игроков в комнате? Игроков для освобождения комнаты?");
			string [] inputParam = Console.ReadLine().Split(' ');

			if (inputParam.Length == 2)
			{
				MAX_PLAYERS_IN_ROOM = Convert.ToInt32(inputParam[0]);
				COUNT_PLAYERS_FOR_DISPOSE_ROOM = Convert.ToInt32(inputParam[1]);
			}

			AutomaticLoadServer();
			QueueOfServer = new QueueRoyalBattle(MAX_PLAYERS_IN_ROOM);
#else
			//создаем сервер обрабатывающий запросы на подключение и первичную обработку сообщений от пользователей
			switch (StartMessage())
            {
                case 1:
                    AutomaticLoadServer();
                    break;
                case 2:
                    UserLoadServer();
                    break;
            }
			//создаем очередь, обрабатывающая игроков, ожидающих боя 
			QueueOfServer = new QueueRoyalBattle();
#endif
			//создаем класс, который будет хранит все игровые комнаты, активные в данный момент
			RoomsOfRoyaleBattle = new Rooms();
        }
        static private int StartMessage()
        {
            for (;;)
            {
                Console.WriteLine("Выберите тип загружки сервера: ");
                Console.WriteLine("1. Локальный сервер, порт 11000");
                Console.WriteLine("2. Свои значения IP адрес и порта");
                Console.Write("Введите ваш вариант ответа: ");
                int Result;
                if (int.TryParse(Console.ReadLine(), out Result))
                {
                        if (Result == 1 || Result == 2)
                        {
                            return Result;
                        }
                        Console.WriteLine("Ошибка ввода значения!");
                }
                Console.WriteLine();
            }
        }

        static private void AutomaticLoadServer()
        {
            Server = new Server<IMessage>("127.0.0.1", 11000, new AuthorizationController());
            if (Server.StartServer())
            {
                Console.WriteLine("Сервер запущен");
            }
            else
            {
                Console.WriteLine("Ошибка запуска сервера");
            }
        }
        static private void UserLoadServer()
        {
            Console.WriteLine();
            for (;;)
            {
                Console.Write("Введиет IP-адрес хоста: ");
                string ipAdress = Console.ReadLine();
                Console.Write("Введиет порт сервера: ");
                int portServer;

                if (!int.TryParse(Console.ReadLine(), out portServer))
                {
                    Console.WriteLine("Введены неправильные данные!");
                    continue;
                }

                Server = new Server<IMessage>(ipAdress, portServer, new AuthorizationController());

                if (Server.StartServer())
                {
                    Console.WriteLine("Сервер запущен");
                    break;
                }
                else
                {
                    Console.WriteLine("Введены неправильные данные!");
                }

            }
        }
    }
}
