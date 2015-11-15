using UnityEngine;
using System.Collections;

public class DontLoadDestroy : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
