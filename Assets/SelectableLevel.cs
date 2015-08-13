using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectableLevel : MonoBehaviour
{
	public int levelID;
	public GameObject PopupPanel;

	public Color selectedColor;
	public float selectMoveHeight, selectMoveSpeed;

	Bridger.TransformData origin;
	bool highlit;

	Renderer renderer;

	void Awake()
	{
		renderer = GetComponent<Renderer>();
	}

	void Start()
	{
		PopupPanel.SetActive(false);
		origin = new Bridger.TransformData(transform);
	}

	public void Select()
	{
		StartCoroutine("SelectMove", origin.localPosition + Vector3.up*selectMoveHeight);
		PopupPanel.SetActive(true);
		highlit = true;
	}
	public void UnSelect()
	{
		if(highlit)
		{
			StopCoroutine("SelectMove");
			origin.Reload(transform);
			highlit = false;
		}
	}

	IEnumerator SelectMove(Vector3 target)
	{
		while(Vector3.Distance(transform.position, target) > 0.05f )
		{
			if(renderer != null)
			{
				renderer.material.color = Color.Lerp(
					renderer.material.color, 
					selectedColor, 
					selectMoveSpeed * Time.deltaTime);
			}

			transform.position = Vector3.Lerp(
				transform.position, 
				target, 
				selectMoveSpeed * Time.deltaTime);

			transform.Rotate(
				Vector3.up,
				Mathf.LerpAngle(0,180, selectMoveSpeed * Time.deltaTime));

			transform.localScale = Vector3.Lerp(
				transform.localScale, 
				origin.localScale + Vector3.one * selectMoveHeight,
				selectMoveSpeed * Time.deltaTime);

			yield return null;
		}
	}
}
