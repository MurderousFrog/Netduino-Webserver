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
            bool updatedTime = NTP.UpdateTimeFromNtpServer("pool.ntp.org", 1);
            StartHTTPServer();
        }

        #region Private Methods
        private void StartHTTPServer()
        {
            HTTPServer httpServer = new HTTPServer(80, 10000, 1000, false, IPAddress.Parse("192.168.0.144"), IPAddress.Parse("255.255.255.0"), IPAddress.Parse("192.168.0.254"));
            httpServer.Start();
        }
        #endregion
    }
}
