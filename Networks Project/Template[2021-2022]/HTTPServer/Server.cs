using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;
        int portNumber;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket
            this.portNumber = portNumber;
            //Initialize serverSocket object and bind it to local host
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(hostEndPoint);
            Console.WriteLine("listing...");

        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(1000);

            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.

                Socket clientSocket = this.serverSocket.Accept();
                Console.WriteLine("New client accepted: {0}", clientSocket.RemoteEndPoint);
                Thread newthread = new Thread(new ParameterizedThreadStart (HandleConnection));

                newthread.Start(clientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            Socket clientSocket = (Socket)obj;

            clientSocket.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            int receivedLength;

            while (true)
            {
                try
                {
                    // TODO: Receive request
                    string message = "";
                    byte[] requestData = new byte[65536];
                  
                    receivedLength = clientSocket.Receive(requestData);

                    // TODO: break the while loop if receivedLen==0
                    if (receivedLength == 0)
                    {
                        Console.WriteLine("Client: {0} ended the connection", clientSocket.RemoteEndPoint);
                        break;
                    }

                    // TODO: Create a Request object using received request string
                     message = Encoding.ASCII.GetString(requestData,0, receivedLength );
                  
                     Request request = new Request((message));
                    // TODO: Call HandleRequest Method that returns the response
                    Response response = HandleRequest(request);

                    string ressponseString = response.ResponseString;

                    // TODO: Send Response back to client
                    byte[] respnseByte = Encoding.ASCII.GetBytes(ressponseString);
                    clientSocket.Send(respnseByte);

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
 
                }
            }

            // TODO: close client socket
            clientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            
            string content;
            try
            {
                //TODO: check for bad request 

                if(!request.ParseRequest())
                {    
                     content =LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                     Response res = new Response(StatusCode.BadRequest, "text/html", content, string.Empty);
                     return res;
                }

               //TODO: map the relativeURI in request to get the physical path of the resource.
                string phPass = Configuration.RootPath +"\\"+ request.relativeURI;


                 //TODO: check for redirect


                string redir = GetRedirectionPagePathIFExist(request.relativeURI);
                    if(redir!=null)
                    {
                        phPass = Configuration.RootPath + "\\"+redir;
                        Response rerespon = new Response(StatusCode.Redirect, "text/html", phPass, redir);
                        return rerespon;
                    }
        
                //TODO: check file exists
                if (!File.Exists(phPass))
                {
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    Response notfoundResponse=new Response(StatusCode.NotFound,"text/html",content,string.Empty);
                    return notfoundResponse;
                }

                //TODO: read the physical file
                content = File.ReadAllText(phPass);

                // Create OK response
                Response okResponse = new Response(StatusCode.OK, "text/html", content, string.Empty);
                return okResponse;
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                // TODO: in case of exception, return Internal Server Error. 
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                Response re=new Response(StatusCode.InternalServerError,"text/html",content,string.Empty);
                Logger.LogException(ex);
                return re;
            }
        }
        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            LoadRedirectionRules(@"C:\inetpub\wwwroot\fcis1\redirectionRules.txt");
            foreach (var item in Configuration.RedirectionRules)
            {
                if (item.Key == relativePath)
                    return item.Value;
            }
            
            return null;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                // else read file and return its content
                return string.Empty;
            }
        }

        private void LoadRedirectionRules(string filePath)

        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 

                string fileContent = File.ReadAllText(filePath);
                string [] s=fileContent.Split(',');
                Configuration.RedirectionRules.Add(s[0], s[1]);


                 
                // then fill Configuration.RedirectionRules dictionary 
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                //Environment.Exit(1);
            }
        }
    }
}
