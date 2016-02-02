using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(ButtonDownTransition))]
	public class ButtonDownTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_ButtonName" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}