using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(AnyKeyDownTransition))]
	public class AnyKeyDownTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_CheckDown" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}