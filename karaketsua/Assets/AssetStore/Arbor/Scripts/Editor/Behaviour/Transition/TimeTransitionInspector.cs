using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.TimeTransition))]
	public class TimeTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Seconds" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}