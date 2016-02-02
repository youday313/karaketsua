using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(ButtonUpTransition))]
	public class ButtonUpTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_ButtonName" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}