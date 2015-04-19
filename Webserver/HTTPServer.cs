using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Webserver
{
    class HTTPServer
    {
        private int port;
        private int timeout;
        private bool dhcp;
        private IPAddress ipAddress;
        private IPAddress subnetMask;
        private IPAddress gateway;
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
        /// <param name="ipAddress">IP of webserver, not used if DHCP is enabled</param>
        /// <param name="subnetMask">Subnet mask of webserver, not used if DHCP is enabled</param>
        /// <param name="gateway">Gateway of webserver, not used if DHCP is enabled</param>
        public HTTPServer(int port, int timeout, int backlog, bool dhcp, IPAddress ipAddress, IPAddress subnetMask, IPAddress gateway)
        {
            this.port = port;
            this.timeout = timeout;
            this.backlog = backlog;
            this.dhcp = dhcp;
            this.ipAddress = ipAddress;
            this.subnetMask = subnetMask;
            this.gateway = gateway;

            //create server endpoint with or without dhcp enabled
            if(this.dhcp == true){
                serverEndpoint = new IPEndPoint(IPAddress.Any, this.port);
            }
            else if (this.dhcp == false)
            {
                NetworkInterface ni = NetworkInterface.GetAllNetworkInterfaces()[0];
                ni.EnableStaticIP(this.ipAddress.ToString(), this.subnetMask.ToString(), this.gateway.ToString());
                serverEndpoint = new IPEndPoint(this.ipAddress, this.port);
            }
        }
        /// <summary>
        /// Starts the http server
        /// </summary>
        public void Start()
        {
            stop = false;
            try
            {
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

                                    //DebugUtils.Print(DebugLevel.INFO, "Parameter 1 of received request: " + parameters[0]);

                                    string header = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nConnection: close\r\n\r\n";

                                    connection.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
                                    
                                    //TODO: Event handler for incoming requests
                                }
                            }
                        }
                        catch (Exception e){
                            DebugUtils.Print(DebugLevel.ERROR, "An exception occured while accepting client socket connections.");
                            DebugUtils.Print(DebugLevel.ERROR, e.ToString());
                        }
                    }

                }

            }
            catch (Exception e)
            {
                DebugUtils.Print(DebugLevel.ERROR, "An exception occured while trying to initialize listener socket.");
                DebugUtils.Print(DebugLevel.ERROR, e.ToString());
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
