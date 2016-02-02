using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(ActivateGameObject))]
	public class ActivateGameObjectInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Target" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_BeginActive" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_EndActive" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}