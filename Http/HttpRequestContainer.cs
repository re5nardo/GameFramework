using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;
using System.IO.Compression;

namespace GameFramework
{
    public class HttpRequestContainer<TResponse> : CustomYieldInstruction
    {
        private bool isDone = false;
        public override bool keepWaiting => !isDone;
        public bool isError => !string.IsNullOrEmpty(error);

        public string uri;
        public Dictionary<string, string> requestHeaders;
        public TResponse response;
        public Action<TResponse> onResult;
        public Action<string> onError;
        public string error;

        public void Finish()
        {
            isDone = true;
        }

        public void OnResponse(UnityWebRequest www)
        {
            string jsonResponse = "";
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
                                jsonResponse = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    jsonResponse = www.downloadHandler.text;
                }

                response = OnDeserializeResponse(jsonResponse);
            }
            catch (Exception e)
            {
                OnError(e.Message);
                return;
            }

            onResult?.Invoke(response);
            Finish();
        }

        public void OnError(string error)
        {
            this.error = error;
            onError?.Invoke(error);
            Finish();
        }

        private TResponse OnDeserializeResponse(string json)
        {
            var response = JsonUtility.FromJson<TResponse>(json);
            if (response is IPostDeserialize postDeserialize)
            {
                postDeserialize.OnPostDeserialize(json);
            }

            return response;
        }
    }
}
