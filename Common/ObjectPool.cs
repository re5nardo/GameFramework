using System.Collections.Generic;
using System;
using UnityEngine;

namespace GameFramework
{
    public class ObjectPool : MonoSingleton<ObjectPool>
    {
        private Dictionary<Type, InternalObjectPool> objectPools = new Dictionary<Type, InternalObjectPool>();

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public T GetObject<T>() where T : IPoolObject
        {
            return (T)GetObject(typeof(T));
        }

        public IPoolObject GetObject(Type type)
        {
            if (!objectPools.ContainsKey(type))
            {
                objectPools[type] = new InternalObjectPool();
            }

            return objectPools[type].GetObject(type);
        }

        public void ReturnObject(IPoolObject obj)
        {
            if (objectPools.TryGetValue(obj.GetType(), out var internalObjectPool))
            {
                internalObjectPool.ReturnObject(obj);
            }
            else
            {
                Debug.LogWarning($"Returned object is invalid! Type: {obj.GetType()}");
            }
        }

        class InternalObjectPool
        {
            private Queue<IPoolObject> pool = new Queue<IPoolObject>();
            private LinkedList<IPoolObject> beingUsed = new LinkedList<IPoolObject>();

            public IPoolObject GetObject(Type type)
            {
                if (!typeof(IPoolObject).IsAssignableFrom(type))
                {
                    Debug.LogError($"The type is invalid! Type: {type}");
                    return null;
                }

                IPoolObject target = pool.Count > 0 ? pool.Dequeue() : CreatePoolObject(type);

                if (target is Component component)
                {
                    component.gameObject.SetActive(true);
                }

                beingUsed.AddLast(target);
                return target;
            }

            public void ReturnObject(IPoolObject obj)
            {
                obj.Clear();

                if (obj is Component component)
                {
                    component.gameObject.SetActive(false);
                }

                beingUsed.Remove(obj);
                pool.Enqueue(obj);
            }

            private IPoolObject CreatePoolObject(Type type)
            {
                if (typeof(Component).IsAssignableFrom(type))
                {
                    return new GameObject().AddComponent(type) as IPoolObject;
                }
                else
                {
                    return Activator.CreateInstance(type) as IPoolObject;
                }
            }
        }
    }
}
