using UnityEngine;
using System.Collections;

public class DontLoadDestroy : MonoBehaviour
{
	private static GameObject _instance;
	public static GameObject instance{ get{return _instance;} }
	public bool singleton;
	void Awake()
	{
		if(singleton)
		{
			if(_instance != null)
			{
				Destroy(gameObject);
			}
			else
			{
				_instance = gameObject;
			}
		}
		DontDestroyOnLoad(gameObject);
	}
}
