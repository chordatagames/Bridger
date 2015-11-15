using UnityEngine;
using System.Collections;
using Bridger;

public class LevelUIManager: MonoBehaviour
{
	public Transform decorations;

	public void ReloadLevel()
	{
		Level.Reload();
	}
	public void StartLevel()
	{
		Level.StartPhysics();
		Renderer r;
		foreach(Transform t0 in decorations)
		{
			foreach(Transform t in t0)
			{
				r = t.GetComponent<Renderer>();
				if(r != null)
				{
					r.material.color += new Color(0,0,0,1);
//					r.material.SetFloat("Z-Depth", -127);
				}
//				
			}
		}
	}
	public void ClearLevel()
	{
		Level.Clear();
	}
	public void Undo()
	{
		Level.Undo();
	}
	public void Redo()
	{
		Level.Redo();//Why was this Level.Undo() before, that's just stupid.
	}

	public void LoadLevelSelection()
	{
		Level.ResetLevel();
		Application.LoadLevel("LevelMenu");
	}

	public void UnSlowmo()
	{
		Level.UnSlowmo();
	}

}