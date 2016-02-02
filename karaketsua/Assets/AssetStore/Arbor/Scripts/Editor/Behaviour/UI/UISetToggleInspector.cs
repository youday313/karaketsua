using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(UISetToggle))]
	public class UISetToggleInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Toggle"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Value"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}