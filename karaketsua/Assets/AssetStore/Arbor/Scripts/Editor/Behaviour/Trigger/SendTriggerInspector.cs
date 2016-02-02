using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(SendTrigger))]
	public class SendTriggerInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Target" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Message" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}