using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

namespace UniWebServer
{
    public class Request
    {
        public string method, path, protocol, query, fragment;
        public Uri uri;
        public Headers headers = new Headers();
        public string body;
        public NetworkStream stream;
        public Dictionary<string, MultiPartEntry> formData = null;

        public void Write(Response response)
        {
            if (response.useBytes)
            {
                WriteBytes(response);
            }
            else
            {
                WriteText(response);
            }
        }

        private void WriteBytes(Response response)
        {
            BinaryWriter binWriter = new BinaryWriter(stream);
            headers.Set("Connection", "Close");
            headers.Set("Content-Length", response.stream.Length);
            // ===== Super tricky here =====
            // Note:
            // 1. The header string using BinaryWriter has 1 less line \r\n 
            // than when using StreamWriter
            // 2. If we don't convert string to bytes array and write directly
            // it works on other browsers, not Safari
            string headerStr = string.Format("HTTP/1.1 {0} {1}\r\n{2}\r\n",
                                             response.statusCode,
                                             response.message,
                                             response.headers);
            byte[] headerBytes = Encoding.ASCII.GetBytes(headerStr);
            binWriter.Write(headerBytes);

            // write response
            binWriter.Write(response.dataBytes);
            binWriter.Flush();
        }

        private void WriteText(Response response)
        {
            StreamWriter writer = new StreamWriter(stream);
            headers.Set("Connection", "Close");
            headers.Set("Content-Length", response.stream.Length);
            writer.Write("HTTP/1.1 {0} {1}\r\n{2}\r\n\r\n", response.statusCode,
                          response.message, response.headers);
            response.stream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(response.stream);
            writer.Write(reader.ReadToEnd());
            writer.Flush();
        }

        public void Close()
        {
            if (stream != null)
            {
                stream.Close();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}\r\n{3}\r\n", method, path, protocol, headers);
        }
    }

}
