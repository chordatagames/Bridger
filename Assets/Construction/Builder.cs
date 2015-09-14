using UnityEngine;
using UnityEngine.EventSystems;
namespace Bridger
{
	public class Builder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		static Vector2 mousePosition{ get{return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);} }
		static BridgePart currentPart;

		void Update()
		{
//			if(currentPart != null)
//			{
//				if(currentPart.editing)
//				{
//					currentPart.Stretch(mousePosition);
//				}
//			}
		}
		public void OnPointerDown(PointerEventData eventData)
		{
			currentPart = BridgePart.Create(ConstructionControl.materialControl.currentMaterial, eventData.position);
			currentPart.StartStretch();
		}

		public void OnDrag(PointerEventData eventData)
		{
			currentPart.Stretch(eventData.position);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			currentPart.EndStretch();
		}
	}
}