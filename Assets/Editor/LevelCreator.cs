using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Bridger
{
	public class LevelCreator : ScriptableWizard
	{
		const float levelDepth = 24f;
		public float 
			gapSize,
			gapHeight,
			gapHeightDelta;

		private Transform
			level,
			floor,
			wall_L,
			wall_R;

		[MenuItem ("Bridger/Create Level")]
		static void CreateWizard ()
		{
			ScriptableWizard.DisplayWizard<LevelCreator>("Create Level", "Create", "Apply");
		}
		
		void OnWizardCreate ()
		{
			level = new GameObject("LEVEL").transform;
			level.gameObject.AddComponent<ConstructionHandler>().constructionBorder = new Rect(-gapSize*0.5f, gapHeight*0.5f, gapSize, gapHeight);

			floor = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			floor.name = "FLOOR";
			floor.parent = level;
			DestroyImmediate(floor.GetComponent<BoxCollider>());
			floor.gameObject.AddComponent<BoxCollider2D>();

			wall_L = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			wall_L.name = "WALL_L";
			wall_L.parent = level;
			DestroyImmediate(wall_L.GetComponent<BoxCollider>());
			wall_L.gameObject.AddComponent<BoxCollider2D>();

			wall_R = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			wall_R.name = "WALL_R";
			wall_R.parent = level;
			DestroyImmediate(wall_R.GetComponent<BoxCollider>());
			wall_R.gameObject.AddComponent<BoxCollider2D>();

			ApplyValues();
		}
			
		void ApplyValues()
		{
			floor.localScale = new Vector3(gapSize, gapHeight, levelDepth);
			floor.localPosition = -level.up*gapHeight * 0.5f;

			wall_L.localScale = new Vector3(levelDepth, gapHeight*2, levelDepth);
			wall_L.transform.localPosition = -level.right*0.5f*(gapSize + levelDepth);

			wall_R.localScale = new Vector3(levelDepth, gapHeight*2 + gapHeightDelta, levelDepth);
			wall_R.transform.localPosition = level.right*0.5f*(gapSize + levelDepth) + level.up * gapHeightDelta*0.5f;
		}

		void OnWizardUpdate ()
		{
			gapSize -= (gapSize % Grid.gridSize);
			gapSize += ((gapSize % Grid.gridSize > Grid.gridSize*0.5f) ? -Grid.gridSize : 0);
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
	}
}