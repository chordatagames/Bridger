using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	void LateUpdate ()
	{
		transform.rotation = Quaternion.LookRotation(
			-(Camera.main.transform.position-transform.position).normalized,
			transform.up);
	}
}
