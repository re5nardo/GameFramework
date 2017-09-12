using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    private Dictionary<System.Type, LinkedList<IPooledObject>> m_dicObject = new Dictionary<System.Type, LinkedList<IPooledObject>>();
    private Dictionary<System.Type, LinkedList<IPooledObject>> m_dicObjectUsed = new Dictionary<System.Type, LinkedList<IPooledObject>>();

    private Dictionary<string, LinkedList<GameObject>> m_dicGameObject = new Dictionary<string, LinkedList<GameObject>>();
    private Dictionary<string, LinkedList<GameObject>> m_dicGameObjectUsed = new Dictionary<string, LinkedList<GameObject>>();

    private Transform m_Transform = null;

    private void Start()
    {
        DontDestroyOnLoad(this);

        m_Transform = transform;
    }

    public T GetObject<T>() where T : class, IPooledObject, new()
    {
        LinkedList<IPooledObject> listObject = null;
        if(!m_dicObject.TryGetValue(typeof(T), out listObject))
        {
            listObject = new LinkedList<IPooledObject>();
            m_dicObject[typeof(T)] = listObject;
        }

        T obj = null;
        if(listObject.Count > 0)
        {
            obj = (T)listObject.Last.Value;
            listObject.RemoveLast();
        }
        else
        {
            obj = new T();
        }

        LinkedList<IPooledObject> listObjectUsed = null;
        if(!m_dicObjectUsed.TryGetValue(typeof(T), out listObjectUsed))
        {
            listObjectUsed = new LinkedList<IPooledObject>();
            m_dicObjectUsed[typeof(T)] = listObjectUsed;
        }
        listObjectUsed.AddLast(obj);

        obj.OnUsed();

        return obj;
    }

    public bool ReturnObject<T>(T target) where T : IPooledObject
    {
        if(target == null)
        {
            Debug.LogWarning("Target is null!");
            return false;
        }

        LinkedList<IPooledObject> listObject = null;
        if(!m_dicObject.TryGetValue(target.GetType(), out listObject))
        {
            Debug.LogWarning("Invalid type! Type : " + target.GetType());
            return false;
        }

        listObject.AddLast(target);
        m_dicObjectUsed[target.GetType()].Remove(target);

        target.OnReturned();

        return true;
    }

    public GameObject GetGameObject(string strKey)
    {
        LinkedList<GameObject> listObject = null;
        if(!m_dicGameObject.TryGetValue(strKey, out listObject))
        {
            listObject = new LinkedList<GameObject>();
            m_dicGameObject[strKey] = listObject;
        }

        GameObject obj = null;
        if(listObject.Count > 0)
        {
            obj = listObject.Last.Value;
            listObject.RemoveLast();
        }
        else
        {
            obj = Object.Instantiate(Resources.Load(strKey)) as GameObject;
        }

        obj.SetActive(true);
        obj.transform.parent = null;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        PooledComponent pooled = obj.GetComponent<PooledComponent>();
        pooled.m_strKey = strKey;

        LinkedList<GameObject> listObjectUsed = null;
        if(!m_dicGameObjectUsed.TryGetValue(strKey, out listObjectUsed))
        {
            listObjectUsed = new LinkedList<GameObject>();
            m_dicGameObjectUsed[strKey] = listObjectUsed;
        }
        listObjectUsed.AddLast(obj);

        pooled.OnUsed();

        return obj;
    }

    public bool ReturnGameObject(GameObject target)
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
            
        LinkedList<GameObject> listObject = null;
        if(!m_dicGameObject.TryGetValue(pooled.m_strKey, out listObject))
        {
            Debug.LogWarning("Invalid Key! Key : " + pooled.m_strKey);
            return false;
        }

        target.SetActive(false);
        target.transform.parent = m_Transform;


        listObject.AddLast(target);
        m_dicGameObjectUsed[pooled.m_strKey].Remove(target);

        pooled.OnReturned();

        return true;
    }

    public void ForceReturn()
    {
        foreach (KeyValuePair<string, LinkedList<GameObject>> kv in m_dicGameObjectUsed)
        {
            LinkedList<GameObject> listGameObjectUsed = kv.Value;
            LinkedList<GameObject> listGameObject = m_dicGameObjectUsed[kv.Key];

            foreach (GameObject go in listGameObjectUsed)
            {
                listGameObject.AddLast(go);
            }

            listGameObjectUsed.Clear();
        }
    }

    private void EmptyPool()
    {
        foreach (KeyValuePair<string, LinkedList<GameObject>> kv in m_dicGameObject)
        {
            LinkedList<GameObject> listGameObject = kv.Value;

            foreach (GameObject go in listGameObject)
            {
                Destroy(go);
            }

            listGameObject.Clear();
        }

        //  Should call GC?
    }
} 