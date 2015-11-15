using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Bridger
{
//TODO add object pooling from a "Level"-class
	[RequireComponent(typeof(DontLoadDestroy), typeof(ConstructionControl))]
    public class ConstructionHandler : MonoBehaviour
	{
		public static ConstructionHandler instance;
		public GameObject jointBase;
		public BridgePartType partType;
		public LayerMask blocksConstruction;
		public Rect constructionBorder;
		public AudioSource audioSource;

        private static ConstructionControl _materialControl;
        public static ConstructionControl materialControl { get { return _materialControl; } }

        Vector2 mousePosition{ get{return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);} }
		BridgePart buildingPart;
		BridgePart lastPart;



		void Awake()
		{
			if(instance != null && instance != this)
			{
				Destroy(this);
			}
			else
			{
				instance = this;
                _materialControl = GetComponent<ConstructionControl>();
			}
		}

		void Update()
		{
			switch (Level.mode)
			{
			case Level.LevelMode.BUILD:
				PointerEventData pe = new PointerEventData(EventSystem.current);
				pe.position = Input.mousePosition;
				
				List<RaycastResult> hits = new List<RaycastResult>();
				EventSystem.current.RaycastAll( pe, hits );
				
				bool notUI = true;
				
				foreach (RaycastResult h in hits)
				{
					notUI &= (h.gameObject.layer != 5);//if all pass as other layers, notUI will remain true;
				}
				if(notUI)
				{
					DoConstruction();
				}

				DoCommands();
				break;
			default:
				break;
			}

		}

		void DoConstruction()
		{
			bool obstructed = Physics2D.OverlapPoint(
				(buildingPart != null) ? ((buildingPart.editing) ? buildingPart.partEnd : mousePosition) : mousePosition,
				blocksConstruction
			) != null; // this is a work of art and i refuse to hear otherwise

			if(Input.GetMouseButtonDown(0))
			{
				if(!obstructed && constructionBorder.Contains(mousePosition))
				{
					if(buildingPart != null) 
					{
						if(buildingPart.editing)
						{
							buildingPart.EndStrech();
						}
						buildingPart = BridgePart.Create(partType, (Input.GetKey(KeyCode.LeftShift) ? buildingPart.partEnd : mousePosition));
					}
					else
					{
						buildingPart = BridgePart.Create(partType, mousePosition);
					}
					audioSource.pitch = Random.Range(0.3f,0.7f); // these are magic numbers, but who cares
					audioSource.clip = partType.placementSound;
					audioSource.Play();
				}

			}
			if(buildingPart != null)
			{
				if(buildingPart.editing)
				{
					buildingPart.Strech(mousePosition);
				}
				if(Input.GetMouseButtonUp(0))
				{
					if(!obstructed && constructionBorder.Contains(mousePosition))
					{
						buildingPart.EndStrech();
					}
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
	}
}