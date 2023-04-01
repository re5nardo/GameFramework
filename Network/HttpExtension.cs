using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Linq;

namespace GameFramework
{
    public static partial class HttpExtension
    {
        public static void SetRequestHeader(this UnityWebRequest unityWebRequest, Dictionary<string, string> requestHeaders)
        {
            foreach (var requestHeader in requestHeaders ?? Enumerable.Empty<KeyValuePair<string, string>>())
            {
                if (string.IsNullOrEmpty(requestHeader.Key) || string.IsNullOrEmpty(requestHeader.Value))
                {
                    Debug.LogWarning($"Invalid RequestHeader: {requestHeader.Key} = {requestHeader.Value}");
                }
                else
                {
                    unityWebRequest.SetRequestHeader(requestHeader.Key, requestHeader.Value);
                }
            }
        }

        public static string ToQueryString(this Dictionary<string, string> queryString)
        {
            var stringBuilder = new StringBuilder();

            bool firstParam = true;
            foreach (var pair in queryString ?? Enumerable.Empty<KeyValuePair<string, string>>())
            {
                if (firstParam)
                {
                    stringBuilder.Append("?");
                    firstParam = false;
                }
                else
                {
                    stringBuilder.Append("&");
                }

                stringBuilder.Append($"{pair.Key}={pair.Value}");
            }

            return stringBuilder.ToString();
        }
         

        public static string AppendQueryString(this string target, Dictionary<string, string> queryString)
        {
            return new StringBuilder(target)
                .Append(queryString.ToQueryString())
                .ToString()
                ;
        }
    }
}
