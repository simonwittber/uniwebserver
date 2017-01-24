using UnityEngine;
using System.Collections;


namespace UniWebServer
{
	public interface IWebResource
	{
        void HandleRequest(Request request, Response response);
	}

}