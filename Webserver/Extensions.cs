using System;
using Microsoft.SPOT;

namespace Webserver
{
    public static class Extensions
    {        
        /// <summary>
        /// returns # of "words", aka strings seperated by space or linebreaks
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] Words(this String str)
        {
            return str.Split(new char[] { ' ', '\n' });
        }
    } 
}
