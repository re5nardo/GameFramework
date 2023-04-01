using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using System.Text;

namespace GameFramework
{
    public class WebRequest : CustomYieldInstruction, IWebRequest
    {
        public override bool keepWaiting
        {
            get
            {
                bool isDone = asyncOperation != null && asyncOperation.isDone;
                return isDone == false;
            }
        }

        public UnityWebRequestAsyncOperation asyncOperation { get; private set; }
        public event Action completed;
        public string Text => unityWebRequest.downloadHandler.text;
        public bool isSuccess => unityWebRequest.result == UnityWebRequest.Result.Success;
        public string error => unityWebRequest.error;
        public long responseCode => unityWebRequest.responseCode;

        public UnityWebRequest unityWebRequest { get; private set; }
        public IWebRequestParam webRequestParam { get; private set; }
       
        public WebRequest(IWebRequestParam webRequestParam)
        {
            if (webRequestParam == null)
            {
                throw new Exception("webRequestParam must not null.");
            }

            this.webRequestParam = webRequestParam;
            this.unityWebRequest = CreateUnityWebRequest();
            this.asyncOperation = this.unityWebRequest.SendWebRequest();
            this.asyncOperation.completed += _ =>
            {
                DeserializeObject();

                completed?.Invoke();
            };
        }

        public void Dispose()
        {
            unityWebRequest?.Dispose();
        }

        public virtual UnityWebRequest CreateUnityWebRequest()
        {
            if (webRequestParam is not WebRequestParam param)
            {
                throw new Exception($"Invalid Param Type. Type: {webRequestParam.GetType()}");
            }

            var unityWebRequest = param.form == null ? CreateDefaultUnityWebRequest() : CreateFormUnityWebRequest();

            unityWebRequest.SetRequestHeader(param.requestHeader);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

            return unityWebRequest;
        }

        private UnityWebRequest CreateDefaultUnityWebRequest()
        {
            if (webRequestParam is not WebRequestParam param)
            {
                throw new Exception($"Invalid Param Type. Type: {webRequestParam.GetType()}");
            }

            var unityWebRequest = new UnityWebRequest(param.uri, param.method.ToString());

            if (param.requestBody != null)
            {
                var bodyJson = JsonConvert.SerializeObject(param.requestBody, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                byte[] data = new UTF8Encoding().GetBytes(bodyJson);
                unityWebRequest.uploadHandler = new UploadHandlerRaw(data);
                unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            }

            return unityWebRequest;
        }

        private UnityWebRequest CreateFormUnityWebRequest()
        {
            if (webRequestParam is not WebRequestParam param)
            {
                throw new Exception($"Invalid Param Type. Type: {webRequestParam.GetType()}");
            }

            if (param.method != HttpMethod.POST)
            {
                throw new Exception($"Only post method is valid to send Form. current method: {param.method}");
            }

            return UnityWebRequest.Post(param.uri, param.form);
        }

        public virtual void DeserializeObject() { }
    }

    public class WebRequest<T> : WebRequest
    {
        public T value;

        public WebRequest(IWebRequestParam webRequestParam) : base(webRequestParam) { }

        public override void DeserializeObject()
        {
            value = JsonConvert.DeserializeObject<T>(Text);
        }
    }
}
