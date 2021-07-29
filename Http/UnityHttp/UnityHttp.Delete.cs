using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameFramework
{
    public partial class UnityHttp
    {
        public static void Delete(string uri, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(DeleteRoutine(uri, requestHeaders, onResult, onError));
        }

        private static IEnumerator DeleteRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Delete(uri))
            {
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader(requestHeaders);

                yield return www.SendWebRequest();

                OnResponse(www, onResult, onError);
            }
        }
    }
}
