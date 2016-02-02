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
			EditorGUI.BeginProperty(position, label, property);
			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			SerializedProperty animatorProperty = property.FindPropertyRelative("animator");
			SerializedProperty nameProperty = property.FindPropertyRelative("name");
			SerializedProperty typeProperty = property.FindPropertyRelative("type");

			Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			EditorGUI.LabelField(labelRect, label);

			EditorGUI.indentLevel++;

			Rect containerRect = new Rect(position.x, labelRect.yMax, position.width, EditorGUIUtility.singleLineHeight);
			Rect parameterRect = new Rect(position.x, containerRect.yMax, position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField(containerRect, animatorProperty);

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

					selected = EditorGUI.Popup(parameterRect, "Parameter",selected, names.ToArray());

					if (selected >= 0)
					{
						nameProperty.stringValue = names[selected];
						typeProperty.intValue = types[selected];
                    }
				}
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);
				
				EditorGUI.Popup(parameterRect, "Parameter",-1, new string[] { "" });

				EditorGUI.EndDisabledGroup();
			}

			EditorGUI.indentLevel = indentLevel;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
            return EditorGUIUtility.singleLineHeight * 3;
		}
	}
}
