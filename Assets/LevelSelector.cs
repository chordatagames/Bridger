using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelector : MonoBehaviour
{
	private int currentlySelected = 0;
	public SelectableLevelCameraPoint[] availableLevels;

	private Vector3[,]tangents;

	void Start()
	{
		GotoSelectableLevel();
	}

	void Update ()
	{
		int lastSelected = currentlySelected;
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			currentlySelected--;
			currentlySelected = Mathf.Clamp(currentlySelected,0,availableLevels.Length-1);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			currentlySelected++;
			currentlySelected = Mathf.Clamp(currentlySelected,0,availableLevels.Length-1);
		}
		if(lastSelected != currentlySelected)
		{
			availableLevels[lastSelected].selectableLevel.DeSelect();
			GotoSelectableLevel();
		}
	}

	void GotoSelectableLevel()
	{
		transform.position = availableLevels[currentlySelected].position;
		transform.rotation = Quaternion.LookRotation(
			(availableLevels[currentlySelected].selectableLevel.transform.position+Vector3.up*5 - transform.position).normalized);
		availableLevels[currentlySelected].selectableLevel.Select();
	}

//	public Vector3 GetCurvePoint(float t)
//	{
//		return transform.TransformPoint(Bezier.GetPoint())
//	}
}

public static class Bezier {
	
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		return Vector3.Lerp(p0, p2, t);
	}
}

[System.Serializable]
public struct SelectableLevelCameraPoint
{
	public SelectableLevel selectableLevel;
	public Vector3 position;
}