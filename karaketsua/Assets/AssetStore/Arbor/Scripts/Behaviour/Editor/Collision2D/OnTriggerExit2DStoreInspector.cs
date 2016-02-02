using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(OnTriggerExit2DStore))]
	public class OnTriggerExit2DStoreInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_IsCheckTag"));

			SerializedProperty tagProperty = serializedObject.FindProperty("_Tag");

			EditorGUI.BeginChangeCheck();
			string tag = EditorGUILayout.TagField(ObjectNames.NicifyVariableName(tagProperty.name), tagProperty.stringValue);
			if (EditorGUI.EndChangeCheck())
			{
				tagProperty.stringValue = tag;
			}

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Parameter"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
