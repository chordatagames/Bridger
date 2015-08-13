using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Bridger
{	
	public class GoalZone : MonoBehaviour
	{
		public Vehicle[] acceptedVehicles;
		public float radius;
		private bool _completed;
		public bool completed{ get{return _completed;} }
		void OnTriggerEnter2D(Collider2D col)
		{
			Vehicle vehicle = col.GetComponent<Vehicle>();
			foreach (Vehicle v in acceptedVehicles)
			{
				if(vehicle == v)
				{
					col.attachedRigidbody.AddForceAtPosition(
						col.transform.up*3,
						col.transform.position+Vector3.right * 0.25f
						);
					_completed = true;
					break;
				}
			}
		}
		void Update()
		{

		}
	}
}