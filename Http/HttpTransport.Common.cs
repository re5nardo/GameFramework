using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO.Compression;
using System.IO;

namespace GameFramework
{
    public partial class HttpTransport
    {
        private void SetRequestHeader(UnityWebRequest www, Dictionary<string, string> requestHeaders = null)
        {
            requestHeaders?.ForEach(headerPair =>
            {
                if (!string.IsNullOrEmpty(headerPair.Key) && !string.IsNullOrEmpty(headerPair.Value))
                {
                    www.SetRequestHeader(headerPair.Key, headerPair.Value);
                }
                else
                {
                    Debug.LogWarning("Null header: " + headerPair.Key + " = " + headerPair.Value);
                }
            });
        }

        private void ResponseHandler(UnityWebRequest www, Action<string> onResult = null, Action<string> onError = null)
        {
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                onError?.Invoke(www.error);
            }
            else
            {
                string response = "";
                try
                {
                    if (www.GetResponseHeader("Content-Encoding") == "gzip")
                    {
                        using (var memoryStream = new MemoryStream(www.downloadHandler.data))
                        {
                            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                            {
                                using (var streamReader = new StreamReader(gZipStream))
                                {
                                    response = streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                    else
                    {
                        response = www.downloadHandler.text;
                    }
                }
                catch (Exception e)
                {
                    onError?.Invoke(e.Message);
                    return;
                }

                onResult?.Invoke(response);
            }
        }
        
        private void ResponseHandler(UnityWebRequest www, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                onError?.Invoke(www.error);
            }
            else
            {
                onResult?.Invoke(www.downloadHandler.data);
            }
        }
    }
}