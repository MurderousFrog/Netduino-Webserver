using System;
using Microsoft.SPOT;
using System.Threading;
using System.Net;

namespace Webserver
{
    class Webserver
    {
        private Thread httpServerThread = null;
        public Webserver()
        {
            this.httpServerThread = new Thread(StartHTTPServer);
            try
            {
                httpServerThread.Start();
            }
            catch
            {
                DebugUtils.Print(DebugLevel.ERROR, "Starting HTTP Server Thread failed.");
            }
            
        }

        private void StartHTTPServer()
        {
            DebugUtils.Print(DebugLevel.INFO, "Starting HTTP Webserver in thread " + httpServerThread.GetHashCode().ToString());
            HTTPServer httpServer = new HTTPServer(80,10000, 1000, false, IPAddress.Parse("192.168.0.144"), IPAddress.Parse("255.255.255.0"),IPAddress.Parse("192.168.0.106"));
            httpServer.Start();
        }
    }
}
