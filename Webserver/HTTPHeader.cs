using System;
using Microsoft.SPOT;
using System.Collections;
using System.Text;

namespace Webserver
{
    class HTTPHeader
    {
        
        
        ArrayList list;

        /// <summary>
        /// Create a new HTTP header
        /// </summary>
        public HTTPHeader()
        {
            list = new ArrayList();
        }

        /// <summary>
        /// Add a header field to the HTTP Header
        /// </summary>
        /// <param name="header">Header name</param>
        /// <param name="value">Header value</param>
        public void Add(string header, string value){
            Pair p = new Pair(header, value);
            list.Add(p);
        }

        /// <summary>
        /// Remove a header from list
        /// </summary>
        /// <param name="header">Header name</param>
        public void Remove(string header)
        {
            list.Remove(GetObject(header));
        }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (Pair p in list)
            {
                s.Append(p.Key() + ": " + p.Value() + "\r\n");
            }
            s.Append("\r\n");
            return s.ToString();
        }
        public static HTTPHeader Parse(string rawHeader){
            if (rawHeader == null) { return null; }
            HTTPHeader header = new HTTPHeader();
            try
            {
                //Split raw data into lines
                string[] lines = rawHeader.Split(new char[] { '\n' });

                for (int i = 0; i < lines.Length; i++)
                {

                    if (lines[i] != null && lines[i].IndexOf(':') > -1)
                    {
                        //Remove line breaks and carriage returns
                        lines[i] = lines[i].Replace("\n", "").Replace("\r", "");
                        //Split at all ':', first substring is the header name
                        string headerName = lines[i].Split(new char[] { ':' })[0];
                        //Substring after ':' is headerValue
                        string headerValue = lines[i].Substring(lines[i].IndexOf(':') + 2);

                        //Add header to list
                        header.Add(headerName, headerValue);
                    }
                }
            }
            catch (Exception e)
            {
                DebugUtils.Print(DebugLevel.ERROR, "Failed to parse raw data into HTTP header.");
                DebugUtils.Print(DebugLevel.ERROR, e.StackTrace);
            }
            return header;
        }
        private Object GetObject(string id)
        {
            foreach (Pair p in list)
            {
                if (p.Key() == id) { return p; }
            }
            return null;
        }
    }
    class Pair
    {
        string key;
        string value;

        public Pair(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        public string Key() { return key; }
        public string Value() { return value; }
    }
}
