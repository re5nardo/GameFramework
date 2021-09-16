using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    public class SimplePubSubService
    {
        private Dictionary<Type, object> allSubscribers = new Dictionary<Type, object>();

        public void Publish<T>(T value)
        {
            if (allSubscribers.TryGetValue(typeof(T), out var subscribers))
            {
                var subscriberList = (GenericHandlerList<T>)subscribers;

                for (int i = subscriberList.handlers.Count - 1; i >= 0; i = Mathf.Min(i - 1, subscriberList.handlers.Count - 1))
                {
                    var subscriber = subscriberList.handlers[i];
                    subscriber?.Invoke(value);
                }
            }
        }

        public void AddSubscriber<T>(Action<T> subscriber)
        {
            if (!allSubscribers.ContainsKey(typeof(T)))
            {
                allSubscribers.Add(typeof(T), new GenericHandlerList<T>());
            }

            ((GenericHandlerList<T>)allSubscribers[typeof(T)]).handlers.Add(subscriber);
        }

        public void RemoveSubscriber<T>(Action<T> subscriber)
        {
            ((GenericHandlerList<T>)allSubscribers[typeof(T)]).handlers.Remove(subscriber);
        }

        public void Clear()
        {
            allSubscribers.Clear();
        }
    }

    class GenericHandlerList<T>
    {
        public List<Action<T>> handlers = new List<Action<T>>();
    }
}
