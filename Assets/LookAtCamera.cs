using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	public Vector3 transformUpOffset;

	void LateUpdate ()
	{
		transform.rotation = Quaternion.LookRotation(
			-(Camera.main.transform.position-transform.position).normalized,
			(transform.up+transformUpOffset).normalized);
	}
}
