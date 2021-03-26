using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [CreateAssetMenu(fileName = "ServerSettings", menuName = "ScriptableObjects/ServerSettings", order = 1)]
    public class ServerSettings : ScriptableObjectWrapper<ServerSettings>
    {
        public string scheme;
        public string host;
        public int port;

        public string GetFullUri(string apiCall, Dictionary<string, string> queryString)
        {
            return Http.GetFullUri(apiCall, queryString, this);
        }
    }
}
