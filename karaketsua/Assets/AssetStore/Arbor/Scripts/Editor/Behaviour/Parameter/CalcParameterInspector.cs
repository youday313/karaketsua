using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(CalcParameter))]
	public class CalcParameterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty referenceProperty = serializedObject.FindProperty("reference");

			EditorGUILayout.PropertyField(referenceProperty);

			SerializedProperty containerProperty = referenceProperty.FindPropertyRelative("container");

			ParameterContainerBase containerBase = containerProperty.objectReferenceValue as ParameterContainerBase;
			ParameterContainer container = null;
			if (containerBase != null)
			{
				container = containerBase.defaultContainer as ParameterContainer;
			}

			if (container != null)
			{
				SerializedProperty idProperty = referenceProperty.FindPropertyRelative("id");

				Parameter parameter = container.GetParam(idProperty.intValue);

				if (parameter != null)
				{
					SerializedProperty functionProperty = serializedObject.FindProperty("function");

					switch (parameter.type)
					{
						case Parameter.Type.Int:
							{
								EditorGUILayout.PropertyField(functionProperty);

								EditorGUILayout.PropertyField(serializedObject.FindProperty("_IntValue"));
							}
							break;
						case Parameter.Type.Float:
							{
								EditorGUILayout.PropertyField(functionProperty);

								EditorGUILayout.PropertyField(serializedObject.FindProperty("_FloatValue"));
							}
							break;
						case Parameter.Type.Bool:
							{
								EditorGUILayout.PropertyField(serializedObject.FindProperty("_BoolValue"));
							}
							break;
						case Parameter.Type.GameObject:
							{
								EditorGUILayout.PropertyField(serializedObject.FindProperty("_GameObjectValue"));
							}
							break;
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}

