using UnityEngine;
using System.Collections;

namespace Bridger
{
//TODO add object pooling from a "Level"-class
	public class ConstructionHandler : MonoBehaviour
	{
		public BridgePartType partType;
		public ConstructionMode mode = ConstructionMode.BUILD;

		Vector2 mousePosition{ get{return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);} }
		BridgePart currentPart;

		public enum ConstructionMode
		{
			BUILD,
			EREASE,
			MODIFY
		}

		void Awake()
		{
			DontDestroyOnLoad(this);
		}

		void Update()
		{
			DoConstruction();
			DoCommands();
		}

		void DoConstruction()
		{
			switch (mode)
			{
			case ConstructionMode.BUILD:
				if(Input.GetMouseButtonDown(0))
				{
					if(currentPart != null && Input.GetKey(KeyCode.LeftShift))
					{
						currentPart = BridgePart.Create(partType, currentPart.partEnd);
					}
					else
					{
						currentPart = BridgePart.Create(partType, mousePosition);
					}
				}
				if(currentPart != null)
				{
					if(Input.GetMouseButton(0))
					{
						currentPart.Strech(mousePosition);
					}
					if(Input.GetMouseButtonUp(0))
					{
						Debug.Log("Should Test!");
						if(currentPart.partLength < Grid.gridSize)
						{
							Destroy(currentPart.gameObject);
						}
						else
						{
						currentPart.EndStrech();
						}
					}
				}
				break;
			case ConstructionMode.EREASE:
				break;
			case ConstructionMode.MODIFY:
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
					Level.Undo(Level.currentItem);
				}
				if(Input.GetKeyDown(KeyCode.R))
				{
					Level.Redo(Level.lastItem);
				}
			}

		}
	}
}