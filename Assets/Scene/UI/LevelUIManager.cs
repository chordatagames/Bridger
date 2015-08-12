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

}