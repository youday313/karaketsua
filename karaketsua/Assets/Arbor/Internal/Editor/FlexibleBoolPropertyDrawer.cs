using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(FlexibleBool))]
	public class FlexibleBoolPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			SerializedProperty typeProperty = property.FindPropertyRelative("_Type");

			FlexibleBool.Type type = (FlexibleBool.Type)typeProperty.enumValueIndex;

			switch (type)
			{
				case FlexibleBool.Type.Constant:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Value"), label);
					break;
				case FlexibleBool.Type.Parameter:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Parameter"), label);
					break;
				case FlexibleBool.Type.Random:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Probability"), label);
					break;
			}

			Rect popupRect = EditorGUITools.GetPopupRect(position);

			if (EditorGUITools.ButtonMouseDown(popupRect, GUIContent.none,FocusType.Passive,Styles.shurikenDropDown ))
			{
				GenericMenu menu = new GenericMenu();
				foreach (FlexibleBool.Type t in System.Enum.GetValues(typeof(FlexibleBool.Type)))
				{
					menu.AddItem(new GUIContent(t.ToString()), t == type, SelectType,new KeyValuePair<SerializedProperty, FlexibleBool.Type>(typeProperty, t) );
				}
				menu.DropDown(popupRect);
            }

			EditorGUI.EndProperty();
		}

		private static void SelectType(object obj)
		{
			KeyValuePair<SerializedProperty, FlexibleBool.Type> pair = (KeyValuePair<SerializedProperty, FlexibleBool.Type>)obj;
			SerializedProperty typeProperty = pair.Key;
			FlexibleBool.Type type = pair.Value;

			typeProperty.serializedObject.Update();

            typeProperty.enumValueIndex = (int)type;

			typeProperty.serializedObject.ApplyModifiedProperties();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight;

			SerializedProperty typeProperty = property.FindPropertyRelative("_Type");
			FlexibleBool.Type type = (FlexibleBool.Type)typeProperty.enumValueIndex;
			switch (type)
			{
				case FlexibleBool.Type.Constant:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Value"));
					break;
				case FlexibleBool.Type.Parameter:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Parameter"));
					break;
				case FlexibleBool.Type.Random:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Probability"));
					break;
			}

			return height;
		}
	}
}
