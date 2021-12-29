﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        List<string> requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
            headerLines = new Dictionary<string, string>();
            requestLines = new List<string>();
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            bool Ok =true;
            //TODO: parse the receivedRequest using the \r\n delimeter   
            string[] separatingStrings = { "\r\n" };
            requestLines = requestString.Split(separatingStrings,System.StringSplitOptions.None).ToList<string>();
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (requestLines.Count >= 3)
            {
                /*do nothing*/;
            }
            else
            {
                Console.WriteLine("Bad requset\n");
                return false;
            }
            // Parse Request line
            if (!ParseRequestLine())
            {
                return false;
            }
            // Validate blank line exists
            if (!ValidateBlankLine())
            {
                Console.WriteLine("Bad Requst blank lines");
                return false;
            }
            // Load header lines into HeaderLines dictionary
            LoadHeaderLines();
            return Ok;
        }

        private bool ParseRequestLine()
        {
           // throw new NotImplementedException

            //chheking the all requst line
            string[] reqLine = requestLines[0].Split(' ');
            if (reqLine.Length != 3)
            {
                Console.WriteLine("Bad Requset line");
                return false;
            }

            //checking the method 
            if (RequestMethod.GET.ToString() == reqLine[0])
                method = RequestMethod.GET;
            else if (RequestMethod.HEAD.ToString() == reqLine[0])
                method = RequestMethod.HEAD;
            else if (RequestMethod.POST.ToString() == reqLine[0])
                method = RequestMethod.POST;
            else
            {
                Console.WriteLine("Bad Requst Method");
                return false;
            }

            //checking the uri 
            if (ValidateIsURI(reqLine[1]))
            {

                relativeURI = reqLine[1];
                relativeURI = relativeURI.Remove(0, 1);
            }
            else
            {
                Console.WriteLine("Bad URI \n");
                return false;
            }

            //checking the http ver
            if (reqLine[2] == "HTTP/1.0" || reqLine[2] == "HTTP/1.0\n")
                httpVersion = HTTPVersion.HTTP10;
            else if (reqLine[2] == "HTTP/1.1" || reqLine[2] == "HTTP/1.1\n")
                httpVersion = HTTPVersion.HTTP11;
            else if (reqLine[2] == "HTTP/0.9" || reqLine[2] == "HTTP/0.9\n")
                httpVersion = HTTPVersion.HTTP09;
            else
            {
                Console.WriteLine("Bad Requst HTTP Ver");
                return false;
            }
            return true;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            //string[] each = requestLines[1].Split('\n');
           /* foreach (string s in requestLines)
            {
                string[] tmp = s.Split(':');
                if (tmp.Length < 2)
                    return false;
                headerLines.Add(tmp[0], tmp[1]);
            }*/

            for (int i = 1; i < requestLines.Count-2; i++)
            {
                if(requestLines[i]== "\r")
                {
                    continue;

                }
                else
                {
                    string [] tmp = requestLines[i].Split(':');
                    try
                    {
                        headerLines.Add(tmp[0], tmp[1]);
                    }
                    catch(Exception e)
                    {
                        continue;
                    }
                }
            }
            return true;
        }

        private bool ValidateBlankLine()
        {
            //throw new NotImplementedException();
            string[] tmp = requestString.Split('\r');
            if (tmp.Length >= 2)
                return true;
            else
                return false;

        }

    }
}
