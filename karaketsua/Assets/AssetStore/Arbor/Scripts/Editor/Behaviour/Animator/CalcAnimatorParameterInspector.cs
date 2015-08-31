using UnityEngine;
using UnityEditor;
#if UNITY_5
using UnityEditor.Animations;
#else
using UnityEditorInternal;
#endif
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(CalcAnimatorParameter))]
	public class CalcAnimatorParameterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SerializedProperty referenceProperty = serializedObject.FindProperty("reference");

			EditorGUILayout.PropertyField(referenceProperty);

			Animator animator = referenceProperty.FindPropertyRelative("animator").objectReferenceValue as Animator;

			if (animator != null)
			{
				AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

				EditorGUILayout.BeginHorizontal();

				SerializedProperty nameProperty = referenceProperty.FindPropertyRelative("name");

				AnimatorControllerParameter selectParameter = null;

#if UNITY_5
				foreach (AnimatorControllerParameter parameter in animatorController.parameters)
				{
					if (parameter.name == nameProperty.stringValue)
					{
						selectParameter = parameter;
						break;
					}
				}
#else
				for (int paramIndex = 0; paramIndex < animatorController.parameterCount; paramIndex++)
				{
					AnimatorControllerParameter parameter = animatorController.GetParameter(paramIndex);

					if (parameter.name == nameProperty.stringValue)
					{
						selectParameter = parameter;
					}
				}
#endif

				if (selectParameter != null)
				{
					SerializedProperty functionProperty = serializedObject.FindProperty("function");

					switch (selectParameter.type)
					{
						case AnimatorControllerParameterType.Float:
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
						case AnimatorControllerParameterType.Int:
							{
								Rect position = GUILayoutUtility.GetRect(0.0f, 18f);
								Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
								EditorGUI.LabelField(labelRect, "Value");

								Rect functionRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
								Rect valueRect = new Rect(functionRect.xMax, position.y, Mathf.Max(0.0f, position.xMax - functionRect.xMax), 16f);

								EditorGUI.PropertyField(functionRect, functionProperty, GUIContent.none);

								EditorGUI.PropertyField(valueRect, serializedObject.FindProperty("intValue"), GUIContent.none);
							}
							break;
						case AnimatorControllerParameterType.Bool:
							{
								Rect position = GUILayoutUtility.GetRect(0.0f, 18f);

								EditorGUI.PropertyField(position, serializedObject.FindProperty("boolValue"), new GUIContent("Value"));
							}
							break;
						case AnimatorControllerParameterType.Trigger:
							break;
					}
				}

				EditorGUILayout.EndHorizontal();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}