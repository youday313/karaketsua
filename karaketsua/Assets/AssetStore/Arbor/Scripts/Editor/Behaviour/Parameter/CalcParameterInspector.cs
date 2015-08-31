using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.CalcParameter))]
	public class CalcParameterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty referenceProperty = serializedObject.FindProperty("reference");

			EditorGUILayout.PropertyField(referenceProperty);

			SerializedProperty containerProperty = referenceProperty.FindPropertyRelative("container");

			Arbor.ParameterContainer container = containerProperty.objectReferenceValue as Arbor.ParameterContainer;

			if (container != null)
			{
				EditorGUILayout.BeginHorizontal();

				SerializedProperty idProperty = referenceProperty.FindPropertyRelative("id");

				Arbor.Parameter parameter = container.GetParam(idProperty.intValue);

				if (parameter != null)
				{
					SerializedProperty functionProperty = serializedObject.FindProperty("function");

					switch (parameter.type)
					{
						case Arbor.Parameter.Type.Int:
							{
								Rect position = GUILayoutUtility.GetRect(0.0f, 18f);
								Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
								EditorGUI.LabelField(labelRect, "Value");

								Rect functionRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
								Rect valueRect = new Rect(functionRect.xMax, position.y, Mathf.Max(0.0f, position.xMax - functionRect.xMax), 16f);

								EditorGUI.PropertyField(functionRect, functionProperty, GUIContent.none);

								EditorGUI.PropertyField(valueRect,serializedObject.FindProperty("intValue"), GUIContent.none);
							}
							break;
						case Arbor.Parameter.Type.Float:
							{
								Rect position = GUILayoutUtility.GetRect(0.0f, 18f);
								Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
								EditorGUI.LabelField(labelRect, "Value");

								Rect functionRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
								Rect valueRect = new Rect(functionRect.xMax, position.y, Mathf.Max(0.0f, position.xMax - functionRect.xMax), 16f);

								EditorGUI.PropertyField(functionRect, functionProperty, GUIContent.none);

								EditorGUI.PropertyField(valueRect, serializedObject.FindProperty("floatValue"), GUIContent.none);
							}
							break;
						case Arbor.Parameter.Type.Bool:
							{
								Rect position = GUILayoutUtility.GetRect(0.0f, 18f);

								EditorGUI.PropertyField(position,serializedObject.FindProperty("boolValue"), new GUIContent("Value"));
							}
							break;
					}
				}

				EditorGUILayout.EndHorizontal();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}

