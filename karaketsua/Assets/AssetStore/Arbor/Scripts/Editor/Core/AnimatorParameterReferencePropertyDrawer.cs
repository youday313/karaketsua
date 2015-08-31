using UnityEngine;
using UnityEditor;
#if UNITY_5
using UnityEditor.Animations;
#else
using UnityEditorInternal;
#endif
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(AnimatorParameterReference))]
	public class AnimatorParameterReferencePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty animatorProperty = property.FindPropertyRelative("animator");
			SerializedProperty nameProperty = property.FindPropertyRelative("name");
			SerializedProperty typeProperty = property.FindPropertyRelative("type");

			Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
			EditorGUI.LabelField(labelRect, label);

			Rect containerRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
			Rect parameterRect = new Rect(containerRect.xMax, position.y, Mathf.Max(0.0f, position.xMax - containerRect.xMax), 16f);

			EditorGUI.PropertyField(containerRect, animatorProperty, GUIContent.none);

			Animator animator = animatorProperty.objectReferenceValue as Animator;

			if (animator != null && animator.runtimeAnimatorController != null )
			{
				AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

#if UNITY_5
				int parameterCount = (animatorController.parameters != null) ? animatorController.parameters.Length : 0;
#else
				int parameterCount = animatorController.parameterCount;
#endif
				if (parameterCount > 0)
				{
					string name = nameProperty.stringValue;
					int selected = -1;

					List<string> names = new List<string>();
					List<int> types = new List<int>();
					for (int paramIndex = 0; paramIndex < parameterCount; paramIndex++)
					{
#if UNITY_5
						AnimatorControllerParameter parameter = animatorController.parameters[paramIndex];
#else
						AnimatorControllerParameter parameter = animatorController.GetParameter(paramIndex);
#endif
						names.Add( parameter.name );
						types.Add( (int)parameter.type );

						if (parameter.name == name)
						{
							selected = paramIndex;
						}
					}

					selected = EditorGUI.Popup(parameterRect, selected, names.ToArray());

					if (selected >= 0)
					{
						nameProperty.stringValue = names[selected];
						typeProperty.intValue = types[selected];
                    }
				}
			}
			else
			{
				GUI.enabled = false;

				EditorGUI.Popup(parameterRect, -1, new string[] { "" });

				GUI.enabled = true;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 16f;
		}
	}
}