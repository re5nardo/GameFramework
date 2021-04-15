using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

namespace GameFramework
{
    public partial class Http
    {
        public static HttpRequestContainer<TResponse> Post<TResponse>(string apiEndpoint, string postData, Action<TResponse> onResult = null, Action<string> onError = null,
            Dictionary<string, string> requestHeaders = null, Dictionary<string, string> queryString = null, ServerSettings apiSettings = null)
        {
            var reqContainer = new HttpRequestContainer<TResponse>
            {
                uri = apiSettings.GetFullUri(apiEndpoint, queryString),
                onResult = onResult,
                onError = onError,
                requestHeaders = requestHeaders,
            };

            UnityHttp.Post(reqContainer.uri, postData, reqContainer.requestHeaders, reqContainer.OnResponse, reqContainer.OnError);

            return reqContainer;
        }

        public static HttpRequestContainer<TResponse> Post<TResponse>(string apiEndpoint, List<IMultipartFormSection> multipartFormSections, Action<TResponse> onResult = null, Action<string> onError = null,
            Dictionary<string, string> requestHeaders = null, Dictionary<string, string> queryString = null, ServerSettings apiSettings = null)
        {
            var reqContainer = new HttpRequestContainer<TResponse>
            {
                uri = apiSettings.GetFullUri(apiEndpoint, queryString),
                onResult = onResult,
                onError = onError,
                requestHeaders = SetBaseHeader(requestHeaders ?? new Dictionary<string, string>()),
            };

            UnityHttp.Post(reqContainer.uri, multipartFormSections, reqContainer.requestHeaders, reqContainer.OnResponse, reqContainer.OnError);

            return reqContainer;
        }
    }
}
