using System.Collections;
using System.Collections.Generic;
using System;

namespace GameFramework
{
    public partial class Http
    {
        public static HttpRequestContainer<TResponse> Get<TResponse>(string apiEndpoint, Action<TResponse> onResult = null, Action<string> onError = null,
            Dictionary<string, string> requestHeaders = null, Dictionary<string, string> queryString = null, ServerSettings apiSettings = null)
        {
            var reqContainer = new HttpRequestContainer<TResponse>
            {
                uri = apiSettings.GetFullUri(apiEndpoint, queryString),
                onResult = onResult,
                onError = onError,
                requestHeaders = SetBaseHeader(requestHeaders ?? new Dictionary<string, string>()),
            };

            UnityHttp.Get(reqContainer.uri, reqContainer.requestHeaders, reqContainer.OnResponse, reqContainer.OnError);

            return reqContainer;
        }
    }
}
