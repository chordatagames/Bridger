using UnityEngine;
using UnityEngine.EventSystems;


namespace Bridger
{

	[RequireComponent(typeof(UnityEngine.UI.Image))]
	public class BridgeEditorArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		BridgeConstruction visuals;
		static BridgePart currentPart;
		Vector2 pivot = Vector2.one * 0.5f;
		Rect editorArea;

		void OnEnable()
		{
			editorArea = GetComponent<RectTransform>().rect;
			editorArea.position += (Vector2)transform.position;
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
			if(!editorArea.Contains(position))
			{
				BridgeMath.RectangleIntersects(placementOrigin, position, editorArea, out clampedPosition);
				Debug.Log(clampedPosition);
			}
			return clampedPosition;
		}

		void Update()
		{

		}

		public void OnPointerDown(PointerEventData eventData)
		{
			
			if(leftMouseDown(eventData))
			{
				placementOrigin = mousePosition(eventData);
				Debug.Log(placementOrigin);
				//currentPart = BridgePart.Create(
				//	ResourcesManager.Instance.bridgePartPrefabs[ConstructionControl.partType],
				//	Input.GetKey(KeyCode.LeftShift) ? currentPart.partEnd : mousePosition(eventData));
				//currentPart.Stretch(mousePosition(eventData));
			}
			//if(rightMouseDown(eventData))
			//{
			//	if(ConstructionControl.partType < ResourcesManager.Instance.bridgePartPrefabs.Length - 1)
			//	{
			//		ConstructionControl.partType++;
			//	}
			//	else
			//	{
			//		ConstructionControl.partType = 0;
			//	}
			//}

		}

		public void OnDrag(PointerEventData eventData)
		{
			if(leftMouseDown(eventData))
			{
				Debug.DrawLine(placementOrigin, mousePosition(eventData), Color.red);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			//if(leftMouseDown(eventData))
			//{
			//	if(currentPart.EndStretch())
			//	{
			//		ResourcesManager.Instance.audioMaster.pitch = Random.Range(0.45f, 0.65f); // these are magic numbers, but who cares
			//		ResourcesManager.Instance.audioMaster.clip = currentPart.partType.placementSound;
			//		ResourcesManager.Instance.audioMaster.Play();
			//	}
			//}
		}
		void OnDrawGizmosSelected()
		{

		}
	}
}
