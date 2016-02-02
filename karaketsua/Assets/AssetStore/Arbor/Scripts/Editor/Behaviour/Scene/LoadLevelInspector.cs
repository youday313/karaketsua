using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(LoadLevel))]
	public class LoadLevelInspector : Editor
	{
		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_LevelName" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}