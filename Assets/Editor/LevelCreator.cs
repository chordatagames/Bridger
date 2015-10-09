using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LevelCreator : ScriptableWizard
{
	const float levelDepth = 16f;

	public float
		gapSize = 5,
		gapHeight = 5,
		gapHeightDelta = 2,
		goalX = 30;
	public bool
		placeJointPoints = true;

	private Transform
		level,
		floor,
		wall_L,
		wall_R;
	static private Transform
		groundPrefab,
		jointPointPrefab,
		carPrefab,
		constructionHandlerPrefab,
		eventSystemPrefab,
		goalsPrefab,
		isometricCameraPrefab,
		mainCameraPrefab,
		UIManagerPrefab;

	[MenuItem ("Bridger/Create Level")]
	static void CreateWizard ()
	{
		ImportPrefabs();
		ScriptableWizard.DisplayWizard<LevelCreator>("Create Level", "Create");
	}
	
	void OnWizardCreate ()
	{
		DoLevelObject ();
		CreateFeatures ();
		ApplyValues();
		if (placeJointPoints)
			PlaceJointPoints();
		SetupLevelNecessities();
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
		Transform groundContainer = new GameObject(name).transform;
		groundContainer.parent = level;

		feature = GameObject.Instantiate(groundPrefab);
		feature.parent = groundContainer;
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

	static void ImportPrefabs()
	{
		ImportPrefab(out groundPrefab, "Ground");
		ImportPrefab(out jointPointPrefab, "JointPoint");
		ImportPrefab(out carPrefab, "EL CAR");
		ImportPrefab(out constructionHandlerPrefab, "Construction Handler");
		ImportPrefab(out eventSystemPrefab, "EventSystem");
		ImportPrefab(out goalsPrefab, "Goals");
		ImportPrefab(out isometricCameraPrefab, "IsoCam Full");
		ImportPrefab(out mainCameraPrefab, "Main Camera");
		ImportPrefab(out UIManagerPrefab, "UI Manager");
	}

	static void ImportPrefab(out Transform prefab, string name)
	{
		prefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Level Creation/" + name + ".prefab") as Transform;
	}

	void SetupLevelNecessities ()
	{
		SetupCar();
		SetupConstructionHandler();
		SetupEventSystem();
		SetupGoals();
		SetupIsometricCamera();
		SetupMainCamera();
		SetupUIManager();
	}
	
	void SetupGoals ()
	{
		Transform goals = GameObject.Instantiate (goalsPrefab);
		Transform goalZone = goals.Find("GoalZone");
		goalZone.position = new Vector3 (goalX, 0);
		goalZone.GetComponent<BoxCollider2D> ().size = new Vector2 (1, 20);
	}

	void SetupIsometricCamera ()
	{
		GameObject.Instantiate (isometricCameraPrefab);
	}

	void SetupMainCamera ()
	{

	}

	void SetupEventSystem()
	{

	}

	void SetupConstructionHandler()
	{

	}

	void SetupCar ()
	{

	}

	void SetupUIManager ()
	{
		GameObject.Instantiate (UIManagerPrefab);
	}
}
