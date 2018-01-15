using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    private Dictionary<System.Type, LinkedList<IPooledObject>> m_dicObjectPool = new Dictionary<System.Type, LinkedList<IPooledObject>>();
    private Dictionary<string, LinkedList<GameObject>> m_dicGameObjectPool = new Dictionary<string, LinkedList<GameObject>>();
//    private HashSet<IPooledObject> m_hashAll = new HashSet<IPooledObject>();

    private Transform m_Transform = null;

    private void Start()
    {
        DontDestroyOnLoad(this);

        m_Transform = transform;
    }

    public T GetObject<T>() where T : class, IPooledObject, new()
    {
        lock (m_dicObjectPool)
        {
            System.Type type = typeof(T);
            if (!m_dicObjectPool.ContainsKey(type))
            {
                m_dicObjectPool[type] = new LinkedList<IPooledObject>();
            }

            T obj = null;
            if (m_dicObjectPool[type].Count > 0)
            {
                obj = (T)m_dicObjectPool[type].Last.Value;
                m_dicObjectPool[type].RemoveLast();
            }
            else
            {
                obj = new T();
//                m_hashAll.Add(obj);
            }

            obj.m_bInUse = true;
            obj.m_StartTime = System.DateTime.Now;
            obj.OnUsed();

            return obj;
        }
    }

    public bool ReturnObject<T>(T target) where T : IPooledObject
    {
        lock (m_dicObjectPool)
        {
            if(target == null)
            {
                Debug.LogWarning("Target is null!");
                return false;
            }

            System.Type type = target.GetType();
            if (!m_dicObjectPool.ContainsKey(type))
            {
                Debug.LogWarning("Invalid type! Type : " + type);
                return false;
            }

            target.m_bInUse = false;
            target.OnReturned();

            m_dicObjectPool[type].AddLast(target);

            return true;
        }
    }

    public GameObject GetGameObject(string strKey)
    {
        lock (m_dicGameObjectPool)
        {
            if (!m_dicGameObjectPool.ContainsKey(strKey))
            {
                m_dicGameObjectPool[strKey] = new LinkedList<GameObject>();
            }

            GameObject obj = null;
            PooledComponent pooled = null;
            if(m_dicGameObjectPool[strKey].Count > 0)
            {
                obj = m_dicGameObjectPool[strKey].Last.Value;
                m_dicGameObjectPool[strKey].RemoveLast();
            }
            else
            {
                obj = Object.Instantiate(Resources.Load(strKey)) as GameObject;

                if (obj == null)
                {
                    Debug.LogError("strKey is invalid! strKey : " + strKey);
                    return null;
                }

                pooled = obj.GetComponent<PooledComponent>();
                pooled.m_strKey = strKey;

//                m_hashAll.Add(pooled);
            }

            obj.SetActive(true);
            obj.transform.parent = null;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            pooled.m_bInUse = true;
            pooled.m_StartTime = System.DateTime.Now;
            pooled.OnUsed();

            return obj;
        }
    }

    public bool ReturnGameObject(GameObject target)
    {
        lock (m_dicGameObjectPool)
        {
            if(target == null)
            {
                Debug.LogWarning("Target is null!");
                return false;
            }

            PooledComponent pooled = target.GetComponent<PooledComponent>();
            if(pooled == null)
            {
                Debug.LogWarning("PooledComponent is null!");
                return false;
            }

            if (!m_dicGameObjectPool.ContainsKey(pooled.m_strKey))
            {
                Debug.LogWarning("Invalid Key! Key : " + pooled.m_strKey);
                return false;
            }

            target.SetActive(false);
            target.transform.parent = m_Transform;

            pooled.m_bInUse = false;
            pooled.OnReturned();

            m_dicGameObjectPool[pooled.m_strKey].AddLast(target);

            return true;
        }
    }

//    public void ForceReturn()
//    {
//        foreach (KeyValuePair<string, LinkedList<GameObject>> kv in m_dicGameObjectUsed)
//        {
//            LinkedList<GameObject> listGameObjectUsed = kv.Value;
//            LinkedList<GameObject> listGameObject = m_dicGameObjectUsed[kv.Key];
//
//            foreach (GameObject go in listGameObjectUsed)
//            {
//                listGameObject.AddLast(go);
//            }
//
//            listGameObjectUsed.Clear();
//        }
//    }
//
//    private void EmptyPool()
//    {
//        foreach (KeyValuePair<string, LinkedList<GameObject>> kv in m_dicGameObject)
//        {
//            LinkedList<GameObject> listGameObject = kv.Value;
//
//            foreach (GameObject go in listGameObject)
//            {
//                Destroy(go);
//            }
//
//            listGameObject.Clear();
//        }
//
//        //  Should call GC?
//    }
} 