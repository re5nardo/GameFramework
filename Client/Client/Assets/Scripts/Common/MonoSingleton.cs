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
				GameObject goSingleton = GameObject.Find ("Singleton");
				if (goSingleton == null)
				{
					goSingleton = new GameObject ("Singleton");
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
			if (gameObject.name == "Singleton")
			{
				instance = (T)this;
			}
			else
			{
				Debug.LogWarning ("MonoSingleton is not on Singleton object, so delete this");
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