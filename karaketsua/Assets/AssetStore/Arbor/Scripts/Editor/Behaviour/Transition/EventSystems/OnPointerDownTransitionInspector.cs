using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(OnPointerDownTransition))]
	public class OnPointerDownTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_CheckButton"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Button"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}