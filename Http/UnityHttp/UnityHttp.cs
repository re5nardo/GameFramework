using UnityEngine.Networking;
using System;
using UnityEngine;

namespace GameFramework
{
    public partial class UnityHttp : MonoSingleton<UnityHttp>
    {
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        private static void OnResponse(UnityWebRequest www, Action<UnityWebRequest> onResult = null, Action<string> onError = null)
        {
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                onError?.Invoke(www.error);
            }
            else
            {
                onResult?.Invoke(www);
            }
        }
    }
}
