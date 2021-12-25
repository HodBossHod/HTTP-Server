using System;
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
            //throw new NotImplementedException();
            bool Ok =true;
            bool pars =true , reqLin = true,ver = true,balnk = true;
            //TODO: parse the receivedRequest using the \r\n delimeter   
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            pars = ParseRequestLine();
            if (pars == false)
                return false;
            // Parse Request line
            string[] reqLine = requestLines[0].Split(' ');
            if (reqLine.Length != 3)
                return false;

            if (reqLine[0].ToLower() == "get")
                method = RequestMethod.GET;
            else if (reqLine[0].ToLower() == "head")
                method = RequestMethod.HEAD;
            else if (reqLine[0].ToLower() == "post")
                method = RequestMethod.POST;
            else
                return false;

            reqLin = ValidateIsURI(reqLine[1]);

            if (reqLine[2] == "HTTP/1.0" || reqLine[2] == "HTTP/1.0\n")
                httpVersion = HTTPVersion.HTTP10;
            else if (reqLine[2] == "HTTP/1.1" || reqLine[2] == "HTTP/1.1\n")
                httpVersion = HTTPVersion.HTTP11;
            else if (reqLine[2] == "HTTP/0.9" || reqLine[2] == "HTTP/0.9\n")
                httpVersion = HTTPVersion.HTTP09;
            else
                return false;


            // Validate blank line exists
            balnk = ValidateBlankLine();
            // Load header lines into HeaderLines dictionary
            LoadHeaderLines(requestLines[1]);
            if (pars && reqLin && ver)
            {
                /*foreach (string a in requestLines)
                {
                    Console.WriteLine(a);
                    Console.WriteLine("==========================\r");
                }
                foreach (string b in reqLine)
                {
                    Console.WriteLine(b);
                    Console.WriteLine("==========================\r");
                }
                Console.WriteLine("the heder dictionary contant is : \n");
                foreach (var item in headerLines)
                {
                    Console.WriteLine("key :{0} ---- val :{1}", item.Key, item.Value);

                }

                Console.WriteLine(Ok);*/
                return Ok;

            }
            else
                return !Ok;
                
        }

        private bool ParseRequestLine()
        {
           // throw new NotImplementedException
            string[] separatingStrings = { "\r\n" };
            requestLines = requestString.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            requestLines.Add("\r");
            if (requestLines.Count == 3 || requestLines.Count == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines(string heder)
        {
            //throw new NotImplementedException();
            string[] each = heder.Split('\n');

            foreach(string s in each)
            {
                string[] tmp = s.Split(':');
                if (tmp.Length < 2)
                    return false;
                headerLines.Add(tmp[0], tmp[1]);
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
