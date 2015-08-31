using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.DestroyGameObject))]
	public class DestroyGameObjectInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Target" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}