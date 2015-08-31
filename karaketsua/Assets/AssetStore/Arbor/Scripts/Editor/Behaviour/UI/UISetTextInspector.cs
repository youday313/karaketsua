using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(UISetText))]
	public class UISetTextInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Text"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_String"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}