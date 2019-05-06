using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using ObservalableExtended;
using System.Threading.Tasks;
using CSInteraction.Server;


namespace CSInteraction.Client
{
    public class BaseClient<T>:IController<T>
    {
        public string IPAdress { get; private set; }
        public int Port { get; private set; }
        public StatusClient Status { get; private set; }
		private ConnectedClient<T> client;

		//уведомляет о получении нового сообщения от сервера
		public event NewMessage EventNewMessage;
        public event EndSession EventEndSession;
		
		//конструктор
		public BaseClient(string ipAdress, int port)
        {
            IPAdress = ipAdress;
            Port = port;
            Status = StatusClient.Initialize;
		}

        //закрывает подлючение
        public void Close()
        {
			if (client != null)
			{
				client?.Close();
				client.EventEndSession -= Handler_EndSession;
				Status = StatusClient.EndSession;
			}
			else throw new Exception("Невозможно выполнить операцию до создания подключения");	
		}

        //подключаемся к серверу
        public void ConnectToServer()
        {
			if (client == null)
			{
				try
				{
					TcpClient newConnection = new TcpClient();
					newConnection.Connect(IPAdress, Port);
					client = new ConnectedClient<T>(newConnection, this);
					client.EventEndSession += Handler_EndSession;
					Status = StatusClient.Connect;
				}
				catch (Exception e)
				{
					Status = StatusClient.FailConect;
				}

			}
        }
        //отправляет сообщение серверу
        public void SendMessage(T msg)
        {
			if(client!=null)
				client?.SendMessage(msg);
			else throw new Exception("Невозможно выполнить операцию до создания подключения");
		}

		public T GetRecievedMsg()
		{
			if (client != null)
				return client.GetRecievedMsg();
			else throw new Exception("Невозможно выполнить операцию до создания подключения");
		}

		public int GetCountReceivedMsg()
		{
			return client.GetCountReceivedMsg();
		}

		void IController<T>.Hanlder_NewMessage()
		{
			EventNewMessage?.Invoke();
		}

		IController<T> IController<T>.GetNewControler(ConnectedClient<T> client)
		{
			return this;
		}

		public void Dispose()
		{
			Close();
		}

		private void Handler_EndSession(ConnectedClient<T> Client)
		{
			EventEndSession?.Invoke();
		}
	}

    public delegate void NewMessage();
    public delegate void EndSession();

    public enum StatusClient
    {
        Initialize,
        Connect,
        FailConect,
        EndSession
	}

}
