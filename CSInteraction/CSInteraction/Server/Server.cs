using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace CSInteraction.Server
{
     public class Server
        {
            public string IPAdress { get; private set; }
            public int Port { get; private set; }
            private IControler BaseControler { get; set; }
            public List<ServerClient> ConnectedClients { get; private set; }

            private TcpListener ServerPoint;
            private Thread ThreadCheckNewClient;
            //конструктор
            public Server(string ipAdress, int port, IControler baseControler)
            {
                if (baseControler != null)
                {
                    IPAdress = ipAdress;
                    Port = port;
                    BaseControler = baseControler;
                    ConnectedClients = new List<ServerClient>();
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
                            ServerClient Client = new ServerClient(ServerPoint.AcceptTcpClient(), BaseControler);
                            Client.EventEndSession += HandlerEndSessionClient;
                            ConnectedClients.Add(Client);
                        }
                        else throw new Exception("Не указана сслыка на базоый обработчик сообщений от клиента");
                    }
                    else Thread.Sleep(500);
                }
            }

            private void HandlerEndSessionClient(ServerClient client)
            {
                ConnectedClients.Remove(client);
                client = null;
            }
        }
    
}
