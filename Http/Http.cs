using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public partial class Http
    {
        private static Dictionary<string, string> SetBaseHeader(Dictionary<string, string> headers)
        {
            headers["Content-Type"] = "application/json";
            headers["Accept-Encoding"] = "gzip";

            return headers;
        }

        public static string GetFullUri(string apiCall, Dictionary<string, string> queryString, ServerSettings apiSettings)
        {
            StringBuilder sb = new StringBuilder(1000);

            sb.Append(apiSettings.scheme).Append("://")
                .Append(apiSettings.host).Append(":")
                .Append(apiSettings.port)
                .Append("/")
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
    }
}
