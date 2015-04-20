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

        public HTTPRequest(Type type, HTTPHeader header, string body)
        {
            //for httpheader class?
            //if (httpVersion == null) { httpVersion = ""; }
            //if (url == null) { url = ""; }
            //if (contentType == null) { contentType = ""; }
            //if (connection == null) { connection = ""; }
            //if (httpBody == null) { httpBody = ""; }

        }
    }
}
