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
            string[] lines = message.Split(new char[] { '\n' });
            string print = null;
            foreach (string line in lines)
            {
                string mLine = line.Replace("\n","").Replace("\r", "");

                switch (lvl)
                {
                    case DebugLevel.INFO:
                        print = "INFO:\t\t" + mLine;
                        break;
                    case DebugLevel.WARNING:
                        print = "WARNING:\t" + mLine;
                        break;
                    case DebugLevel.ERROR:
                        print = "ERROR:\t\t" + mLine;
                        break;
                    default:
                        break;
                }

                switch (debugLevel)
                {
                    case DebugLevel.INFO:
                        Debug.Print(print);
                        break;
                    case DebugLevel.WARNING:
                        if (lvl == DebugLevel.ERROR || lvl == DebugLevel.WARNING)
                        {
                            Debug.Print(print);
                        }
                        break;
                    case DebugLevel.ERROR:
                        if (lvl == DebugLevel.ERROR)
                        {
                            Debug.Print(print);
                        }
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}
