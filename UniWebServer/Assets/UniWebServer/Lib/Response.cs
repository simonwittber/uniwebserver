using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace UniWebServer
{

    public class Response
    {
        public int statusCode = 404;
        public string message = "Not Found";
        public Headers headers;
        public MemoryStream stream;
        public StreamWriter writer;

        public Response ()
        {
            stream = new MemoryStream();
            writer = new StreamWriter (stream);
        }

        public void Write(string text) {
            writer.Write(text);
            writer.Flush();
        }
    }

}