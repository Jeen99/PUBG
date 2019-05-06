using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace CSInteraction.Server
{
     public class Server<T>
        {
            public string IPAdress { get; private set; }
            public int Port { get; private set; }
            private IController<T> BaseControler { get; set; }
            public List<ConnectedClient<T>> ConnectedClients { get; private set; }

            private TcpListener ServerPoint;
            private Thread ThreadCheckNewClient;
            //конструктор
            public Server(string ipAdress, int port, IController<T> baseControler)
            {
                if (baseControler != null)
                {
                    IPAdress = ipAdress;
                    Port = port;
                    BaseControler = baseControler;
                    ConnectedClients = new List<ConnectedClient<T>>();
                }
                else throw new Exception("Не передано исключения по умолчанию");
            }

            //запускает сервер
            public bool StartServer()
            {
                try
                {
                    ServerPoint = new TcpListener(IPAddress.Parse(IPAdress), Port);
                    ServerPoint.Start();
                }
                catch (Exception)
                {
                    return false;
                }
                //запускаем поток обработки подлючений
                ThreadCheckNewClient = new Thread(CheckerNewClient);
                ThreadCheckNewClient.Start();
                return true;
            }

            private void CheckerNewClient()
            {
                while (ThreadCheckNewClient.ThreadState == ThreadState.Running)
                {
                    if (ServerPoint.Pending())
                    {
                        if (BaseControler != null)
                        {
                            //добавляем нового клиента
                            ConnectedClient<T> Client = new ConnectedClient<T>(ServerPoint.AcceptTcpClient(), BaseControler);
                            Client.EventEndSession += HandlerEndSessionClient;
                            ConnectedClients.Add(Client);
                        }
                        else throw new Exception("Не указана сслыка на базоый обработчик сообщений от клиента");
                    }
                    else Thread.Sleep(500);
                }
            }

            private void HandlerEndSessionClient(ConnectedClient<T> client)
            {
                ConnectedClients.Remove(client);
				client.EventEndSession -= HandlerEndSessionClient;
            }
        }
    
}
