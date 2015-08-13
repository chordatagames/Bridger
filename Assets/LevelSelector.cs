using UnityEngine;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit mouseRayHit = new RaycastHit();
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(mouseRay, out mouseRayHit,100.0f, 1<<12))
			{
				mouseRayHit.transform.GetComponent<SelectableLevel>().Select();
			}
		}

	}
}
