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
			materialPreview.sprite = ConstructMaterialPreview(currentMaterial);
		}
		void Update()
		{
			if(Input.GetKeyDown(KeyCode.X))
			{
				ch.mode = (ch.mode == ConstructionHandler.ConstructionMode.BUILD) ? 
						ConstructionHandler.ConstructionMode.EREASE :
						ConstructionHandler.ConstructionMode.BUILD;
			}
			if(Input.GetMouseButtonDown(1))//RightClick
			{
				currentMaterial += ((currentMaterial < partMaterials.Length-1) ? 1 : -currentMaterial);
				ch.partType = partMaterials[currentMaterial];
//				materialPreview.sprite = ConstructMaterialPreview(currentMaterial);
			}
		}
		Sprite ConstructMaterialPreview(int index)
		{
			//Sprite s = Sprite.Create(UnityEditor.AssetPreview.GetAssetPreview(availableMaterials.materialsObjects[currentMaterial]), new Rect(0, 0, 128, 128), Vector2.one * 0.5f);
			return new Sprite();
		}
	}
}