using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(PlaySound))]
	public class PlaySoundInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_AudioSource") );

			SerializedProperty isSetClipProperty = serializedObject.FindProperty("_IsSetClip");

			EditorGUILayout.PropertyField(isSetClipProperty);

			if (isSetClipProperty.boolValue)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_Clip"));
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}