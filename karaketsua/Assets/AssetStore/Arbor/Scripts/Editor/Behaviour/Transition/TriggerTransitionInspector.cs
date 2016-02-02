using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(TriggerTransition))]
	public class TriggerTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Message" ) );
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}