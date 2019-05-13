using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace UniWebServer
{
    [RequireComponent(typeof(EmbeddedWebServerComponent))]
    public class FileServer : MonoBehaviour, IWebResource
    {
        public string folderPath = "/hextris";
        EmbeddedWebServerComponent server;

        void Start()
        {
            server = GetComponent<EmbeddedWebServerComponent>();
            server.AddResource(folderPath, this);
        }

        public void HandleRequest(Request request, Response response)
        {
            // check if file exist at folder (need to assume a base local root)
            string folderRoot = Application.streamingAssetsPath;
            string fullPath = folderRoot + Uri.UnescapeDataString(request.uri.LocalPath);
            // get file extension to add to header
            string fileExt = Path.GetExtension(fullPath);
            // not found
            if (!File.Exists(fullPath)) {
                response.statusCode = 404;
                response.message = "Not Found";
                return;
            }

            // serve the file
            response.statusCode = 200;
            response.message = "OK";
            response.headers.Add("Content-Type", MimeTypeMap.GetMimeType(fileExt));

            // read file and set bytes
            using (FileStream fs = File.OpenRead(fullPath))
            {
                int length = (int)fs.Length;
                byte[] buffer;

                // add content length
                response.headers.Add("Content-Length", length.ToString());

                // use binary for mostly all except text
                using (BinaryReader br = new BinaryReader(fs))
                {
                    buffer = br.ReadBytes(length);
                }
                response.SetBytes(buffer);

            }
        }

    }
}