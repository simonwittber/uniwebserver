using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System;

namespace UniWebServer
{
    public class Request
    {
        public string method, path, protocol, query, fragment;
        public Uri uri;
        public Headers headers = new Headers ();
        public string body;
        public NetworkStream stream;
        public Dictionary<string, MultiPartEntry> formData = null;

        public void Write (Response response)
        {
            StreamWriter writer = new StreamWriter (stream);
            headers.Set("Connection", "Close");
            headers.Set("Content-Length", response.stream.Length);
            writer.Write ("HTTP/1.1 {0} {1}\r\n{2}\r\n\r\n", response.statusCode, response.message, response.headers);
            response.stream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader (response.stream);
            writer.Write(reader.ReadToEnd());
            writer.Flush();
        }

        public void Close ()
        {
		if (stream != null) {
			stream.Close();
		}
        }

        public override string ToString ()
        {
            return string.Format ("{0} {1} {2}\r\n{3}\r\n", method, path, protocol, headers);
        }
    }

}
