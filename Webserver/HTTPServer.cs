using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;
using Webserver.Extensions;

namespace Webserver
{
    class HTTPServer
    {
        private int port;
        private int timeout;
        private bool dhcp;
        private IPAddress ipAddress;
        private IPEndPoint serverEndpoint;
        private bool stop;
        private int backlog;

        /// <summary>
        /// HTTP Server for Web service requests
        /// </summary>
        /// <param name="port">Port server will be listening on</param>
        /// <param name="timeout">Timeout for requests</param>
        /// <param name="backlog">Number of queued connections the server will accept</param>
        /// <param name="dhcp">Use DHCP for IP?</param>
        /// <param name="ipAddress">IP of webserver, null if DHCP is enabled</param>
        public HTTPServer(int port, int timeout, int backlog, bool dhcp, IPAddress ipAddress)
        {
            this.port = port;
            this.timeout = timeout;
            this.backlog = backlog;
            this.dhcp = dhcp;
            this.ipAddress = ipAddress;
            //create server endpoint with or without dhcp enabled
            if(this.dhcp == true){
                serverEndpoint = new IPEndPoint(IPAddress.Any, this.port);
            }
            else if (this.dhcp == false)
            {
                serverEndpoint = new IPEndPoint(this.ipAddress, this.port);
            }
        }
        /// <summary>
        /// Starts the http server
        /// </summary>
        public void Start()
        {
            stop = false;
            using (Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                server.ReceiveTimeout = timeout;
                server.Bind(serverEndpoint);
                server.Listen(backlog);

                while (!stop)
                {
                    try
                    {
                        using (Socket connection = server.Accept())
                        {
                            if(connection.Poll(-1,SelectMode.SelectRead)){
                                byte[] bytes = new byte[connection.Available];
                                int count = connection.Receive(bytes);
                                DebugUtils.Print(DebugLevel.INFO, "Request received from " + connection.RemoteEndPoint.ToString() + " at " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                connection.SendTimeout = this.timeout;

                                string rawData = new String(Encoding.UTF8.GetChars(bytes));
                                string[] parameters = rawData.Words();

                                DebugUtils.Print(DebugLevel.INFO, "Parameter 1 of received request: " + parameters[0]);
                            }
                        }
                    }
                }

            }
        }
        /// <summary>
        /// stops the http server
        /// </summary>
        public void Stop()
        {
            stop = true;
        }
    }
}
