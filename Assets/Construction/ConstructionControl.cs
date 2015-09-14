using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Bridger
{
	
	public class ConstructionControl : MonoBehaviour
	{
		public Image materialPreview;//TODO move somewhere else.

		public static ConstructionControl materialControl{get{return ConstructionHandler.materialControl;}}
		public BridgePartType[] partMaterials;
		public BridgePartType currentMaterial{get{return partMaterials[currentMaterialIndex];} }
		int currentMaterialIndex = 0;

		public RectTransform materialSelection;

		float mouseDownTime = 0.0f, desiredMouseDownTime = 5.0f;

		void Start()
		{
			ConstructionHandler.instance.partType = partMaterials[currentMaterialIndex];
			materialSelection = Instantiate<RectTransform>(materialSelection);
		}

		void Update()
		{

			if(Input.GetMouseButtonDown(1))//RightClick
			{
				ResetMaterialSelection();
//				currentMaterialIndex += ((currentMaterialIndex < partMaterials.Length-1) ? 1 : -currentMaterialIndex);
//				ConstructionHandler.instance.partType = partMaterials[currentMaterialIndex];
				if(mouseDownTime >= desiredMouseDownTime)
				{
					OpenMaterialSelection();
				}
				desiredMouseDownTime += Time.deltaTime;
			}
			else
			{
				mouseDownTime = 0;
			}
		}

		void OpenMaterialSelection()
		{	
			materialSelection.gameObject.SetActive(true);
		}
		void ResetMaterialSelection()
		{
			materialSelection.gameObject.SetActive(false);
		}

		Sprite ConstructMaterialPreview()
		{
			return Sprite.Create(currentMaterial.partPreviewTexture, new Rect(0,0,32,32), Vector2.zero);
		}
	}
}