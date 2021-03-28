using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using UnityEngine.Networking;

namespace GameFramework
{
    public class Http : MonoSingleton<Http>
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        #region Static functions
        public static void MakeApiCall<TResult>(string method, string apiEndpoint, HttpRequestBase request, Action<TResult> resultCallback, Action<string> errorCallback,
            Dictionary<string, string> extraHeaders = null, ServerSettings apiSettings = null) where TResult : HttpResultBase
        {
            var reqContainer = new HttpRequestContainer
            {
                apiEndpoint = apiEndpoint,
                fullUri = apiSettings.GetFullUri(apiEndpoint, null),
                serverSettings = apiSettings ?? ServerSettings.Get(),
                payload = Encoding.UTF8.GetBytes(JsonUtility.ToJson(request)),
                httpRequest = request,
                errorCallback = errorCallback,
                requestHeaders = extraHeaders ?? new Dictionary<string, string>(),
            };

            reqContainer.deserializeResult = () =>
            {
                reqContainer.httpResult = JsonUtility.FromJson<TResult>(reqContainer.jsonResponse);
            };
            reqContainer.successCallback = () =>
            {
                resultCallback?.Invoke((TResult)reqContainer.httpResult);
            };

            MakeApiCall(method, reqContainer);
        }

        public static void MakeApiCall(string method, HttpRequestContainer reqContainer)
        {
            reqContainer.requestHeaders["Content-Type"] = "application/json";
            reqContainer.requestHeaders["Accept-Encoding"] = "gzip";

            bool compress = reqContainer.payload.Length > 1024;
            if (compress)
            {
                reqContainer.requestHeaders["Content-Encoding"] = "gzip";

                using (var memoryStream = new MemoryStream())
                {
                    using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                    {
                        gZipStream.Write(reqContainer.payload, 0, reqContainer.payload.Length);
                    }
                    reqContainer.payload = memoryStream.ToArray();
                }
            }

            switch (method)
            {
                case UnityWebRequest.kHttpVerbGET:
                    HttpTransport.Get(reqContainer.fullUri, reqContainer.requestHeaders,
                        result =>
                        {
                            Instance.OnResponse(result, reqContainer);
                        },
                        error =>
                        {
                            Instance.OnError(error, reqContainer);
                        }
                    );
                    break;

                case UnityWebRequest.kHttpVerbPOST:
                    HttpTransport.Post(reqContainer.fullUri, Encoding.UTF8.GetString(reqContainer.payload), reqContainer.requestHeaders,
                        result =>
                        {
                            Instance.OnResponse(result, reqContainer);
                        },
                        error =>
                        {
                            Instance.OnError(error, reqContainer);
                        }
                    );
                    break;

                case UnityWebRequest.kHttpVerbPUT:
                    HttpTransport.Put(reqContainer.fullUri, reqContainer.payload, reqContainer.requestHeaders,
                        result =>
                        {
                            Instance.OnResponse(result, reqContainer);
                        },
                        error =>
                        {
                            Instance.OnError(error, reqContainer);
                        }
                    );
                    break;

                case UnityWebRequest.kHttpVerbDELETE:
                    HttpTransport.Delete(reqContainer.fullUri, reqContainer.requestHeaders,
                        result =>
                        {
                            Instance.OnResponse(result, reqContainer);
                        },
                        error =>
                        {
                            Instance.OnError(error, reqContainer);
                        }
                    );
                    break;
            }
        }

        public static string GetFullUri(string apiCall, Dictionary<string, string> queryString, ServerSettings apiSettings)
        {
            StringBuilder sb = new StringBuilder(1000);

            sb.Append(apiSettings.scheme).Append("://")
                .Append(apiSettings.host).Append(":")
                .Append(apiSettings.port)
                .Append(apiCall);

            bool firstParam = true;
            queryString?.ForEach(pair =>
            {
                if (firstParam)
                {
                    sb.Append("?");
                    firstParam = false;
                }
                else
                {
                    sb.Append("&");
                }
                sb.Append(pair.Key).Append("=").Append(pair.Value);
            });

            return sb.ToString();
        }
        #endregion

        private void OnResponse(string jsonResponse, HttpRequestContainer reqContainer)
        {
            reqContainer.jsonResponse = jsonResponse;
            reqContainer.deserializeResult();
            reqContainer.successCallback();
        }

        private void OnError(string error, HttpRequestContainer reqContainer)
        {
            reqContainer.jsonResponse = error;
            reqContainer.errorCallback?.Invoke(error);
        }
    }
}
