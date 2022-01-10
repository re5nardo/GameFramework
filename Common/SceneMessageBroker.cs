using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    public class SceneMessageBroker : MonoSingleton<SceneMessageBroker>
    {
        public SimplePubSubService DefaultMessageBroker { get; } = new SimplePubSubService();

        protected override void OnDestroy()
        {
            base.OnDestroy();

            DefaultMessageBroker.Clear();
        }

        public static void Publish<T>()
        {
            Instance.DefaultMessageBroker.Publish(default(T));
        }

        public static void Publish<T>(T message)
        {
            Instance.DefaultMessageBroker.Publish(message);
        }

        public static void Publish(Type type, object message)
        {
            Instance.DefaultMessageBroker.Publish(type, message);
        }

        public static GenericHandler<T> AddSubscriber<T>(Action<T> subscriber)
        {
            return Instance.DefaultMessageBroker.AddSubscriber(subscriber);
        }

        public static void RemoveSubscriber<T>(Action<T> subscriber)
        {
            if (HasInstance())
            {
                Instance.DefaultMessageBroker.RemoveSubscriber(subscriber);
            }
        }
    }
}
