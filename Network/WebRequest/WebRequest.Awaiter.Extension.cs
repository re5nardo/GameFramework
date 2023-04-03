using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public static class WebRequestAwaiterExtension
    {
        public static UnityWebRequestAwaiter GetAwaiter(this WebRequestBase webRequest)
        {
            return new UnityWebRequestAwaiter(webRequest.asyncOperation);
        }
    }
}
