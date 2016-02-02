using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(FlexibleFloat))]
	public class FlexibleFloatPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			SerializedProperty typeProperty = property.FindPropertyRelative("_Type");

			FlexibleFloat.Type type = (FlexibleFloat.Type)typeProperty.enumValueIndex;

			switch (type)
			{
				case FlexibleFloat.Type.Constant:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Value"), label);
					break;
				case FlexibleFloat.Type.Parameter:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Parameter"), label);
					break;
				case FlexibleFloat.Type.Random:
					Rect contentsLabel = EditorGUITools.PrefixLabel(EditorGUITools.SubtractPopupWidth(position), label);
					contentsLabel.width *= 0.5f;
                    EditorGUI.PropertyField(contentsLabel, property.FindPropertyRelative("_MinRange"), GUIContent.none);
					contentsLabel.x += contentsLabel.width;
					EditorGUI.PropertyField(contentsLabel, property.FindPropertyRelative("_MaxRange"), GUIContent.none);
					break;
			}

			Rect popupRect = EditorGUITools.GetPopupRect(position);

			if (EditorGUITools.ButtonMouseDown(popupRect, GUIContent.none,FocusType.Passive, Styles.shurikenDropDown))
			{
				GenericMenu menu = new GenericMenu();
				foreach (FlexibleFloat.Type t in System.Enum.GetValues(typeof(FlexibleFloat.Type)))
				{
					menu.AddItem(new GUIContent(t.ToString()), t == type, SelectType,new KeyValuePair<SerializedProperty, FlexibleFloat.Type>(typeProperty, t) );
				}
				menu.DropDown(popupRect);
            }

			EditorGUI.EndProperty();
		}

		private static void SelectType(object obj)
		{
			KeyValuePair<SerializedProperty, FlexibleFloat.Type> pair = (KeyValuePair<SerializedProperty, FlexibleFloat.Type>)obj;
			SerializedProperty typeProperty = pair.Key;
			FlexibleFloat.Type type = pair.Value;

			typeProperty.serializedObject.Update();

            typeProperty.enumValueIndex = (int)type;

			typeProperty.serializedObject.ApplyModifiedProperties();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight;

			SerializedProperty typeProperty = property.FindPropertyRelative("_Type");

			FlexibleFloat.Type type = (FlexibleFloat.Type)typeProperty.enumValueIndex;

			switch (type)
			{
				case FlexibleFloat.Type.Constant:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Value"));
					break;
				case FlexibleFloat.Type.Parameter:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Parameter"));
					break;
				case FlexibleFloat.Type.Random:
					height = EditorGUIUtility.singleLineHeight;
					break;
			}

			return height;
		}
	}
}
