using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    public class PoolObjectDisposer<T> : IDisposable where T : IPoolObject
    {
        private static Queue<PoolObjectDisposer<T>> queue = new Queue<PoolObjectDisposer<T>>();

        public T PoolObject { get; private set; }

        public static PoolObjectDisposer<T> Get()
        {
            var poolObjectDisposer = queue.Count > 0 ? queue.Dequeue() : new PoolObjectDisposer<T>();

            poolObjectDisposer.PoolObject = ObjectPool.Instance.GetObject<T>();

            return poolObjectDisposer;
        }

        public void Dispose()
        {
            ObjectPool.Instance.ReturnObject(PoolObject);

            PoolObject = default;

            queue.Enqueue(this);
        }
    }
}
