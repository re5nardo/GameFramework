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

        public static string GetFullUri(string api, Dictionary<string, string> queryString, ServerSettings apiSettings)
        {
            return new StringBuilder(1000)
                .Append(apiSettings.scheme)
                .Append("://")
                .Append(apiSettings.host)
                .Append("/")
                .Append(api)
                .Append(queryString.ToQueryString())
                .ToString()
                ;
        }
    }
}
