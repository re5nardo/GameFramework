using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework
{
    public static class HttpExtension
    {
        public static void SetRequestHeader(this UnityWebRequest unityWebRequest, Dictionary<string, string> requestHeaders)
        {
            requestHeaders?.ForEach(requestHeader =>
            {
                if (string.IsNullOrEmpty(requestHeader.Key) || string.IsNullOrEmpty(requestHeader.Value))
                {
                    Debug.LogWarning("Null requestHeader: " + requestHeader.Key + " = " + requestHeader.Value);
                }
                else
                {
                    unityWebRequest.SetRequestHeader(requestHeader.Key, requestHeader.Value);
                }
            });
        }
    }
}
