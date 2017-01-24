using UnityEngine;
using System.Collections;


namespace UniWebServer
{
    [RequireComponent(typeof(EmbeddedWebServerComponent))]
    public class FileUpload : MonoBehaviour, IWebResource
    {
        public string path = "/upload";
        public TextAsset html;

        EmbeddedWebServerComponent server;

        void Start ()
        {
            server = GetComponent<EmbeddedWebServerComponent>();
            server.AddResource(path, this);
        }
	
        public void HandleRequest (Request request, Response response)
        {
            response.statusCode = 200;
			response.message = "OK.";
            response.Write(html.text);
        }

    }
}