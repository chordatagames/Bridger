using UnityEngine;
using System.Collections;

namespace Bridger
{
	public class Vehicle : MonoBehaviour, IResetable
	{
		public Color vehicleColor;
		public VehicleType type;
		Rigidbody2D rigid;
		TransformData resetTransform;

		void Awake()
		{
			rigid = GetComponent<Rigidbody2D>();
		}

		void Start()
		{
			resetTransform = new TransformData(transform);
			Level.AddToLevel(this);

			transform.FindChild("Body").GetComponent<Renderer>().material.color = vehicleColor;
		}

		public void Reset()
		{
			resetTransform.Reload(transform);
			rigid.velocity = Vector2.zero;
			rigid.angularVelocity = 0;
			rigid.isKinematic = true;
		}
		public void StartPhysics()
		{
			Rigidbody2D rigid = GetComponent<Rigidbody2D>();
			rigid.isKinematic = false;
		}
		public bool Pool()
		{
			return false;
			//gameObject.SetActive(false); Vehicles should not be pooled, as player has no control of them
		}
		public bool UnPool()
		{
			return false;
			//gameObject.SetActive(true); Vehicles should not be pooled, as player has no control of them
		}
	}
}