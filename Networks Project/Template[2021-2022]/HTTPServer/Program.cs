using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;


namespace HTTPServer
{

    class Program
    {

        static void Main(string[] args)
        {
           
            

            //---------------------------testing-----------------------------------------
           /* string s = Console.ReadLine() ;
            string s = "POST /test/demo_form.php HTTP/1.1\r\nHost: w3schools.comname1=value1&name2=value2";
            Request t = new Request(s);
            t.ParseRequest();

            while(true)
                Console.ReadLine();
           */
            //--------------------------------------------------------------------------
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();

           
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "redirectionRules.txt";

            //Start server
            // 1) Make server object on port 1000
            Server server =new Server(1000,filePath);
            // 2) Start Server
            server.StartServer();


            //closing the log file to save the logs
            //Logger.sr.Close();
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt


            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
        }
         
    }
}
