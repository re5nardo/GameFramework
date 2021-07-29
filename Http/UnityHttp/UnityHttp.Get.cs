using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameFramework
{
    public partial class UnityHttp
    {
        public static void Get(string uri, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(GetRoutine(uri, requestHeaders, onResult, onError));
        }

        private static IEnumerator GetRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Get(uri))
            {
                www.SetRequestHeader(requestHeaders);

                yield return www.SendWebRequest();

                OnResponse(www, onResult, onError);
            }
        }
    }
}
