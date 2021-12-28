using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//подключение библиотек для работы с сетью и потоками
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace курсовиОчКо
{
    class Klient
    {
        //номер порта для обмена сообщениями
        int port = 8888;
        //ip адрес сервера
        string address = "127.0.0.1";
        //объявление TCP клиента
        TcpClient client = null;
        //объявление канала соединения с сервером
        NetworkStream stream = null;
        //имя пользователя
        string userName;

        public delegate void recieveMessage(string message);
        public recieveMessage recieve;

        public Klient(int port, string adres)
        {
            this.port = port;
            this.address = adres;
        }

        public bool connect(string name)

        {
            //получение имени пользователя
            userName = name;
            try //если возникнет ошибка - переход в catch
            {
                //создание клиента
                client = new TcpClient(address, port);
                //получение канала для обмена сообщениями
                stream = client.GetStream();

                //создание нового потока для ожидания сообщения от сервера
                Thread listenThread = new Thread(() => listen());
                listenThread.Start();

            }
            catch
            {
                return false;
            }
            return true;
        }

        //функция ожидания сообщений от сервера
        void listen()
        {
            try //в случае возникновения ошибки - переход к catch
            {
                //цикл ожидания сообщениями
                while (true)
                {
                    //буфер для получаемых данных
                    byte[] data = new byte[64];
                    //объект для построения смтрок
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    //до тех пор, пока есть данные в потоке
                    do
                    {
                        //получение 64 байт
                        bytes = stream.Read(data, 0, data.Length);
                        //формирование строки
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    //получить строку
                    string message = builder.ToString();
                    //вывод сообщения в лог клиента
                    Application.Current.Dispatcher.Invoke(delegate {
                        recieve(message);
                        });
                    //Dispatcher.BeginInvoke(new Action(() => recieve(message)));
                }
            }
            catch
            {
                //вывести сообщение об ошибке
                //log.Items.Add(ex.Message);
            }
            finally
            {
                //закрыть канал связи и завершить работу клиента
                stream.Close();
                client.Close();
            }
        }

        public void send(string message)
        {
            //добавление имени пользователя к сообщению
            //message = String.Format("{0}: {1}", userName, message);
            //преобразование сообщение в массив байтов
            byte[] data = Encoding.Unicode.GetBytes(message);
            //отправка сообщения
            stream.Write(data, 0, data.Length);
        }

    }
}
