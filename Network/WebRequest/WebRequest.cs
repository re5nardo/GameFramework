using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace GameFramework
{
    public class WebRequest<T> : WebRequestBase
    {
        public T response;

        public WebRequest(IWebRequestParam webRequestParam) : base(webRequestParam) { }

        public override void OnCompleted()
        {
            if (webRequestParam is not WebRequestParam<T> param)
            {
                throw new Exception($"param is invalid. Type: {webRequestParam.GetType()}");
            }

            var deserialize = param.deserialize ?? DefaultDeserialize;

            response = deserialize.Invoke(Text);
        }

        private T DefaultDeserialize(string json)
        {
            return response = JsonConvert.DeserializeObject<T>(json);
        }
    }
}
