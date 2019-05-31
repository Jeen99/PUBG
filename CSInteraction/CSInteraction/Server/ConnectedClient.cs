using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using ObservalableExtended;

namespace CSInteraction.Server
{
    public class ConnectedClient<T>
    {
        private TcpClient streamConnection;
        private BinaryFormatter formatter;
        private Thread threadOfHandlerMsg;
        public IController<T> Controler { get; set; }
		private object sendMsgSinch = new object();
		
		//уведомляет о получении нового сообщения от клиента
		public event EndSession EventEndSession;

        public ConnectedClient(TcpClient client, IController<T> baseControler)
        {
            streamConnection = client;
            formatter = new BinaryFormatter();
            //создаем обработчик сообшений от пользователя
            Controler = baseControler.GetNewControler(this);
			//обработка сообщений производитсва в отдельном потоке
			threadOfHandlerMsg = new Thread(StartReadMessage);
            threadOfHandlerMsg.Start();
        }

        //закрывает подлючение
        public void Close()
        {
            //отправляем клиенту сообщение об окончании сессии
            byte[] EndMsg = CreateTitleMessage((byte)InsideTypesMessage.EndSession, 0);
            try
            {
                if (streamConnection.Connected)
                {
                    streamConnection.GetStream().Write(EndMsg, 0, EndMsg.Length);
                }
            }
            catch (Exception)
            {
                //пропускаем, если уже невозможно отправить сообщение
            }
            //закрываем соединение и освобождаем ресурсы
            HandlerEndSession();
        }

        //отправляет сообщение серверу
        public void SendMessage(T msg)
        {
            try
            {
                if (streamConnection.Connected)
                {
                    byte[] BytesMsg = null;
                    using (MemoryStream TempStream = new MemoryStream())
                    {
                        formatter.Serialize(TempStream, msg);
                        BytesMsg = TempStream.ToArray();
                    }
                    //создаем и отправляем заголовк сообщения
                    byte[] TitleMsg = CreateTitleMessage((byte)InsideTypesMessage.ProgramMessage, BytesMsg.Length);

                    lock (sendMsgSinch)
                    {
						streamConnection.GetStream().Write(TitleMsg, 0, TitleMsg.Length);
						streamConnection.GetStream().Write(BytesMsg, 0, BytesMsg.Length);
					}
                }
            }
			catch (IOException ex)
			{
				EventEndSession?.Invoke(this);
			}
        }

        //соединяет тип сообщения и его длинну в один массив
        private byte[] CreateTitleMessage(byte type, int Length)
        {
            //конвертируем длинну сообщения в байты
            byte[] BytesLenMsg = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Length));
            //создаем заголовочное сообщение
            byte[] TitleMessage = new byte[BytesLenMsg.Length + 1];
            TitleMessage[0] = type;
            for (int i = 0; i < BytesLenMsg.Length; i++)
            {
                TitleMessage[i + 1] = BytesLenMsg[i];
            }
            return TitleMessage;
        }

        //начинает обработку сообщений поступающих от клиента
        private void StartReadMessage()
        {
			if (streamConnection.Connected)
			{
				NetworkStream StreamOfClient = streamConnection.GetStream();
				while (streamConnection.Connected && threadOfHandlerMsg.ThreadState == ThreadState.Running)
				{
					byte[] TitleMsg = new byte[5];
					//если пришло сообщение от сервера
					if (ReadData(TitleMsg, 5, StreamOfClient) > 0)
					{
						//определяем тип сообщения
						switch ((InsideTypesMessage)TitleMsg[0])
						{
							case InsideTypesMessage.ProgramMessage:
								//вызываем функцию для обработки сообщения от пользователя
								HandlerProgramMessage(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(TitleMsg, 1)), StreamOfClient);
								break;
							case InsideTypesMessage.EndSession:
								//вызов функции обрабатывающей завершения соединения
								HandlerEndSession();
								break;
						}
					}
				}
			}
        }

        //считывает некоторое количество байт из потока и записывает их в массив байт
        private int ReadData(byte[] data, int length, NetworkStream stream)
        {
            try
            {
                int ReadBytes = 0;
                while (ReadBytes != length)
                {
                    int readed = stream.Read(data, ReadBytes, length - ReadBytes);
                    ReadBytes += readed;
                    if (readed == 0) return 0;
                }
                return ReadBytes;
            }
            catch (Exception)
            {
                EventEndSession?.Invoke(this);
                return 0;
            }
        }

        //обрабатывает завершение соединение
        private void HandlerEndSession()
        {
            threadOfHandlerMsg.Abort();
            streamConnection.Close();
            //уведомляем о завершении соединения
            EventEndSession?.Invoke(this);
        }

        //обрабатывает программные сообщения
        private void HandlerProgramMessage(int length, NetworkStream stream)
        {
            //читаем сообщение от сервера
            byte[] Msg = new byte[length];
            //считываем сообщение
            ReadData(Msg, length, stream);
            //десериализуем сообщение
            T ObjectMsg;
            using (MemoryStream MemStream = new MemoryStream())
            {
                MemStream.Write(Msg, 0, Msg.Length);
                MemStream.Seek(0, SeekOrigin.Begin);
                ObjectMsg = (T)formatter.Deserialize(MemStream);
            }
			//генерируем событие
			Controler?.Hanlder_NewMessage(ObjectMsg);
        }

		public delegate void EndSession(ConnectedClient<T> Client);
	}
}
