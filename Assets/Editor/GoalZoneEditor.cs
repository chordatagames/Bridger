using UnityEngine;
using UnityEditor;
using Bridger;
using System.Collections;

namespace BridgerEditor
{
	[CustomEditor(typeof(GoalZone))]
	public class GoalZoneEditor : Editor
	{
		GoalZone zone;
        
		void OnSceneGUI()
		{
			zone = target as GoalZone;
			Handles.color = Color.blue;

		}
	}
}