using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelSelector))]
public class LevelSelectorEditor : Editor
{
	private const float handleSize = 0.04f;
	private const float pickSize = 0.1f;

	LevelSelector levelSelector;

	private Transform handleTransform;
	private Quaternion handleRotation;
	private int selectedIndex = -1;

	void OnEnable()
	{
		levelSelector = target as LevelSelector;

		handleTransform = levelSelector.transform;
		handleRotation = Quaternion.identity;
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
	}

	void OnSceneGUI()
	{

		for(int i = 0; i < levelSelector.availableLevels.Length; i++)
		{
			ShowSelector(i);
		}
//		Vector3 lineStart = curve.GetPoint(0f);
//
//		for (int i = 1; i <= curveSteps; i++) {
//			Vector3 lineEnd = curve.GetPoint(i / (float)curveSteps);
//			Handles.DrawLine(lineStart, lineEnd);
//			lineStart = lineEnd;
//		}
	}

	Vector3 ShowSelector(int index)
	{
		Handles.color = Color.red;
		Handles.SphereCap(index,levelSelector.availableLevels[index].position, Quaternion.identity,0.3f);
		Vector3 levelPoint = levelSelector.availableLevels[index].selectableLevel.transform.position;
		Handles.DrawLine(levelSelector.availableLevels[index].position, levelPoint);

		Handles.color = Color.black;

		if (Handles.Button(levelPoint, handleRotation, handleSize, pickSize, Handles.DotCap)) {
			selectedIndex = index;
		}

		if (selectedIndex == index)
		{
			EditorGUI.BeginChangeCheck();
			levelPoint = Handles.DoPositionHandle(levelPoint, handleRotation);

			if (EditorGUI.EndChangeCheck()) 
			{
				Undo.RecordObject(levelSelector.availableLevels[index].selectableLevel.transform, "Change selectable level");
				EditorUtility.SetDirty(levelSelector.availableLevels[index].selectableLevel);
				levelSelector.availableLevels[index].selectableLevel.transform.position = levelPoint;
			}
		}
		return levelPoint;
	}
}

[CustomPropertyDrawer (typeof (SelectableLevelCameraPoint))]
public class SelectableLevelCameraPointDrawer : PropertyDrawer
{
	const int width = 200;
	const float min = 0;
	const float max = 1;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight*3;
	}

	public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label)
	{
		SerializedProperty selectableLevel = prop.FindPropertyRelative ("selectableLevel");
		SerializedProperty position = prop.FindPropertyRelative ("position");

		Color guiColor = GUI.color;
		GUI.color = new Color(75f/255f, 190f/255f, 46f/255f, 1f);
		GUI.Box(pos,"");
		GUI.color = guiColor;

		EditorGUI.PropertyField(
			new Rect (pos.x, pos.y, pos.width, pos.height/3),
			selectableLevel, new GUIContent(selectableLevel.objectReferenceValue.name));

		EditorGUI.PrefixLabel(
			new Rect (pos.x+40, pos.y+pos.height/3, pos.width-40, pos.height/3),
			new GUIContent("Camera Position:"));

		position.vector3Value = EditorGUI.Vector3Field(
			new Rect (pos.x+40, pos.y+pos.height*2/3, pos.width-40, pos.height/3),
				"",position.vector3Value);

		if(GUI.Button(new Rect (pos.x, pos.y+pos.height*2/3, 55, pos.height/3), "Set "))
		{
			position.vector3Value = SceneView.lastActiveSceneView.camera.transform.position;
		}
	}
}

