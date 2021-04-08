using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameFramework
{
    public partial class HttpTransport
    {
        private IEnumerator GetRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Get(uri))
            {
                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }

        private IEnumerator GetRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Get(uri))
            {
                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }
    }
}
