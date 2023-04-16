using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace GameFramework
{
    [CreateAssetMenu(fileName = "ServerSettings", menuName = "ScriptableObjects/ServerSettings", order = 1)]
    public class ServerSettings : ScriptableObjectWrapper<ServerSettings>
    {
        public string scheme;
        public string host;

        public string GetUri(string api, Dictionary<string, string> queryString = null)
        {
            return new StringBuilder(1000)
                .Append(scheme)
                .Append("://")
                .Append(host)
                .Append("/")
                .Append(api)
                .Append(queryString.ToQueryString())
                .ToString()
                ;
        }
    }
}
