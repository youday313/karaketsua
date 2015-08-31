using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.OnTriggerStay2DTransition))]
	public class OnTriggerStay2DTransitionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update ();
			
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "_IsCheckTag" ) );

			SerializedProperty tagProperty = serializedObject.FindProperty( "_Tag" );
			
			EditorGUI.BeginChangeCheck();
			string tag = EditorGUILayout.TagField( ObjectNames.NicifyVariableName(tagProperty.name), tagProperty.stringValue );
			if( EditorGUI.EndChangeCheck() )
			{
				tagProperty.stringValue = tag;
			}
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}