using System;
using Microsoft.SPOT;

namespace Webserver
{
    /// <summary>
    /// Debug level for error reporting, INFO means all errors, WARNING only warnings and errors, and ERROR only errors.
    /// </summary>
    public enum DebugLevel
	{
        INFO,
        WARNING,
        ERROR
    }	
    public static class DebugUtils
    {
        public static DebugLevel debugLevel = DebugLevel.INFO;
        public static void Print(DebugLevel lvl, String message)
        {
            switch (lvl)
            {
                case DebugLevel.INFO:
                    message = "INFO:\t\t" + message;
                    break;
                case DebugLevel.WARNING:
                    message = "WARNING:\t" + message;
                    break;
                case DebugLevel.ERROR:
                    message = "ERROR:\t\t" + message;
                    break;
                default:
                    break;
            }

            switch (debugLevel)
            {
                case DebugLevel.INFO:
                    Debug.Print(message);
                    break;
                case DebugLevel.WARNING:
                    if (lvl == DebugLevel.ERROR || lvl == DebugLevel.WARNING)
                    {
                        Debug.Print(message);
                    }
                    break;
                case DebugLevel.ERROR:
                    if (lvl == DebugLevel.ERROR)
                    {
                        Debug.Print(message);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
