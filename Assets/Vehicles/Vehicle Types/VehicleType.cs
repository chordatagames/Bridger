using UnityEngine;
using System.Collections;

namespace Bridger
{
	[CreateAssetMenu()]
	public class VehicleType : ScriptableObject
	{
		public GameObject prefab;
		public float mass;
		
		public Material material;
		public Texture2D vehiclePreview;

		public void LoadType(GameObject go)
		{
			go.GetComponent<Rigidbody2D>().mass = mass;
		}
	}
}