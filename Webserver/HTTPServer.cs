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
        #region Variables

        private int port;
        private int timeout;
        private bool dhcp;
        private IPAddress ipAddress;
        private IPAddress subnetMask;
        private IPAddress gateway;
        private IPEndPoint serverEndpoint;
        private bool stop;
        private bool running;
        private int backlog;

        private Thread httpServerThread = null;

        #endregion

        #region Constructor
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
        #endregion

        #region Events

        /// <summary>
        /// Delegate for the CommandReceived event.
        /// </summary>
        public delegate void GetRequestHandler(object obj, WebServerEventArgs e);
        public class WebServerEventArgs : EventArgs
        {
            public WebServerEventArgs(Socket response, string rawData)
            {
                this.response = response;
                this.rawData = rawData;
            }
            public Socket response { get; protected set; }
            public string rawData { get; protected set; }

        }


        /// <summary>
        /// CommandReceived event is triggered when a valid command (plus parameters) is received.
        /// Valid commands are defined in the AllowedCommands property.
        /// </summary>
        public event GetRequestHandler CommandReceived;

        #endregion

        #region Public and private methods

        /// <summary>
        /// Method is called when a new client request is received
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void ProcessClientRequest(object obj, HTTPServer.WebServerEventArgs e)
        {
            DebugUtils.Print(DebugLevel.INFO, "Received a request in ProcessClientGetRequest method:" + e.rawData);
            
        }
        /// <summary>
        /// Starts the http server thread
        /// </summary>
        public void Start()
        {
            running = true;
            httpServerThread = new Thread(StartServer);
            CommandReceived += new GetRequestHandler(ProcessClientRequest);
            try
            {
                stop = false;
                httpServerThread.Start();
            }
            catch (Exception)
            {
                DebugUtils.Print(DebugLevel.ERROR, "Starting HTTP Server Thread failed.");
                stop = true;
                running = false;
            }
        }
        private void StartServer()
        {
            DebugUtils.Print(DebugLevel.INFO, "Starting HTTP Webserver in thread " + httpServerThread.GetHashCode().ToString());
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
                                if (connection.Poll(-1, SelectMode.SelectRead))
                                {
                                    byte[] bytes = new byte[connection.Available];
                                    int count = connection.Receive(bytes);
                                    DebugUtils.Print(DebugLevel.INFO, "Request received from " + connection.RemoteEndPoint.ToString() + " at " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                                    connection.SendTimeout = this.timeout;

                                    string rawData = new String(Encoding.UTF8.GetChars(bytes));
                                    //string[] parameters = rawData.Words();

                                    string header = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nConnection: close\r\n\r\n";
                                    string body = "Hello World!";
                                    string data = header + body;

                                    connection.Send(Encoding.UTF8.GetBytes(data), data.Length, SocketFlags.None);

                                    if (CommandReceived != null)
                                    {
                                        CommandReceived(this, new WebServerEventArgs(connection, rawData));
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
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
        /// Restarts the HTTP Server
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }
        /// <summary>
        /// stops the http server
        /// </summary>
        public void Stop()
        {
            stop = true;
            Thread.Sleep(100);
            httpServerThread.Suspend();
            DebugUtils.Print(DebugLevel.WARNING, "Stopped HTTP server in thread: ");
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                httpServerThread = null;
            }
        }
        #endregion
    }
}
