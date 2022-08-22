using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameFramework
{
    public class ScriptableObjectUtil
    {
        private static Dictionary<Type, ScriptableObject[]> cachedScriptableObjects = new Dictionary<Type, ScriptableObject[]>();

        public static T[] GetAll<T>(bool forceUpdate = false) where T : ScriptableObject
        {
            try
            {
                if (forceUpdate || !cachedScriptableObjects.ContainsKey(typeof(T)))
                {
                    cachedScriptableObjects[typeof(T)] = Resources.LoadAll<T>($"ScriptableObject/{typeof(T).Name}");
                }

                return cachedScriptableObjects[typeof(T)] as T[];
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }

        public static T Get<T>(string name, bool forceUpdate = false) where T : ScriptableObject
        {
            return Get<T>(x => x.name == name, forceUpdate);
        }

        public static T Get<T>(Func<T, bool> predicate, bool forceUpdate = false) where T : ScriptableObject
        {
            try
            {
                return GetAll<T>(forceUpdate).First(predicate);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return default;
            }
        }
    }
}
