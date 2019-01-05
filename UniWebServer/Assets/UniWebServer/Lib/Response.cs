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
        public bool useBytes = false;
        public byte[] dataBytes;
        public Headers headers;
        public MemoryStream stream;
        public StreamWriter writer;


        public Response()
        {
            headers = new Headers();
            stream = new MemoryStream();
            writer = new StreamWriter(stream);
        }

        public void SetBytes(byte[] data)
        {
            useBytes = true;
            dataBytes = data;
        }

        public void Write(string text)
        {
            writer.Write(text);
            writer.Flush();
        }
    }

}