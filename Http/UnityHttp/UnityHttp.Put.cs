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

        /// <summary>반환된 UnityWebRequest를 using 문으로 감싸거나 사용 후에 UnityWebRequest.Dispose 함수 호출을 권장합니다.</summary>
        public static UnityWebRequest Put(string uri, Dictionary<string, string> requestHeaders = null)
        {
            using (var www = new UnityWebRequest(uri))
            {
                www.method = UnityWebRequest.kHttpVerbPUT;
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader(requestHeaders);
                www.SendWebRequest();

                return www;
            }
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
