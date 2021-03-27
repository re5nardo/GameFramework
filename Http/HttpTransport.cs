using System.Collections.Generic;
using System;

namespace GameFramework
{
    public partial class HttpTransport : MonoSingleton<HttpTransport>
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        #region Static functions
        public static void Get(string uri, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.GetRoutine(uri, requestHeaders, onResult, onError));
        }

        public static void Get(string uri, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.GetRoutine(uri, requestHeaders, onResult, onError));
        }

        public static void Post(string uri, string postData, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.PostRoutine(uri, postData, requestHeaders, onResult, onError));
        }

        public static void Post(string uri, string postData, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.PostRoutine(uri, postData, requestHeaders, onResult, onError));
        }

        public static void Put(string uri, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.PutRoutine(uri, requestHeaders, onResult, onError));
        }

        public static void Put(string uri, byte[] bodyData, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.PutRoutine(uri, bodyData, requestHeaders, onResult, onError));
        }

        public static void Put(string uri, string bodyData, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.PutRoutine(uri, bodyData, requestHeaders, onResult, onError));
        }

        public static void Delete(string uri, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.DeleteRoutine(uri, requestHeaders, onResult, onError));
        }

        public static void Delete(string uri, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            Instance.StartCoroutine(Instance.DeleteRoutine(uri, requestHeaders, onResult, onError));
        }
        #endregion
    }
}
