using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(AgentPatrol))]
	public class AgentPatrolInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_AgentController"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Speed"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Radius"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_MinInterval"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_MaxInterval"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}