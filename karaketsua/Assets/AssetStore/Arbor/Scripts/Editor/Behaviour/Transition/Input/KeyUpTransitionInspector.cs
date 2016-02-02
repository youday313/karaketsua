using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(KeyUpTransition))]
	public class KeyUpTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_KeyCode" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}