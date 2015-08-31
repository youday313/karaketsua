using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(AgentController))]
	public class AgentControllerInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Agent"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_SpeedParameter"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}