using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.MouseButtonDownTransition))]
	public class MouseButtonDownTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Button" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}