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

        public GenericHandler<T> AddSubscriber<T>(Action<T> subscriber)
        {
            if (!allSubscribers.ContainsKey(typeof(T)))
            {
                allSubscribers.Add(typeof(T), new GenericHandlerList<T>());
            }

            var handler = new GenericHandler<T>(subscriber);

            ((GenericHandlerList<T>)allSubscribers[typeof(T)]).handlers.Add(handler);

            return handler;
        }

        public void RemoveSubscriber<T>(Action<T> subscriber)
        {
            var handler = ((GenericHandlerList<T>)allSubscribers[typeof(T)]).handlers.Find(x => x.Equals(subscriber));

            ((GenericHandlerList<T>)allSubscribers[typeof(T)]).handlers.Remove(handler);
        }

        public void Clear()
        {
            allSubscribers.Clear();
        }
    }

    class GenericHandlerList<T> : IGenericHandlerList
    {
        public List<GenericHandler<T>> handlers = new List<GenericHandler<T>>();

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

    public class GenericHandler<T>
    {
        private Action<T> handler;
        private Predicate<T> predicate;

        public GenericHandler(Action<T> handler)
        {
            this.handler = handler;
        }

        public void Where(Predicate<T> predicate)
        {
            this.predicate = predicate;
        }

        public void Invoke(object value)
        {
            if (predicate == null ? true : predicate.Invoke((T)value))
            {
                handler?.Invoke((T)value);
            }
        }

        public bool Equals(Action<T> handler)
        {
            return this.handler == handler;
        }
    }
}
