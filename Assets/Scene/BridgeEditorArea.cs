using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Bridger
{
	[RequireComponent(typeof(UnityEngine.UI.Image))]
	public class BridgeEditorArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		static BridgePart currentPart;
		Rect editorArea{get{return (transform as RectTransform).rect;}}
		Vector2 pivot = Vector2.one*0.5f;

		Vector2 mousePosition(PointerEventData eventData)
		{
			Vector2 position = (Vector2)eventData.pressEventCamera.ScreenToWorldPoint(eventData.position);
			if(!editorArea.Contains(position))
			{
				float aspect = editorArea.height/editorArea.width;

				float tan = Mathf.Abs(position.y/position.x);
				float cot = Mathf.Abs(position.x/position.y);
				position = new Vector2(
					((tan <= aspect) ? editorArea.width * pivot.x : cot * editorArea.height * pivot.y) * Mathf.Sign(position.x),
					((tan > aspect) ? editorArea.height * pivot.y : tan * editorArea.width * pivot.x)) * Mathf.Sign(position.y);
			}
			//Vektor[( ((tangens <= f) ? a / 2 : cotangens b / 2) Methf.sign(position.x), (tangens <= f), tangens a / 2, b / 2] sgn(y(F)))]

//			position = new Vector2( Mathf.Clamp(position.x,(position-editorArea.min).x,(position-editorArea.max).x),  Mathf.Clamp(position.y,(position-editorArea.min).y,(position-editorArea.max).y));//TODO normalize to rect
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
