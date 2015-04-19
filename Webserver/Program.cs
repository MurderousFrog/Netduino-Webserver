using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Webserver
{
    public class Program
    {
        public static void Main()
        {
            //setting the error reporting level
            DebugUtils.debugLevel = DebugLevel.INFO;

            DebugUtils.Print(DebugLevel.INFO, "Starting Webserver class");
            Webserver webserver = new Webserver();
        }

    }
}
