using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Bridger;
using UnityEngine.UI;

namespace Bridger
{
	public class LevelCreator : ScriptableWizard
	{
		const float levelDepth = 16f;

		public float
			gapSize = 5,
			gapHeight = 5,
			gapHeightDelta = 2,
			buildZoneLeft = 2,
			buildZoneRight = 2,
			buildZoneDown = 2,
			buildZoneUp = 2,
			goalLineX = 5;
		public bool
			placeJointPoints = true;

		private Transform
			level,
			floor,
			wall_L,
			wall_R,
			parentOfNecessities;
		static private Transform
			groundPrefab,
			jointPointPrefab,
			parentOfNecessitiesPrefab;

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
			ImportPrefab (out parentOfNecessitiesPrefab, "ParentOfNecessities");
		}

		static void ImportPrefab(out Transform prefab, string name)
		{
			prefab = AssetDatabase.LoadAssetAtPath<Transform>("Assets/Level Creation/" + name + ".prefab") as Transform;
			if (prefab == null)
			{
				Debug.LogError ("The prefab for " + name + " was not found. It is null");
			}
		}

		void SetupLevelNecessities ()
		{
			InstantiateNecessities();
			SetupCar();
			SetupConstructionHandler();
			SetupEventSystem();
			SetupGoals();
			SetupIsometricCamera();
			SetupMainCamera();
			SetupUIManager();
			RemoveParentOfNecessities();
		}

		void InstantiateNecessities ()
		{
			parentOfNecessities = GameObject.Instantiate (parentOfNecessitiesPrefab);
		}

		void SetupGoals ()
		{
			Transform goals = parentOfNecessities.Find("Goals");
			Transform goalZone = goals.Find("GoalZone");
			goalZone.position = new Vector3 (goalLineX, gapHeight + gapHeightDelta);
			goalZone.GetComponent<BoxCollider2D> ().size = new Vector2 (1, 20);
		}

		void SetupIsometricCamera ()
		{
		}

		void SetupMainCamera ()
		{
			Transform mainCam = parentOfNecessities.Find ("Main Camera");
			mainCam.position = new Vector3 (0, gapHeight, -10);
			mainCam.GetComponent<Camera> ().orthographicSize = gapHeight + 1;
			mainCam.GetComponent<GridGUI> ().UpdateGrid ();
		}

		void SetupEventSystem()
		{
		}

		void SetupConstructionHandler()
		{
			//Transform ch = parentOfNecessities.Find("ConstructionHandler");
			//ConstructionHandler chScript = ch.GetComponent<ConstructionHandler> ();
			//chScript.constructionBorder.x = -gapSize/2 - buildZoneLeft;
			//chScript.constructionBorder.y = gapHeight - buildZoneDown;
			//chScript.constructionBorder.width = buildZoneLeft + gapSize + buildZoneRight;
			//chScript.constructionBorder.height = buildZoneUp + buildZoneUp;
		}

		void SetupCar ()
		{
			Transform car = parentOfNecessities.Find("EL CAR");
			car.position = new Vector3 (-gapSize / 2 - 1.0f, gapHeight + 1.0f);
		}

		void SetupUIManager ()
		{
		}

		void RemoveParentOfNecessities()
		{
			parentOfNecessities.DetachChildren ();
			GameObject.DestroyImmediate ((Object)parentOfNecessities.gameObject);
		}
	}
}

