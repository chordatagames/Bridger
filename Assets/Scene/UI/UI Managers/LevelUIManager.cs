using UnityEngine;
using System.Collections;
using Bridger;

public class LevelUIManager: MonoBehaviour
{
	public void ReloadLevel()
	{
		Level.Reload();
	}
	public void StartLevel()
	{
		Level.StartPhysics();
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

	public void CompleteLevel()
	{
		Level.Complete();
	}
	public void LoadLevelSelection()
	{
		Level.CloseLevel();
		Application.LoadLevel("LevelMenu");
	}

	public void UnSlowmo()
	{
		Level.UnSlowmo();
	}
}