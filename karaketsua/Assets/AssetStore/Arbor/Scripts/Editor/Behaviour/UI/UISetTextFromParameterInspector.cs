using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(UISetTextFromParameter))]
	public class UISetTextFromParameterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_Text"));

			SerializedProperty parameterProperty = serializedObject.FindProperty("_Parameter");
            EditorGUILayout.PropertyField(parameterProperty);

			SerializedProperty containerProperty = parameterProperty.FindPropertyRelative("container");

			ParameterContainerBase containerBase = containerProperty.objectReferenceValue as ParameterContainerBase;
			ParameterContainer container = null;
			if (containerBase != null)
			{
				container = containerBase.defaultContainer as ParameterContainer;
			}

			if (container != null)
			{
				SerializedProperty idProperty = parameterProperty.FindPropertyRelative("id");

				Parameter parameter = container.GetParam(idProperty.intValue);

				if (parameter != null)
				{
					if (parameter.type != Parameter.Type.Bool && parameter.type != Parameter.Type.GameObject)
					{
						EditorGUILayout.PropertyField(serializedObject.FindProperty("_Format"));
					}
				}
			}

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_ChangeTimingUpdate"));

			serializedObject.ApplyModifiedProperties();
		}
	}
}
