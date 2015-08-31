using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.ActivateGameObject))]
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