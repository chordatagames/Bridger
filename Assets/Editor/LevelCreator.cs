using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LevelCreator : ScriptableWizard
{
	const float levelDepth = 16f;
	public float 
		gapSize = 5.0f,
		gapHeight = 5.0f,
		gapHeightDelta = 0.0f;

	private Transform
		level,
		floor,
		wall_L,
		wall_R;
	static private Transform groundPrefab;

	[MenuItem ("Bridger/Create Level")]
	static void CreateWizard ()
	{
		groundPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Level Creation/Ground.prefab") as Transform;
		ScriptableWizard.DisplayWizard<LevelCreator>("Create Level", "Create", "Apply");
	}
	
	void OnWizardCreate ()
	{
		if (GameObject.Find ("LEVEL") == null)
		{
			level = new GameObject ("LEVEL").transform;
		}
		else
		{
			level = GameObject.Find ("LEVEL").transform;
		}

		floor = GameObject.Instantiate(groundPrefab);
		floor.name = "FLOOR";
		floor.parent = level;

		wall_L = GameObject.Instantiate(groundPrefab);
		wall_L.name = "WALL_L";
		wall_L.parent = level;

		wall_R = GameObject.Instantiate(groundPrefab);
		wall_R.name = "WALL_R";
		wall_R.parent = level;

		ApplyValues();
		
		DeScaleJoints (floor);
		DeScaleJoints (wall_L);
		DeScaleJoints (wall_R);
		
		Undo.IncrementCurrentGroup ();
		Undo.RegisterCreatedObjectUndo (floor, "");
		Undo.RegisterCreatedObjectUndo (wall_L, "");
		Undo.RegisterCreatedObjectUndo (wall_R, "");
		Undo.SetCurrentGroupName ("level creation");
	}

	void ApplyValues()
	{
		floor.localScale = new Vector3(gapSize, gapHeight, levelDepth);
		floor.localPosition = -level.up * gapHeight * 0.5f;

		wall_L.localScale = new Vector3(levelDepth, gapHeight * 2, levelDepth);
		wall_L.transform.localPosition = -level.right * 0.5f * (gapSize + levelDepth);

		wall_R.localScale = new Vector3(levelDepth, gapHeight * 2 + gapHeightDelta, levelDepth);
		wall_R.transform.localPosition = level.right * 0.5f * (gapSize + levelDepth) + level.up * gapHeightDelta * 0.5f;
	}

	void OnWizardUpdate ()
	{
		gapSize -= (gapSize % Grid.gridSize);
		gapSize += ((gapSize % Grid.gridSize > Grid.gridSize * 0.5f) ? -Grid.gridSize : 0);
		gapHeightDelta = Mathf.Clamp(gapHeightDelta, -gapHeight + Grid.gridSize, gapHeightDelta);
	}
	
	// When the user pressed the "Apply" button OnWizardOtherButton is called.
	void OnWizardOtherButton () 
	{
		level = Selection.activeTransform;
		if(level != null)
		{
			if(level.name.Equals("LEVEL"))
			{
				floor = level.FindChild("FLOOR");
				wall_L = level.FindChild("WALL_L");
				wall_R = level.FindChild("WALL_R");

				ApplyValues();
			}
		}
	}

	void DeScaleJoints(Transform jointParent)
	{
		Vector3 inverseParentScale = new Vector3 (1.00f / jointParent.localScale.x, 1.00f / jointParent.localScale.y, 1.00f / jointParent.localScale.z);

		foreach (Transform c in jointParent)
		{
			if (c.name == "JointPoint")
			{
				c.localScale = Vector3.Scale(c.localScale, inverseParentScale);
			}
		}
	}
}
