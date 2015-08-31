using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.AnyKeyTransition))]
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