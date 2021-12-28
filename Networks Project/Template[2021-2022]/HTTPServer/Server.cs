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
           // Socket serverSocket = this.serverSocket.Accept();
            this.LoadRedirectionRules(redirectionMatrixPath);
            this.portNumber = portNumber;
            //Initialize serverSocket object and bind it to local host
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(hostEndPoint);

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


            //string welcome = "Welcome to my test server";
            //byte[] data = Encoding.ASCII.GetBytes(welcome);
            //clientSocket.Send(data);
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
                     message = Encoding.ASCII.GetString(requestData,0, 	receivedLength );
                  
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
            throw new NotImplementedException();
            string status;
            int code;
            string content;
            try
            {
                //TODO: check for bad request 

                if(!request.ParseRequest())
                {
                     
                     content =LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                     Response res = new Response(StatusCode.BadRequest, "text.html", content, string.Empty);
                     return res;

                }

               //TODO: map the relativeURI in request to get the physical path of the resource.
                string phPass = Path.Combine(Configuration.RootPath, request.relativeURI);


                 //TODO: check for redirect
                 string relativePath = GetRedirectionPagePathIFExist(phPass);
                if(relativePath.Length!=0)
                {
                    content = LoadDefaultPage(Configuration.RedirectionDefaultPageName);

                    Response respon=new Response(StatusCode.Redirect, "text.htm",content ,relativePath);

                    return respon;
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
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            for (int i = 0; i < Configuration.RedirectionRules.Count; i++)
            {
                if (Configuration.RedirectionRules.Keys.ElementAt(i).ToString() == relativePath)
                {
                     return Configuration.RedirectionRules.Values.ElementAt(i).ToString();
                }
            }
            
            return string.Empty;
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


                 
                // then fill Configuration.RedirectionRules dictionary 
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
