using UnityEngine;

namespace GameFramework
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject goSingleton = new GameObject(typeof(T).Name + "Singleton");

                    instance = goSingleton.AddComponent<T>();
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else
            {
                Debug.LogWarning("There is already MonoSingleton, so delete this");
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
            if (instance == null)
            {
                GameObject goSingleton = new GameObject(typeof(T).Name + "Singleton");

                instance = goSingleton.AddComponent<T>();
            }
        }
    }
}
