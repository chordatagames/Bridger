using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	public Vector3 offset;

	void Start()
	{
//		offset = transform.position + (Vector3)GetComponent<RectTransform>().rect.center - Camera.main.transform.position;
	}

	void LateUpdate ()
	{
		transform.rotation = Quaternion.LookRotation(-((Camera.main.transform.position-offset)-transform.position).normalized, transform.up);
//		transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(transform.position +((Camera.main.transform.position-offset)-transform.position), transform.position);
		Gizmos.DrawSphere(transform.position + offset, 0.25f);
	}
}
