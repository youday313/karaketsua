using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(AddForceRigidbody2D))]
	public class AddForceRigidbody2DInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Target"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Angle"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Power"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
