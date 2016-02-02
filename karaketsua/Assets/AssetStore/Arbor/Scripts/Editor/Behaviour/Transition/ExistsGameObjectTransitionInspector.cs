using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(ExistsGameObjectTransition))]
	public class ExistsGameObjectTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Targets"),true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_CheckUpdate"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}