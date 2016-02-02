using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(DistanceTransition))]
	public class DistanceTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Target" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Distance" ) );
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}