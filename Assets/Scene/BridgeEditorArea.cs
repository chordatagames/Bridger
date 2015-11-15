using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Bridger
{
	[RequireComponent(typeof(UnityEngine.UI.Image))]
	public class BridgeEditorArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		static BridgePart currentPart;
		Rect editorArea{ get { return( transform as RectTransform ).rect; } }
		Vector2 pivot = Vector2.one*0.5f;

		Vector2 mousePosition(PointerEventData eventData)
		{
			Vector2 position = (Vector2)eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
            Vector2 relativePos = ( ( position - (Vector2)transform.position ) - editorArea.center );
            if(!editorArea.Contains(position))
			{

				float tanAspect = editorArea.width/editorArea.height;
                float cotAspect = 1 / tanAspect;

                float relativeTanAspect = relativePos.x / relativePos.y;

                float tan = Mathf.Abs(cotAspect*(relativeTanAspect));
                float cot = tanAspect*(1/tan); //no need for abs as we're already operating with only positives

                Vector2 sign = new Vector2(Mathf.Sign(relativePos.x), Mathf.Sign(relativePos.y));

                if( relativeTanAspect < tanAspect )
                {
                    position = new Vector2(tan * sign.x, editorArea.height * 0.5f * sign.y);
                }
                else
                {
                    position = new Vector2(editorArea.width * 0.5f * sign.x, cot * sign.y);
                }
            }
			return position;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			currentPart = BridgePart.Create(
				ConstructionControl.materialControl.currentMaterial, 
				Input.GetKey(KeyCode.LeftShift) ? currentPart.partEnd : mousePosition(eventData));
			currentPart.StartStretch();
		}

		public void OnDrag(PointerEventData eventData)
		{
			currentPart.Stretch(mousePosition(eventData));
		}
		
		public void OnPointerUp(PointerEventData eventData)
		{
			currentPart.EndStretch();
		}
	}
}
