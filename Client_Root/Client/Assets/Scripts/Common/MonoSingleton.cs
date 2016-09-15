using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	private static T instance = null;
	public static T Instance
	{
		get
		{
			if (instance == null)
            {
                GameObject goSingleton = GameObject.Find (typeof(T).Name + "Singleton");
				if (goSingleton == null)
				{
                    goSingleton = new GameObject (typeof(T).Name + "Singleton");
				}
					
				instance = (T)goSingleton.AddComponent (typeof(T));
			}

			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
            if (gameObject.name == (typeof(T).Name + "Singleton"))
			{
				instance = (T)this;
			}
			else
			{
                Debug.LogWarning (string.Format("MonoSingleton is not on {0}Singleton object, so delete this", typeof(T).Name));
				Destroy (this);
			}
		}
		else
		{
			Debug.LogWarning ("There is already MonoSingleton, so delete this");
			Destroy (this);
		}
	}

}