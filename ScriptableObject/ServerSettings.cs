using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [CreateAssetMenu(fileName = "ServerSettings", menuName = "ScriptableObjects/ServerSettings", order = 1)]
    public class ServerSettings : ScriptableObjectWrapper<ServerSettings>
    {
        public string scheme;
        public string host;

        public string GetFullUri(string api, Dictionary<string, string> queryString = null)
        {
            return Http.GetFullUri(api, queryString, this);
        }
    }
}
