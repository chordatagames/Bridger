using UnityEngine;
using UnityEngine.SceneManagement;
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

	public void LoadLevelSelection()
	{
		Level.ResetLevel();
        SceneManager.LoadScene("LevelMenu");
	}

	public void UnSlowmo()
	{
		Level.UnSlowmo();
	}

}