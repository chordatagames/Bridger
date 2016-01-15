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
			Vector2 position = (Vector2)eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
            Vector2 newPosition = position;
            if(!editorArea.Contains(position))
			{
                if(currentPart != null)
                {
                    //position -= currentPart.partOrigin;
                }
				float tanAspect = editorArea.width/editorArea.height; //positive
                float cotAspect = 1 / tanAspect; //positive

                float relativeTanAspect = Mathf.Abs(position.x / position.y);

                float tan = cotAspect*(relativeTanAspect);
                float cot = tanAspect*(1/tan); //no need for abs as we're already operating with only positives

                Vector2 sign = new Vector2(Mathf.Sign(position.x), Mathf.Sign(position.y));

                if( relativeTanAspect < tanAspect )
                {
                    
                    newPosition = new Vector2(tan * sign.x * editorArea.width, sign.y * editorArea.height) * Grid.gridSize;//RED
                }
                else
                {
                    newPosition = new Vector2(sign.x * editorArea.width, cot * cotAspect * sign.y * editorArea.height) * Grid.gridSize;//BLUE
                }
                if(currentPart != null)
                {
                    Debug.DrawLine(Vector3.zero, (Vector3)position, Color.yellow);
                    Debug.DrawLine((Vector3)currentPart.partOrigin, new Vector3(tan * sign.x * editorArea.width, sign.y * editorArea.height) * Grid.gridSize, Color.red);
                    Debug.DrawLine((Vector3)currentPart.partOrigin, new Vector3(sign.x * editorArea.width, cot*cotAspect * sign.y * editorArea.height) * Grid.gridSize, Color.blue);
                }

            }
            
			return newPosition;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
	        if(leftMouseDown(eventData))
	        {
			    currentPart = BridgePart.Create(
			    	ConstructionControl.materialControl.currentMaterial, 
			    	Input.GetKey(KeyCode.LeftShift) ? currentPart.partEnd : mousePosition(eventData));
				currentPart.Stretch(mousePosition(eventData));
			}
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
