using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.MouseButtonUpTransition))]
	public class MouseButtonUpTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Button" ) );
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}