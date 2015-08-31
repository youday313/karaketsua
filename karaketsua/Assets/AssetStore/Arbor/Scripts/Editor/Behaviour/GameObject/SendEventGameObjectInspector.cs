using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(SendEventGameObject))]
	public class SendEventGameObjectInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty typeProperty = serializedObject.FindProperty("_Type");
			EditorGUILayout.PropertyField(typeProperty);

			SendEventGameObject.Type type = (SendEventGameObject.Type)typeProperty.enumValueIndex;

			switch (type)
			{
				case SendEventGameObject.Type.None:
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_Event"));
					break;
				case SendEventGameObject.Type.Int:
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_IntValue"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_IntEvent"));
					break;
				case SendEventGameObject.Type.Float:
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_FloatValue"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_FloatEvent"));
					break;
				case SendEventGameObject.Type.Bool:
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_BoolValue"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_BoolEvent"));
					break;
				case SendEventGameObject.Type.String:
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_StringValue"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("_StringEvent"));
					break;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}