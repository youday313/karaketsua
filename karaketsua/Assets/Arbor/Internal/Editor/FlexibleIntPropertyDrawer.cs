using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(FlexibleInt))]
	public class FlexibleIntPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			SerializedProperty typeProperty = property.FindPropertyRelative("_Type");

			FlexibleInt.Type type = (FlexibleInt.Type)typeProperty.enumValueIndex;

			switch (type)
			{
				case FlexibleInt.Type.Constant:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Value"), label);
					break;
				case FlexibleInt.Type.Parameter:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Parameter"), label);
					break;
				case FlexibleInt.Type.Random:
					Rect contentsLabel = EditorGUITools.PrefixLabel(EditorGUITools.SubtractPopupWidth(position), label);
					contentsLabel.width *= 0.5f;
					EditorGUI.PropertyField(contentsLabel, property.FindPropertyRelative("_MinRange"), GUIContent.none);
					contentsLabel.x += contentsLabel.width;
					EditorGUI.PropertyField(contentsLabel, property.FindPropertyRelative("_MaxRange"), GUIContent.none);
					break;
			}

			Rect popupRect = EditorGUITools.GetPopupRect(position);

			if (EditorGUITools.ButtonMouseDown(popupRect, GUIContent.none, FocusType.Passive, Styles.shurikenDropDown ))
			{
				GenericMenu menu = new GenericMenu();
				foreach (FlexibleInt.Type t in System.Enum.GetValues(typeof(FlexibleInt.Type)))
				{
					menu.AddItem(new GUIContent(t.ToString()), t == type, SelectType, new KeyValuePair<SerializedProperty, FlexibleInt.Type>(typeProperty, t));
				}
				menu.DropDown(popupRect);
			}

			EditorGUI.EndProperty();
		}

		private static void SelectType(object obj)
		{
			KeyValuePair<SerializedProperty, FlexibleInt.Type> pair = (KeyValuePair<SerializedProperty, FlexibleInt.Type>)obj;
			SerializedProperty typeProperty = pair.Key;
			FlexibleInt.Type type = pair.Value;

			typeProperty.serializedObject.Update();

			typeProperty.enumValueIndex = (int)type;

			typeProperty.serializedObject.ApplyModifiedProperties();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight;

			SerializedProperty typeProperty = property.FindPropertyRelative("_Type");

			FlexibleInt.Type type = (FlexibleInt.Type)typeProperty.enumValueIndex;

			switch (type)
			{
				case FlexibleInt.Type.Constant:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Value"));
					break;
				case FlexibleInt.Type.Parameter:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Parameter"));
					break;
				case FlexibleInt.Type.Random:
					height = EditorGUIUtility.singleLineHeight;
                    break;
			}

			return height;
		}
	}
}
