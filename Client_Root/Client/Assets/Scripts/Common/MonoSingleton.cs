using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T instance = null;
	public static T Instance
	{
		get
		{
			if (instance == null)
            {
                GameObject goSingleton = new GameObject (typeof(T).Name + "Singleton");

				instance = (T)goSingleton.AddComponent (typeof(T));
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
			Debug.LogWarning ("There is already MonoSingleton, so delete this");
			Destroy (this);
		}
	}

    public static T GetInstance()
    {
        return instance;
    }
}