using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;

namespace GameFramework
{
    public partial class UnityHttp
    {
        public static void Post(string uri, string postData, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(PostRoutine(uri, postData, requestHeaders, onResult, onError));
        }

        public static void Post(string uri, List<IMultipartFormSection> multipartFormSections, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(PostRoutine(uri, multipartFormSections, requestHeaders, onResult, onError));
        }

        private static IEnumerator PostRoutine(string uri, string postData/*json format*/, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Post(uri, postData))
            {
                byte[] jsonToSend = new UTF8Encoding().GetBytes(postData);
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader(requestHeaders);

                yield return www.SendWebRequest();

                OnResponse(www, onResult, onError);
            }
        }

        private static IEnumerator PostRoutine(string uri, List<IMultipartFormSection> multipartFormSections, Dictionary<string, string> requestHeaders = null, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Post(uri, multipartFormSections))
            {
                www.SetRequestHeader(requestHeaders);

                yield return www.SendWebRequest();

                OnResponse(www, onResult, onError);
            }
        }
    }
}
