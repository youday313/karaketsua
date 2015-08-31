using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class TweenBaseInspector : Editor
	{
		protected void DrawBase()
		{
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Type" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Duration" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Curve" ) );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_UseRealtime" ) );
		}
	}
}