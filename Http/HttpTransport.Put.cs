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
        private IEnumerator PutRoutine(string uri, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            using (var www = new UnityWebRequest(uri))
            {
                www.method = UnityWebRequest.kHttpVerbPUT;
                www.downloadHandler = new DownloadHandlerBuffer();

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

                yield return www.SendWebRequest();

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
                        yield break;
                    }

                    onResult?.Invoke(response);
                }
            }
        }

        private IEnumerator PutRoutine(string uri, byte[] bodyData, Dictionary<string, string> requestHeaders = null, Action<string> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Put(uri, bodyData))
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

                yield return www.SendWebRequest();

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
                        yield break;
                    }

                    onResult?.Invoke(response);
                }
            }
        }

        private IEnumerator PutRoutine(string uri, string bodyData, Dictionary<string, string> requestHeaders = null, Action<byte[]> onResult = null, Action<string> onError = null)
        {
            using (var www = UnityWebRequest.Put(uri, bodyData))
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

                yield return www.SendWebRequest();

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
}
