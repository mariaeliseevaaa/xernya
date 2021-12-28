using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace курсовиОчКо
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Klient klient = new Klient(8888, "127.0.0.1");
        History history = new History();
        DispatcherTimer timer = new DispatcherTimer();
        int ch = DateTime.Now.Hour, min = DateTime.Now.Minute, k = DateTime.Now.Second;
        public MainWindow()
        {
            
            InitializeComponent();

           

            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick); 
            
            timer.Start();

            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ch = DateTime.Now.Hour; min = DateTime.Now.Minute; k = DateTime.Now.Second;
            vremya.Content = ch.ToString() + " : " + min.ToString();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //подключиться
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<saveFormat> m = save.load1();
            if (m != null)
            foreach (saveFormat l in m)
            {
                SmsOCHKA sms = new SmsOCHKA(l.text);
                history.addsms(sms, l.type);
               grd.Children.Add(sms.gettext());
            }
            

            bool rezult = klient.connect("Me");

            klient.recieve = recieveMessage;

            if (rezult == false)
                MessageBox.Show("Соединение не установлено");
        }
        
        private void prokrutka_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double sdvig= e.OldValue - e.NewValue;
            if (e.OldValue  < e.NewValue)
            {
                history.sdvig2(sdvig);
            }
            else
            {
                history.sdvig2(sdvig );
            }
           
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void vvod_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            save.save1(history.getMessages());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SmsOCHKA sms = new SmsOCHKA(vvod.Text);
            
            history.addsms(sms, MessageType.My);
            grd.Children.Add(sms.gettext());

            klient.send(vvod.Text);

            vvod.Clear();

            prokrutka.Maximum += 1;

            //для проигрывания звука из класса ZvukHuuk
            ZvukHuuk zvukHuuk = new ZvukHuuk();
            zvukHuuk.Zvuk();

            save.save1(history.getMessages());
            
        }

        void recieveMessage(string message)
        {
            SmsOCHKA sms = new SmsOCHKA(message);
            vvod.Clear();
            history.addsms(sms, MessageType.Other);
            grd.Children.Add(sms.gettext());

            //для проигрывания звука из класса ZvukHuuk
            ZvukHuuk zvukHuuk = new ZvukHuuk();
            zvukHuuk.Zvuk();
        }

    }
}
