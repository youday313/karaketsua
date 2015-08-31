using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.LoadLevel))]
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