using UnityEngine;
using System.Collections;

namespace Bridger
{	
	public class GoalZone : MonoBehaviour
	{
		public Vehicle acceptedVehicle;
		private bool _completed = false;
		public bool completed{ get{return _completed;} }

		void OnTriggerEnter2D(Collider2D col)
		{
			Vehicle vehicle = col.GetComponent<Vehicle>();
			if(vehicle == acceptedVehicle)
			{
				_completed = true;
			}
		}
	}
}