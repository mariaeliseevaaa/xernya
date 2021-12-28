using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace курсовиОчКо
{
    public enum MessageType { My, Other };
    public class History
    {
        public Vector pos1;
        public Vector pos2;

        public double otstup;
        public List<SmsOCHKA> history;

        public History()
        {
            history = new List<SmsOCHKA>();
            pos1 = new Vector(195, 337);
            pos2 = new Vector(10, 337);
            
            otstup = 20;
        }

        public void addsms(SmsOCHKA smska,/*SmsOCHKA smska1,*/ MessageType type)
        {
            if (type == MessageType.My)
            {
                smska.setPosition(pos1);
       
            }
            if (type == MessageType.Other)
            {
                smska.setPosition(pos2);
               
            }
            smska.type = type;

            for (int i = history.Count - 2; i >= 0; i--)
            {
                history[i+1].sdvig(smska.visota + otstup);
            }
            history.Add(smska);

        }
            //public void sdvig1(double shift)
            //{
            //    foreach(SmsOCHKA smska in history)
            //    { 
                  
            //        smska.sdvig(shift * smska.visota - otstup);
            //    }
             
            //}
            public void sdvig2(double shift)
            {
                foreach (SmsOCHKA smska in history)
                {
                
                    smska.sdvig(shift /* smska.visota + otstup*/);
                }
            }

        public List<saveFormat> getMessages()
        {
            List<saveFormat> msgs = new List<saveFormat>();

            foreach (SmsOCHKA sms in history)
                msgs.Add(sms.getMsg());

            return msgs;
        }

    }
}
