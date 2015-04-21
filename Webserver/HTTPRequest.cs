using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Webserver
{
    class HTTPRequest
    {
        public enum Type
        {
            GET, HEAD, POST, PUT, DELETE, TRACE, CONNECT
        }
        private Type type;
        private string url;
        private string version;
        private HTTPHeader header;
        private string body;


        public HTTPRequest(Type type, string url, string version, HTTPHeader header, string body)
        {
            this.type = type;
            this.url = url;
            this.version = version;
            this.header = header;
            this.body = body;
        }

        /// <summary>
        /// Parse raw http request data into usable HTTPRequest form
        /// </summary>
        /// <param name="rawData">Raw HTTP request data</param>
        public static HTTPRequest Parse(string rawData)
        {
            if (rawData != null)
            {
                HTTPHeader header = HTTPHeader.Parse(rawData);
                //split raw data into lines
                string[] lines = rawData.Split(new char[] { '\n' });

                for (int i = 0; i < lines.Length; i++)
                {
                    //remove line breaks and carriage returns
                    lines[i] = lines[i].Replace("\n", "").Replace("\r", "");
                }

                //take first line, split at all spaces, first string should be method
                string method = lines[0].Words()[0];
                //choose corelating method
                Type extractedType = Type.DELETE;
                switch (method)
                {
                    case "GET":
                        extractedType = Type.GET;
                        break;
                    case "HEAD":
                        extractedType = Type.HEAD;
                        break;
                    case "POST":
                        extractedType = Type.POST;
                        break;
                    case "PUT":
                        extractedType = Type.PUT;
                        break;
                    case "DELETE":
                        extractedType = Type.DELETE;
                        break;
                    case "TRACE":
                        extractedType = Type.TRACE;
                        break;
                    case "CONNECT":
                        extractedType = Type.CONNECT;
                        break;
                    default:
                        break;
                }
                //take first line, split at all spaces, second string should be URL.
                string url = lines[0].Words()[1];
                //http version is everything after HTTP/
                string version = lines[0].Substring(lines[0].IndexOf("HTTP/") + 5);
                string body = rawData.Substring(rawData.IndexOf("\r\n\r\n") + 4);
                return new HTTPRequest(extractedType, url, version, header, body);
            }
            return null;


        }
        public override string ToString()
        {
            StringBuilder request = new StringBuilder();
            string httpType = "";
            switch (type)
            {
                case Type.GET:
                    httpType = "GET";
                    break;
                case Type.HEAD:
                    httpType = "HEAD";
                    break;
                case Type.POST:
                    httpType = "POST";
                    break;
                case Type.PUT:
                    httpType = "PUT";
                    break;
                case Type.DELETE:
                    httpType = "DELETE";
                    break;
                case Type.TRACE:
                    httpType = "TRACE";
                    break;
                case Type.CONNECT:
                    httpType = "CONNECT";
                    break;
                default:
                    break;
            }
            request.Append(httpType + " ");
            request.Append(url + " ");
            request.Append("HTTP/" + version + "\r\n");
            request.Append(header.ToString());
            request.Append(body + "\r\n\r\n");

            return request.ToString();
        }
        public byte[] GetHeaderAsBytes()
        {
            return Encoding.UTF8.GetBytes(header.ToString());
        }
        public byte[] GetBodyAsByte()
        {
            return Encoding.UTF8.GetBytes(body);
        }
    }
}
