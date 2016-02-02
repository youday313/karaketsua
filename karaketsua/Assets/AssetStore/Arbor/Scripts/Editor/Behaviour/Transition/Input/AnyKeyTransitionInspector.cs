using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(AnyKeyTransition))]
	public class AnyKeyTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_CheckOn" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}