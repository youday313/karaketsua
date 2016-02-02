using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(MouseButtonDownTransition))]
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