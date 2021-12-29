using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static int no_of_exceptions = 1;
        public static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            DateTime now = DateTime.Now;
            string date = now.ToString("h:mm tt");
            //message:
            Console.WriteLine("Exption Number {0} : occurred on {1}  --> {2} \r\n", no_of_exceptions, date, ex.Message);
            // for each exception write its details associated with datetime 
            no_of_exceptions++;
            sr.Flush();
        }
    }
}
