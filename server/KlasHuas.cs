using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace server
{   
    struct Sclient
        {
            public uint uID;
            public TcpClient klient;
            public NetworkStream stream;
        }

    public class KlasHuas
    {
        //создание объекта для отслеживания сообщений переданных с ip адреса через порт 

        int port = 8888;
        string address = "127.0.0.1";
        static TcpListener listener;
        uint GUID = 0;

        List<Sclient> list = new List<Sclient>();

        public delegate void recieveMessage1(string message);
        public recieveMessage1 recieve1;

        public KlasHuas(int port, string adres)
        {
            this.port = port;
            this.address = adres;
        }

        uint getUID()
        {
            GUID++;
            return GUID;
        }

        void listen()  //функция ожидания и приёма запросов на подключение 
        {
            while (true)
            {
                try
                {
                    //принятие запроса на подключение         
                    TcpClient klient = listener.AcceptTcpClient();

                    //создание нового потока для обслуживания нового клиента 
                    Thread clientThread = new Thread(() => Process(klient));
                    clientThread.Start();
                }
                catch (SocketException ex) when (ex.ErrorCode == 0000)
                {
                    return;
                }
            }
        }

        public void Process(TcpClient tcpClient)  //функция обработки сообщений от клиента 
        {
            TcpClient klient = tcpClient;
            NetworkStream stream1 = null;

            try
            {
                //получение потока для обмена сообщениями         
                stream1 = klient.GetStream();

                Sclient klient1 = new Sclient();
                klient1.klient = klient;
                klient1.stream = stream1;

                // list.Add(client1);

                byte[] data = new byte[64];

                klient1.uID = getUID();
                list.Add(klient1);

                /*Dispatcher.BeginInvoke(new Action(() => log.Items.Add("Новый клиент подключен: " + klient1.uID)))*/;
                Application.Current.Dispatcher.Invoke(delegate {
                    recieve1("Новый клиент подключен");
                });
                while (true)  //цикл ожидания и отправки сообщений 
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    do
                    {
                        //из потока считываются 64 байта и записываются в data начиная с 0
                        bytes = klient1.stream.Read(data, 0, data.Length);

                        //из считанных данных формируется строка              
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }

                    while (stream1.DataAvailable);
                    //преобразование сообщения    
                    string message = builder.ToString();

                    //Dispatcher.BeginInvoke(new Action(() => log.Items.Add(message)));
                    Application.Current.Dispatcher.Invoke(delegate {
                        recieve1(message);
                    });

                    if (message == "#dsct")
                    {
                        //log.Items.Add("Пользователь: " + klient1.uID + "отключился от сервера");
                        klient1.stream.Close();
                        klient1.klient.Close();
                        list.Remove(klient1);
                        //Dispatcher.BeginInvoke(new Action(() => log.Items.Add("Клиент отключился")));
                        Application.Current.Dispatcher.Invoke(delegate {
                            recieve1("клиент отключился");
                        });
                        //Application.Current.Dispatcher.Invoke(delegate {
                        //    recieve1(ex.Message);
                        break;
                    }

                    data = Encoding.Unicode.GetBytes(message);

                    foreach (Sclient sc in list)
                        //отправка сообщения обратно клиенту
                        if (sc.uID != klient1.uID) sc.stream.Write(data, 0, data.Length);

                }
            }
            catch (Exception ex)
            //если возникла ошибка, вывести сообщение об ошибке     
            {
                //Dispatcher.BeginInvoke(new Action(() => log.Items.Add(ex.Message)));
                Application.Current.Dispatcher.Invoke(delegate {
                    recieve1(ex.Message);
                });
            }
            finally
            //после выхода из бесконечного цикла   
            {
                //освобождение ресурсов при завершении сеанса 
                string StopMsg = "#gohome";
                byte[] data1 = new byte[StopMsg.Length];
                data1 = Encoding.Unicode.GetBytes(StopMsg);




                if (stream1 != null)
                    stream1.Close();
                if (klient != null)
                    klient.Close();

            }
        }

        public void Srart()
        {
            recieve1("Сервер запущен");
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            //начало прослушивания 
            listener.Start();
            //создание нового потока для ожидания и подключения клиентов 
            Thread listenThread = new Thread(() => listen());
            listenThread.Start();
        }


    }
}
