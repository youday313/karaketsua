using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.PlaySoundAtPoint))]
	public class PlaySoundAtPointInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Clip") );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Target") );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Volume") );

			serializedObject.ApplyModifiedProperties();
		}
	}
}