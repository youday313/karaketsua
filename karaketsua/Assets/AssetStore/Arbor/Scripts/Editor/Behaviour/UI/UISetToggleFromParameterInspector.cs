using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(UISetToggleFromParameter))]
	public class UISetToggleFromParameterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Toggle"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Parameter"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_ChangeTimingUpdate"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}