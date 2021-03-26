using System.Collections.Generic;
using System;

namespace GameFramework
{
    public class HttpRequestContainer
    {
        public string apiEndpoint = null;
        public string fullUri = null;
        public Dictionary<string, string> requestHeaders;
        public byte[] payload = null;
        public string jsonResponse = null;
        public HttpRequestBase httpRequest;
        public HttpResultBase httpResult;
        public Action deserializeResult;
        public Action successCallback;
        public Action<string> errorCallback;
        public ServerSettings serverSettings;
    }
}
