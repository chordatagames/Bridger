using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectableLevel : MonoBehaviour
{
	public int levelID;
	public GameObject PopupPanel;

	public Color highlightColor;
	public float highlightHeight, highlightSpeed;
	Vector3 position;

	Renderer renderer;

	void Awake()
	{
		renderer = GetComponent<Renderer>();
	}

	void OnEnable()
	{
		PopupPanel.SetActive(false);
	}

	public void Highlight()
	{
		position = transform.position;
		StartCoroutine(DoHighlight());
		PopupPanel.SetActive(true);
	}

	IEnumerator DoHighlight()
	{
		Vector3 target = position+Vector3.up*highlightHeight;
		while(Vector3.Distance(transform.position, target) < 0.05f )
		{
			if(renderer != null)
			{
				renderer.material.color = Color.Lerp(renderer.material.color, highlightColor, highlightSpeed * Time.deltaTime);
			}
			transform.position = Vector3.Lerp(transform.position, target, highlightSpeed * Time.deltaTime);

			yield return null;
		}
	}
}
