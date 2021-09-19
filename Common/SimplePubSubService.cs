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
            Publish(typeof(T), value);
        }

        public void Publish(Type type, object value)
        {
            if (allSubscribers.TryGetValue(type, out var subscribers))
            {
                var subscriberList = (IGenericHandlerList)subscribers;

                subscriberList.InvokeAll(value);
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

    class GenericHandlerList<T> : IGenericHandlerList
    {
        public List<Action<T>> handlers = new List<Action<T>>();

        public void InvokeAll(object value)
        {
            for (int i = handlers.Count - 1; i >= 0; i = Mathf.Min(i - 1, handlers.Count - 1))
            {
                var handler = handlers[i];
                handler?.Invoke((T)value);
            }
        }
    }

    interface IGenericHandlerList
    {
        void InvokeAll(object value);
    }
}
