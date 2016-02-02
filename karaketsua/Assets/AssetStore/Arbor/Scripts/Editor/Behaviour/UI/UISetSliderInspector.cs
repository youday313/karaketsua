using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(UISetSlider))]
	public class UISetSliderInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Slider"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Value"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}