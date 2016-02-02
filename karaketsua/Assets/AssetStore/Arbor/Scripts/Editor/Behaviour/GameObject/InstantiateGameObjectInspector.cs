using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(InstantiateGameObject))]
	public class InstantiateGameObjectInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Prefab" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Parent" ) );
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_InitTransform"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Parameter"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
