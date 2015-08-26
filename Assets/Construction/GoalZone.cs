using UnityEngine;
using System.Collections;

namespace Bridger
{	
	public class GoalZone : MonoBehaviour
	{
		public Vehicle acceptedVehicle;
		public bool completed;

		void OnTriggerEnter2D(Collider2D col)
		{
			Vehicle vehicle = col.GetComponent<Vehicle>();
			if(vehicle == acceptedVehicle)
			{
				completed = true;
			}
		}
	}
}