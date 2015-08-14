using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Bridger
{
//TODO add object pooling from a "Level"-class
	[RequireComponent(typeof(DontLoadDestroy))]
	public class ConstructionHandler : MonoBehaviour
	{
		public static ConstructionHandler instance;
		public Canvas BuildUICanvas;
		public BridgePartType partType;
		public ConstructionMode mode = ConstructionMode.BUILD;

		Vector2 mousePosition{ get{return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);} }
		BridgePart buildingPart;
		BridgePart lastPart;

		public enum ConstructionMode
		{
			BUILD,
			EREASE,
			MODIFY,
			PLAY
		}

		void Awake()
		{
			if(instance != null && instance != this)
			{
				Destroy(this);
			}
			else
			{
				instance = this;
			}
		}

		void Update()
		{
			switch (mode)
			{
			case ConstructionMode.BUILD:
				PointerEventData pe = new PointerEventData(EventSystem.current);
				pe.position = Input.mousePosition;
				
				List<RaycastResult> hits = new List<RaycastResult>();
				EventSystem.current.RaycastAll( pe, hits );
				bool notUI = true;
				
				foreach (RaycastResult h in hits)
				{
					notUI &= (h.gameObject.layer != 5);//if all pass as other layers, notUI will remain true;
				}
				if (notUI)
				{
					DoConstruction();
				}
				break;
			case ConstructionMode.EREASE:
				break;
			case ConstructionMode.MODIFY:
				break;
			default:
				break;
			}
			DoCommands();
		}

		void DoConstruction()
		{

			if(Input.GetMouseButtonDown(0))
			{

				if(buildingPart != null) 
				{
					if(buildingPart.editing)
					{buildingPart.EndStrech();}
					buildingPart = BridgePart.Create(partType, (Input.GetKey(KeyCode.LeftShift) ? buildingPart.partEnd : mousePosition));
				}
				else
				{
					buildingPart = BridgePart.Create(partType, mousePosition);
				}
			}
			if(buildingPart != null)
			{
				if(Input.GetMouseButton(0))
				{
					buildingPart.Strech(mousePosition);
				}
				if(Input.GetMouseButtonUp(0))
				{
					buildingPart.EndStrech();
				}
			}
			
		}

		void DoCommands()
		{
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				if(Input.GetKeyDown(KeyCode.Z))
				{
					Level.Undo();
				}
				if(Input.GetKeyDown(KeyCode.R))
				{
					Level.Redo();
				}
			}
		}

		public void PlayMode()
		{
			mode = ConstructionMode.PLAY;
		}
		public void BuildMode()
		{
			mode = ConstructionMode.BUILD;
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
	}
}