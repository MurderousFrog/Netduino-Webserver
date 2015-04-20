using System;
using Microsoft.SPOT;
using System.Collections;
using System.Text;

namespace Webserver
{
    class HTTPHeader
    {
        
        
        ArrayList list;

        /// <summary>
        /// Create a new HTTP header
        /// </summary>
        public HTTPHeader()
        {
            list = new ArrayList();
        }
        /// <summary>
        /// Add a header field to the HTTP Header
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        public void Add(string key, string value){
            Pair p = new Pair(key, value);
            list.Add(p);
        }
        public void Remove(string id)
        {
            list.Remove(GetObject(id));
        }
        public string ToString()
        {
            //TODO
            StringBuilder s = new StringBuilder();
            foreach (Pair p in list)
            {
                s.AppendLine(p.Key() + ": " + p.Value() + "\n");
            }
            return s.ToString();
        }
        private Object GetObject(string id)
        {
            foreach (Pair p in list)
            {
                if (p.Key() == id) { return p; }
            }
            return null;
        }
    }
    private class Pair
    {
        string key;
        string value;

        public Pair(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        public string Key() { return key; }
        public string Value() { return value; }
    }
}
