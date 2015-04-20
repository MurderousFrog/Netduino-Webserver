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

        private HTTPHeader header;
        private string body;

        /// <summary>
        /// Converts raw http request data into usable HTTPRequest form
        /// </summary>
        /// <param name="rawData">Raw HTTP request data</param>
        public HTTPRequest(string rawData)
        {
            string[] lines = rawData.Split(new char[] { '\n' });
        }
        public HTTPRequest(Type type, HTTPHeader header, string body)
        {
            this.header = header;
            this.body = body;

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
