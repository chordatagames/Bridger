using UnityEngine;
using UnityEngine.EventSystems;

namespace Bridger
{

	[RequireComponent(typeof(UnityEngine.UI.Image))]
	public class BridgeEditorArea : MonoBehaviour, IPointerDownHandler, /*IPointerUpHandler,*/ IDragHandler
	{
		BridgeConstruction visuals;
		static BridgePart currentPart;
		Vector2 pivot = Vector2.one * 0.5f;
		Rect _editorArea;
		Rect editorArea
		{
			get
			{
				_editorArea = GetComponent<RectTransform>().rect;
				return _editorArea;
			}
		}

		Vector2 placementOrigin;

		private bool leftMouseDown(PointerEventData eventData)
		{
			return eventData.button == PointerEventData.InputButton.Left;
		}
		private bool rightMouseDown(PointerEventData eventData)
		{
			return eventData.button == PointerEventData.InputButton.Right;
		}

		Vector2 mousePosition(PointerEventData eventData)
		{
			Vector2 position = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
			Vector2 clampedPosition = position;
            if (!editorArea.Contains(position))
			{
				Vector2 stretch = position - placementOrigin;
				Vector2 deltaSize = editorArea.max - placementOrigin;

				if (stretch.x / stretch.y > deltaSize.x / deltaSize.y)
				{
					float x = deltaSize.y * stretch.x / stretch.y;
					float y = deltaSize.y;
					clampedPosition.Set(x, y);
				}
				
			}
			
			return clampedPosition;
		}

		void Update()
		{
			
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			placementOrigin = mousePosition(eventData);
			//        if(leftMouseDown(eventData))
			//        {
			//		    currentPart = BridgePart.Create(
			//		        ResourcesManager.Instance.bridgePartPrefabs[ConstructionControl.partType], 
			//		    	Input.GetKey(KeyCode.LeftShift) ? currentPart.partEnd : mousePosition(eventData));
			//			currentPart.Stretch(mousePosition(eventData));
			//		}
			//           if(rightMouseDown(eventData))
			//           {
			//               if(ConstructionControl.partType < ResourcesManager.Instance.bridgePartPrefabs.Length-1)
			//               {
			//                   ConstructionControl.partType++;
			//               }
			//               else
			//               {
			//                   ConstructionControl.partType = 0;
			//               }
			//           }

		}

		public void OnDrag(PointerEventData eventData)
		{
			Debug.DrawLine(placementOrigin, mousePosition(eventData), Color.red);
			
		}

		//	public void OnPointerUp(PointerEventData eventData)
		//	{
		//           if (leftMouseDown(eventData))
		//           {
		//               if(currentPart.EndStretch())
		//               {
		//                   ResourcesManager.Instance.audioMaster.pitch = Random.Range(0.45f, 0.65f); // these are magic numbers, but who cares
		//                   ResourcesManager.Instance.audioMaster.clip = currentPart.partType.placementSound;
		//                   ResourcesManager.Instance.audioMaster.Play();
		//               }
		//           }
		//	}
		//}
		//void OnDrawGizmosSelected()
		//{

		//}
	}

}
