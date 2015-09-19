using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LowPolyMesh), true)]
public class LowPolyMeshEditor : Editor
{
	LowPolyMesh lowPolyMesh;
	
	void OnEnable()
	{
		lowPolyMesh = target as LowPolyMesh;
	}
	
	public override void OnInspectorGUI ()
	{
		if(GUILayout.Button("Generate"))
		{
			lowPolyMesh.CreateMesh();
		}
		base.OnInspectorGUI ();
		EditorGUILayout.LabelField("Resolution: " + lowPolyMesh.resolution);
		if(lowPolyMesh.validMesh)
		{
			if(GUILayout.Button("ReloadColors"))
			{
				lowPolyMesh.ReloadColor();
			}
		}
		else
		{
			EditorGUILayout.HelpBox("Mesh data is lost, regenerate the mesh to modify!", MessageType.Info, true);
		}
	}
	
	//	void OnSceneGUI()
	//	{
	//
	//	}
}
