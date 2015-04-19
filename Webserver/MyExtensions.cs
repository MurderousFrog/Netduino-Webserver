using System;
using Microsoft.SPOT;

namespace Webserver
{
    public static class Extensions
    {        
        public static string[] Words(this String str)
        {
            return str.Split(new char[] { ' ', '\n' });
        }
    } 
}
