using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameFramework
{
    public partial class UnityHttp
    {
        public static void Put(string uri, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(PutRoutine(uri, requestHeaders, onResult, onError));
        }

        public static void Put(string uri, string bodyData, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(PutRoutine(uri, bodyData, requestHeaders, onResult, onError));
        }

        private static IEnumerator PutRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            using (var www = new UnityWebRequest(uri))
            {
                www.method = UnityWebRequest.kHttpVerbPUT;
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader(requestHeaders);

                yield return www.SendWebRequest();

                OnResponse(www, onResult, onError);
            }
        }

        private static IEnumerator PutRoutine(string uri, string bodyData, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Put(uri, bodyData))
            {
                www.SetRequestHeader(requestHeaders);

                yield return www.SendWebRequest();

                OnResponse(www, onResult, onError);
            }
        }
    }
}
