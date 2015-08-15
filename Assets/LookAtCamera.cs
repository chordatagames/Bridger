using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	public Vector3 transformUpOffset;

	void LateUpdate ()
	{
		transform.localRotation = Quaternion.Euler(360.0f-Camera.main.transform.rotation.y, 90, 270);

	}
}
