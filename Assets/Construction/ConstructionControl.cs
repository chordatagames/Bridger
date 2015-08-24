using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Bridger
{
	public class ConstructionControl : MonoBehaviour
	{
		ConstructionHandler ch;
		public Image materialPreview;

		public BridgePartType[] partMaterials;
		int currentMaterial = 0;
		
		void Start()
		{
			ch = GetComponent<ConstructionHandler>();
			ch.partType = partMaterials[currentMaterial];
			materialPreview.sprite = ConstructMaterialPreview();
		}
		void Update()
		{
//			if(Input.GetKeyDown(KeyCode.X))
//			{
//				ch.mode = (ch.mode == ConstructionHandler.ConstructionMode.BUILD) ? 
//						ConstructionHandler.ConstructionMode.EREASE :
//						ConstructionHandler.ConstructionMode.BUILD;
//			}
			if(Input.GetMouseButtonDown(1))//RightClick
			{
				currentMaterial += ((currentMaterial < partMaterials.Length-1) ? 1 : -currentMaterial);
				ch.partType = partMaterials[currentMaterial];
				materialPreview.sprite = ConstructMaterialPreview();
			}
		}
		Sprite ConstructMaterialPreview()
		{
			return Sprite.Create(ch.partType.partPreviewTexture, new Rect(0,0,32,32), Vector2.zero);
		}
	}
}