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

        static void Main(string[] args)
        {
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
