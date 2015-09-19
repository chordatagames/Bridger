using UnityEngine;
using System.Collections;

public class AlphaZControl : MonoBehaviour //All objects that could obstruct the view while building would be a child of this
{
	//TODO make this the control for alpha between modes
	public float zLimit, alphaValue;

	void Start()
	{
		TestZ();
	}

	void TestZ() //This could simply be called by an editor interface-button, instead of in the Start function
	{
		foreach(Transform c in transform)
		{
			if(c.localPosition.z < zLimit)
			{
				MeshRenderer r = c.GetComponentInChildren<MeshRenderer>();
				if(r != null)
				{
					Debug.Log(c.name);
					r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, alphaValue); //This will need to be some sort of shader-stuff TODO
				}
			}
		}
	}
}
