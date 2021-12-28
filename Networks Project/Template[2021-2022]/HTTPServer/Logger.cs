using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static int noExp = 1;
        public static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
            
            DateTime now = DateTime.Now;
            string date = now.ToString("h:mm tt");
            sr.WriteLine("Exption Number {0} : Happin in {1}  --> {2} \r\n",noExp , date , ex.Message);
            noExp++;
            sr.Flush();
            

        }
    }
}
