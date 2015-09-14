using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Bridger
{

	[RequireComponent(typeof(DontLoadDestroy), typeof(ConstructionControl))]
	public class ConstructionHandler : MonoBehaviour
	{
		//Some sort of singleton control
		private static ConstructionHandler _instance;
		public static ConstructionHandler instance{get{return _instance;}}

		private static ConstructionControl _materialControl;
		public static ConstructionControl materialControl{get{return _materialControl;}}

		void Awake()
		{
			if(_instance != null && _instance != this)
			{
				Destroy(this);
			}
			else
			{
				_instance = this;
				_materialControl = GetComponent<ConstructionControl>();
			}
		}


		public GameObject jointBase;
		public BridgePartType partType;
		public LayerMask blocksConstruction;
		public Rect constructionBorder;

		Vector2 mousePosition{ get{return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);} }
		BridgePart buildingPart;
		BridgePart lastPart;


		void Update()
		{
			switch (Level.mode)
			{
			case Level.LevelMode.BUILD:
				DoCommands();
				break;
			default:
				break;
			}

		}

		void DoCommands()
		{
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if(Input.GetKeyDown(KeyCode.Z))
				{
					Level.Undo();
					buildingPart = Level.undoStack.Peek() as BridgePart;
				}

				if(Input.GetKeyDown(KeyCode.R))
				{
					buildingPart = Level.redoStack.Peek() as BridgePart;
					Level.Redo();
				}
			}
		}
		
		public void SlowMo(float transitionTime)
		{
			StartCoroutine("SlowMotion", transitionTime);
		}

		public void UnSlowMo()
		{
			StopCoroutine("SlowMotion");
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}

		IEnumerator SlowMotion(float transitionTime)
		{
			while(Time.timeScale != Level.slowMotionTimeScale)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, Level.slowMotionTimeScale, transitionTime * Time.deltaTime/Time.timeScale);//may cause weird behaviour
				if (Time.timeScale < Level.slowMotionTimeScale)
				{
					Time.timeScale = Level.slowMotionTimeScale;
				}
				Time.fixedDeltaTime = 0.02F * Time.timeScale; //by default 30 times pr sec 0.02*1
				yield return null;
			}
		}

		IEnumerator UnSlowMotion(float transitionTime)
		{
			while(Time.timeScale != Level.slowMotionTimeScale)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, Level.slowMotionTimeScale, transitionTime * Time.deltaTime/Time.timeScale);//may cause weird behaviour
				if (Time.timeScale < Level.slowMotionTimeScale)
				{
					Time.timeScale = Level.slowMotionTimeScale;
				}
				Time.fixedDeltaTime = 0.02F * Time.timeScale; //by default 30 times pr sec 0.02*1
				yield return null;
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine((Vector3)constructionBorder.min, new Vector3(constructionBorder.xMax, constructionBorder.yMin));
			Gizmos.DrawLine(new Vector3(constructionBorder.xMax, constructionBorder.yMin), (Vector3)constructionBorder.max);
			Gizmos.DrawLine((Vector3)constructionBorder.max, new Vector3(constructionBorder.xMin, constructionBorder.yMax));
			Gizmos.DrawLine(new Vector3(constructionBorder.xMin, constructionBorder.yMax), (Vector3)constructionBorder.min);

		}
	}
}