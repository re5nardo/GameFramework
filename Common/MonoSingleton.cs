using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

namespace GameFramework
{
    public class MonoSingletonBase : MonoBehaviour
    {
        public static Func<bool> condition;
    }

    public class MonoSingleton<T> : MonoSingletonBase where T : MonoSingleton<T>
    {
        [SerializeField] private bool overriding = false;
        public bool Overriding => overriding;

        protected static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Instantiate();
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
                return;
            }

            if (instance == this)
            {
                return;
            }

            if (overriding)
            {
                Debug.LogWarning("Destroy old singleton instance!");
                DestroyImmediate(instance);
                instance = (T)this;
            }
            else
            {
                Debug.LogWarning("Destroy new singleton instance!");
                DestroyImmediate(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        public static bool HasInstance()
        {
            return instance != null;
        }

        public static void Instantiate()
        {
            if (instance != null)
            {
                return;
            }

            if (condition != null && condition.Invoke() == false)
            {
                return;
            }

            var candidates = GameObject.FindObjectsOfType<T>(true).ToList();
            var target = candidates?.Find(x => x.Overriding);
            if (target != null)
            {
                instance = target;
                return;
            }

            var factoryMethod = typeof(T).GetMethod("CreateInstance", BindingFlags.Public | BindingFlags.Static);
            if (factoryMethod != null)
            {
                instance = factoryMethod.Invoke(null, null) as T;
            }
            else
            {
                GameObject goSingleton = new GameObject(typeof(T).Name + "Singleton");

                instance = goSingleton.AddComponent<T>();
            }
        }
    }
}
