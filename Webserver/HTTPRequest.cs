using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Webserver
{
    class HTTPRequest
    {
        public enum Type
        {
            GET, HEAD, POST, PUT, DELETE, TRACE, CONNECT
        }
        private Type type;
        private string url;
        private HTTPHeader header;
        private string body;


        public HTTPRequest(Type type, HTTPHeader header, string body)
        {
            this.type = type;
            this.header = header;
            this.body = body;
        }

        /// <summary>
        /// Parse raw http request data into usable HTTPRequest form
        /// </summary>
        /// <param name="rawData">Raw HTTP request data</param>
        public static HTTPRequest Parse(string rawData)
        {
            //split raw data into lines
            string[] lines = rawData.Split(new char[] { '\n' });

            foreach (string line in lines)
            {
                //remove line breaks and carriage returns
                string mLine = line.Replace("\n", "").Replace("\r", "");
            }
            
            //take first line, split at all spaces, first string should be method
            string method = lines[0].Split(new char[] { ' ' })[0];
            //choose corelating method
            Type extractedType; 
            switch (method)
            {
                case "GET":

                 break;
                case "HEAD":
                 break;
                case "POST":
                 break;
                case "PUT":
                 break;
                case "DELETE":
                 break;
                case "TRACE":
                 break;
                case "CONNECT":
                 break;
                default:
                 break;
            }
            //take first line, split at all spaces, second string should be URL.
            string url = lines[0].Split(new char[] { ' ' })[1];


            return new HTTPRequest();

        }
        public string ToString()
        {
            return header.ToString() + body.ToString();
        }
        public byte[] GetHeaderAsBytes(){
            return Encoding.UTF8.GetBytes(header.ToString());
        }
        public byte[] GetBodyAsByte(){
            return Encoding.UTF8.GetBytes(body);
        }
    }
}
