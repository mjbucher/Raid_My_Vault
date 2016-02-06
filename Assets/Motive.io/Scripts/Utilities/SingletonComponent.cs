using UnityEngine;

public abstract class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T>
{
	static T sInstance = null;

	public static T Instance
	{
		get{return sInstance;}
	}

	protected virtual void Awake()
	{
		if (sInstance != null)
		{
 			Debug.LogError("SingletonComponent.Awake: error " + name + " already initialized");
		}

		sInstance = (T)this;
	}

	protected virtual void Start()
	{
		
	}
}