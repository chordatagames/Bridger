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
		Level.Undo();
	}

	public void UnSlowmo()
	{
		Level.UnSlowmo();
	}

}