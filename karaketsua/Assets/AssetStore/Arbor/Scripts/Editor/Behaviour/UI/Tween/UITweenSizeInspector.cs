using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.UITweenSize))]
	public class UITweenSizeInspector : TweenBaseInspector
	{
		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			DrawBase();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Target" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_From" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty("_To") );

			serializedObject.ApplyModifiedProperties();
		}
	}
}