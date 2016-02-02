using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(DestroyGameObject))]
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