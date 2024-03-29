﻿using System.Collections;
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
            if (allSubscribers.TryGetValue(typeof(T), out var value))
            {
                var genericHandlerList = (GenericHandlerList<T>)value;

                genericHandlerList.Remove(subscriber);
            }
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
            var origin = new List<GenericHandler<T>>(handlers);
            origin?.ForEach(handler =>
            {
                handler?.Invoke((T)value);
            });
        }

        public void Remove(Action<T> value)
        {
            var handler = handlers.Find(x => x.Equals(value));

            handlers.Remove(handler);
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
