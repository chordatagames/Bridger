using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LevelCreator : ScriptableWizard
{
	const float levelDepth = 16f;

	public float
		gapSize,
		gapHeight,
		gapHeightDelta;
	public bool
		placeJointPoints;

	private Transform
		level,
		floor,
		wall_L,
		wall_R;
	static private Transform
		groundPrefab,
		jointPointPrefab;

	[MenuItem ("Bridger/Create Level")]
	static void CreateWizard ()
	{
		groundPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Level Creation/Ground.prefab") as Transform;
		jointPointPrefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Level Creation/JointPoint.prefab") as Transform;
		ScriptableWizard.DisplayWizard<LevelCreator>("Create Level", "Create");
	}
	
	void OnWizardCreate ()
	{
		DoLevelObject ();
		CreateFeatures ();
		ApplyValues();
		if (placeJointPoints)
			PlaceJointPoints();
	}

	void DoLevelObject ()
	{
		if (GameObject.Find ("LEVEL") == null)
		{
			level = new GameObject ("LEVEL").transform;
		}
		else
		{
			level = GameObject.Find ("LEVEL").transform;
		}
	}

	void CreateFeatures ()
	{
		CreateFeature (out floor, "Floor");
		CreateFeature (out wall_L, "Start");
		CreateFeature (out wall_R, "End");
	}

	void CreateFeature (out Transform feature, string name)
	{
		feature = GameObject.Instantiate(groundPrefab);
		feature.name = name;
		feature.parent = level;
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

	void PlaceJointPoints()
	{
		Vector3 jointPosition;
		Transform joint;

		jointPosition = new Vector3 (-gapSize * 0.5f, gapHeight, -levelDepth/2 - 0.03f);
		joint = (Transform) GameObject.Instantiate(jointPointPrefab, jointPosition, Quaternion.identity);
		joint.parent = wall_L;
		joint.name = "JointPoint";
		
		jointPosition = new Vector3 (gapSize * 0.5f, gapHeight + gapHeightDelta, -levelDepth/2 - 0.03f);
		joint = (Transform) GameObject.Instantiate(jointPointPrefab, jointPosition, Quaternion.identity);
		joint.parent = wall_R;
		joint.name = "JointPoint";
	}

	void OnWizardUpdate ()
	{
		gapSize -= (gapSize % Grid.gridSize);
		gapSize += ((gapSize % Grid.gridSize > Grid.gridSize * 0.5f) ? -Grid.gridSize : 0);
		gapHeightDelta = Mathf.Clamp(gapHeightDelta, -gapHeight + Grid.gridSize, gapHeightDelta);
	}
}
