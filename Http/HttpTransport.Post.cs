using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameFramework
{
    public partial class HttpTransport
    {
        private IEnumerator PostRoutine(string uri, string postData, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Post(uri, postData))
            {
                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }

        private IEnumerator PostRoutine(string uri, string postData, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Post(uri, postData))
            {
                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }
    }
}
