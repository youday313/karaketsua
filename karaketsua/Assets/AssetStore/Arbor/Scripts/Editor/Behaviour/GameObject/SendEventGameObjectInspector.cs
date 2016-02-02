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

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Event"));
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}