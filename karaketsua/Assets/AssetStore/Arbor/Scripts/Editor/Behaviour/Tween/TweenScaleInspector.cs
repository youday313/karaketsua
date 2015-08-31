using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.TweenScale))]
	public class TweenScaleInspector : TweenBaseInspector
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			
			DrawBase();
			
			EditorGUILayout.Space();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Target" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_From" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_To" ) );
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}