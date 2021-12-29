using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Globalization;

namespace курсовиОчКо
{
    public class SmsOCHKA
    {
         TextBlock smska;
       
        
         TextBlock vremya;
         public string text { get; }
        //string text1;
        Border ram;
        //Border ram1;
         Vector position;
        //Vector posi;
         StackPanel spisok;
        //public System.Windows.TextWrapping TextWrapping { get; set; };
    

        //smska.TextWrapping();
        //smska.

        public MessageType type;

        public double visota { get; }

        public SmsOCHKA(string text /*string text1*/)
        {
            this.text = text;
            smska = new TextBlock();
            //smska1 = new TextBlock();
            ram = new Border();
            //ram1 = new Border();

            spisok = new StackPanel();

            int pFrom = text.IndexOf("*")+1;
            int pTo = text.LastIndexOf("*");
            if (pTo > -1)
            {
                String result = text.Substring(pFrom, pTo - pFrom);
                InlineUIContainer container = Smailiki.getSmailik(result.ToString(), 40);
                smska.Inlines.Add(container);
                //smska1.Inlines.Add(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            }
            else
            {
                 smska.Inlines.Add(text);
                 //smska1.Inlines.Add(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            }


            smska.Background = Brushes.LightPink;
            ram.BorderThickness = new Thickness(1);
            ram.BorderBrush = Brushes.DeepPink;
            ram.RenderTransform = new TranslateTransform(10, 0);
            ram.Width = 200;
           
            //visota = 40;

            smska.TextWrapping = TextWrapping.Wrap;
            smska.Arrange(new Rect(0, 0, 200, 1000));
            visota = smska.DesiredSize.Height;

            //ram.Arrange(new Rect(0, 0, 100, 1000));
            ram.Height = smska.DesiredSize.Height;

            //ram.Height = smska.ActualHeight;//40 * (text.Length / 40.0);
            //visota = smska.ActualHeight;//40 * (text.Length / 40.0);

            //if (text.Length > 40)
            //{
            //    visota = 40 * (text.Length / 40);
            //    ram.Height = visota;

            //}


            ram.HorizontalAlignment = HorizontalAlignment.Left;
            ram.VerticalAlignment = VerticalAlignment.Top;
            ram.Child = smska;

            vremya = new TextBlock();

            vremya.Text = DateTime.Now.ToString("t", CultureInfo.CreateSpecificCulture("es-ES"));

            spisok.Children.Add(ram);
            spisok.Children.Add(vremya);
            
            
        }

        public StackPanel gettext()
        {
            return spisok;
        }

        public void setPosition(Vector pos /*Vector pos1*/)
        {
            position = pos;

            ram.RenderTransform = new TranslateTransform(position.X-10, position.Y-(visota));
            //ram.RenderTransform = new TranslateTransform(position.X - 10, position.Y);
            vremya.RenderTransform = new TranslateTransform(position.X + 80, position.Y- (visota ));
        }

        public void sdvig(double sd)
        {
            position.Y -= sd;

            ram.RenderTransform = new TranslateTransform(position.X-10, position.Y -( visota ));
            //ram.RenderTransform = new TranslateTransform(position.X, position.Y);
            vremya.RenderTransform = new TranslateTransform(position.X+80, position.Y - (visota ));
            
        }

        public saveFormat getMsg()
        {
            saveFormat sf = new saveFormat();

            sf.text = text;
            sf.type = type;

            return sf;
        }
    }
}
