using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    public class SimplePubSubService<T, U>
    {
        private Dictionary<T, List<Action<U>>> allSubscribers = new Dictionary<T, List<Action<U>>>();

        public void Publish(T key, U value)
        {
            if (allSubscribers.TryGetValue(key, out List<Action<U>> subscribers))
            {
                for (int i = subscribers.Count - 1; i >= 0; i = Mathf.Min(i - 1, subscribers.Count - 1))
                {
                    var subscriber = subscribers[i];
                    subscriber?.Invoke(value);
                }
            }
        }

        public void AddSubscriber(T key, Action<U> subscriber)
        {
            if (!allSubscribers.ContainsKey(key))
            {
                allSubscribers.Add(key, new List<Action<U>>());
            }

            allSubscribers[key].Add(subscriber);
        }

        public void RemoveSubscriber(T key, Action<U> subscriber)
        {
            allSubscribers[key].Remove(subscriber);
        }

        public void Clear()
        {
            allSubscribers.Clear();
        }
    }
}
