using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameFramework
{
    public interface IWebRequest : IDisposable
    {
        public UnityWebRequest unityWebRequest { get; }
        public IWebRequestParam webRequestParam { get; }

        public UnityWebRequest CreateUnityWebRequest();
        public void OnCompleted();
    }
}
