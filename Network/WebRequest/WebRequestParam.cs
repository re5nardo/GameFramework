using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework
{
    public class WebRequestParam : IWebRequestParam
    {
        public HttpMethod method { get; set; }
        public string uri { get; set; }
        public Dictionary<string, string> requestHeader { get; set; }
        public object requestBody { get; set; }
        public List<IMultipartFormSection> form { get; set; }
    }
}
