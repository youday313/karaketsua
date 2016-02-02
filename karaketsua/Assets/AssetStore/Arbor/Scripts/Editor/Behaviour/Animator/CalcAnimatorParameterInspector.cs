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

			SerializedProperty referenceProperty = serializedObject.FindProperty("_Reference");

			EditorGUILayout.PropertyField(referenceProperty);

			Animator animator = referenceProperty.FindPropertyRelative("animator").objectReferenceValue as Animator;

			if (animator != null)
			{
				AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

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
					SerializedProperty functionProperty = serializedObject.FindProperty("_Function");

					switch (selectParameter.type)
					{
						case AnimatorControllerParameterType.Float:
							{
								EditorGUILayout.PropertyField(functionProperty);
								EditorGUILayout.PropertyField(serializedObject.FindProperty("_FloatValue"),new GUIContent("Float Value"));
							}
							break;
						case AnimatorControllerParameterType.Int:
							{
								EditorGUILayout.PropertyField(functionProperty);
								EditorGUILayout.PropertyField(serializedObject.FindProperty("_IntValue"), new GUIContent("Int Value"));
							}
							break;
						case AnimatorControllerParameterType.Bool:
							{
								EditorGUILayout.PropertyField(serializedObject.FindProperty("_BoolValue"), new GUIContent("Bool Value"));
							}
							break;
						case AnimatorControllerParameterType.Trigger:
							break;
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
