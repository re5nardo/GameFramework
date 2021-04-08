using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameFramework
{
    public partial class HttpTransport
    {
        private IEnumerator PutRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            using (var www = new UnityWebRequest(uri))
            {
                www.method = UnityWebRequest.kHttpVerbPUT;
                www.downloadHandler = new DownloadHandlerBuffer();

                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }

        private IEnumerator PutRoutine(string uri, byte[] bodyData, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Put(uri, bodyData))
            {
                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }

        private IEnumerator PutRoutine(string uri, string bodyData, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Put(uri, bodyData))
            {
                SetRequestHeader(www, requestHeaders);

                yield return www.SendWebRequest();

                ResponseHandler(www, onResult, onError);
            }
        }
    }
}
