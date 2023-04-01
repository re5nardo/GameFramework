using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework
{
    public interface IWebRequestParam
    {
        public HttpMethod method { get; }
        public string uri { get; }
        public Dictionary<string, string> requestHeader { get; }
        public object requestBody { get; }
        public List<IMultipartFormSection> form { get; }
    }
}
