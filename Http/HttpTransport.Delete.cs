using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameFramework
{
    public partial class HttpTransport
    {
        private IEnumerator DeleteRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Delete(uri))
            {
                www.downloadHandler = new DownloadHandlerBuffer();

                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }

        private IEnumerator DeleteRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Delete(uri))
            {
                www.downloadHandler = new DownloadHandlerBuffer();

                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }
    }
}
