using UnityEngine;
using System.Collections;
namespace Bridger
{
	[CreateAssetMenu()]
	public class BridgePartType : ScriptableObject
	{
		public GameObject model;
		public float 
			strength,
			maxLength,
			massPerLength;
		
		public Material material;
		public Texture2D partPreviewTexture;
		public PhysicsMaterial2D physMaterial;
		public AudioClip placementSound;

		public void LoadType(GameObject go)
		{
			if(go.GetComponent<Renderer>() != null)
			{go.GetComponent<Renderer>().material = material;}
			foreach(Renderer r in go.GetComponentsInChildren<Renderer>())
			{
				r.material = material;
			}
			go.GetComponent<BoxCollider2D>().sharedMaterial = physMaterial;
		}
		
	}
}