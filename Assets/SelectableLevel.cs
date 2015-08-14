using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectableLevel : MonoBehaviour
{
	public int levelID;

	public GameObject PopupPanel;
	Animator popupAnim;

	public float selectMoveHeight, selectMoveSpeed;

	Bridger.TransformData origin;
	bool selected;

	void Awake()
	{
		origin = new Bridger.TransformData(transform);
		popupAnim = PopupPanel.GetComponent<Animator>();
	}

	void Start()
	{

	}

	public void EnterLevel()
	{
		Application.LoadLevel(levelID);
	}

	public void Select()
	{
		selected = true;
		Debug.Log("yarp" + origin.localPosition);
		StopCoroutine("DeHighlight");
		StartCoroutine("Highlight", origin.localPosition + Vector3.up*selectMoveHeight);
		PopupPanel.SetActive(true);
		popupAnim.SetBool("selected", selected);
	}
	public void DeSelect()
	{
		Debug.Log("narp");
		if(selected)
		{
			selected = false;
			StopCoroutine("Highlight");
			StartCoroutine("DeHighlight", origin.localPosition + Vector3.up*selectMoveHeight);
			origin.Reload(transform);
			popupAnim.SetBool("selected", selected);
		}
	}

	IEnumerator Highlight(Vector3 target)
	{
		while(Vector3.Distance(transform.position, target) > 0.05f )
		{
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
	
	IEnumerator DeHighlight()
	{
		Debug.Log(transform.position + " vs " + origin.localPosition );
		while(Vector3.Distance(transform.position, origin.localPosition) > 0.05f )
		{
			transform.position = Vector3.Lerp(
				transform.position, 
				origin.localPosition, 
				selectMoveSpeed * Time.deltaTime);

			transform.rotation = Quaternion.Lerp(
				transform.rotation, 
				Quaternion.Euler(origin.localRotation),
				selectMoveSpeed * Time.deltaTime);
			
			
			transform.localScale = Vector3.Lerp(
				transform.localScale, 
				origin.localScale,
				selectMoveSpeed * Time.deltaTime);
			
			yield return null;
		}
	}
}
