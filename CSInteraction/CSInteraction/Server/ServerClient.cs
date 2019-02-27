using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using System.Threading.Tasks;

namespace CSInteraction.Server
{
    public delegate void EndSession(ServerClient Client);

    public class ServerClient
    {
        private TcpClient Client;
        private BinaryFormatter formatter;
        private Thread ThreadOfHandlerMsg;
        public IController Controler { get; set; }
        private Mutex SendMsgSinch = new Mutex();
        //уведомляет о получении нового сообщения от клиента
        public event EndSession EventEndSession;

        public ServerClient(TcpClient client, IController baseControler)
        {
            Client = client;
            formatter = new BinaryFormatter();
            //создаем обработчик сообшений от пользователя
            Controler = baseControler.GetNewControler(this);
            //обработка сообщений производитсва в отдельном потоке
            ThreadOfHandlerMsg = new Thread(StartReadMessage);
            ThreadOfHandlerMsg.Start();
        }

        //закрывает подлючение
        public void Close()
        {
            //отправляем клиенту сообщение об окончании сессии
            byte[] EndMsg = CreateTitleMessage((byte)InsideTypesMessage.EndSession, 0);
            try
            {
                if (Client.Connected)
                {
                    Client.GetStream().Write(EndMsg, 0, EndMsg.Length);
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
        public bool SendMessage(IMessage msg)
        {
            try
            {
                SendMsgSinch.WaitOne();
                if (Client.Connected)
                {

                    byte[] BytesMsg = null;
                    using (MemoryStream TempStream = new MemoryStream())
                    {
                        formatter.Serialize(TempStream, msg);
                        BytesMsg = TempStream.ToArray();
                    }
                    //создаем и отправляем заголовк сообщения
                    byte[] TitleMsg = CreateTitleMessage((byte)InsideTypesMessage.ProgramMessage, BytesMsg.Length);
                    try
                    {
                        Client.GetStream().Write(TitleMsg, 0, TitleMsg.Length);
                        Client.GetStream().Write(BytesMsg, 0, BytesMsg.Length);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    return true;
                }
                else return false;
            }
            finally
            {
                SendMsgSinch.ReleaseMutex();
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
            NetworkStream StreamOfClient = Client.GetStream();
            while (Client.Connected && ThreadOfHandlerMsg.ThreadState == ThreadState.Running)
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

        //считывает некоторое количество байт из потока и записывает их в массив байт
        private int ReadData(byte[] data, int length, NetworkStream stream)
        {
            try
            {
                int ReadBytes = 0;
                while (ReadBytes != length)
                {
                    int readed = stream.Read(data, 0, length - ReadBytes);
                    ReadBytes += readed;
                    if (readed == 0) return 0;
                }
                return ReadBytes;
            }
            catch (Exception)
            {
                EventEndSession(this);
                return 0;
            }
        }

        //обрабатывает завершение соединение
        public void HandlerEndSession()
        {
            ThreadOfHandlerMsg.Abort();
            ThreadOfHandlerMsg = null;
            Client.Close();
            //уведомляем о завершении соединения
            EventEndSession(this);
        }

        //обрабатывает программные сообщения
        public void HandlerProgramMessage(int length, NetworkStream stream)
        {
            //читаем сообщение от сервера
            byte[] Msg = new byte[length];
            //считываем сообщение
            ReadData(Msg, length, stream);
            //десериализуем сообщение
            IMessage ObjectMsg;
            using (MemoryStream MemStream = new MemoryStream())
            {
                MemStream.Write(Msg, 0, Msg.Length);
                MemStream.Seek(0, SeekOrigin.Begin);
                ObjectMsg = (IMessage)formatter.Deserialize(MemStream);
            }
            //генерируем событие
            Task.Run(() => Controler.HanlderNewMessage(ObjectMsg));
        }
    }
}
