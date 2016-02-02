using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(UISetSliderFromParameter))]
	public class UISetSliderFromParameterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Slider"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Parameter"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_ChangeTimingUpdate"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}