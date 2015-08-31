using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.InstantiateGameObject))]
	public class InstantiateGameObjectInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Prefab" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Parent" ) );
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}