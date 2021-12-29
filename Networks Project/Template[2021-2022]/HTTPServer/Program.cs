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
           
            
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();

           
            string filePath = @"C:\inetpub\wwwroot\fcis1\redirectionRules.txt";

            //Start server
            // 1) Make server object on port 1000
            Server server =new Server(1000,filePath);
            // 2) Start Server
            server.StartServer();
            //closing the log file to save the logs
            Logger.sr.Close();
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            string filePath = @"C:\inetpub\wwwroot\fcis1\redirectionRules.txt";
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileStream fs = File.Create(filePath);
            StreamWriter f=new StreamWriter(fs);


            // each line in the file specify a redirection rule
            f.WriteLine(@"aboutus.html,aboutus2.html");
            f.Close();
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
        }
         
    }
}
