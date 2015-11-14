using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LowPolyTerrain))]
public class LowPolyTerrainEditor : Editor
{
	LowPolyTerrain terrain;

	void OnEnable()
	{
		terrain = target as LowPolyTerrain;
	}

	public override void OnInspectorGUI ()
	{
		if(GUILayout.Button("Generate"))
		{
			terrain.GenerateTerrain();
		}
		base.OnInspectorGUI ();

		if(GUILayout.Button("ReloadColors"))
		{
			terrain.ReloadColor();
		}
	}

//	void OnSceneGUI()
//	{
//
//	}
}
