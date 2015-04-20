using System;
using Microsoft.SPOT;
using System.Threading;
using System.Net;

namespace Webserver
{
    class Webserver
    {
        public Webserver()
        {
            UpdateTime();
            StartHTTPServer();
        }

        #region Private Methods
        private void UpdateTime()
        {

        }
        private void StartHTTPServer()
        {
            HTTPServer httpServer = new HTTPServer(80, 10000, 1000, false, IPAddress.Parse("192.168.0.144"), IPAddress.Parse("255.255.255.0"), IPAddress.Parse("192.168.0.106"));
            httpServer.Start();
        }
        #endregion
    }
}
