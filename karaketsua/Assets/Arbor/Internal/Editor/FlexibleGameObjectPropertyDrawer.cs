using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(FlexibleGameObject))]
	public class FlexibleGameObjectPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			SerializedProperty typeProperty = property.FindPropertyRelative("_Type");

			FlexibleGameObject.Type type = (FlexibleGameObject.Type)typeProperty.enumValueIndex;

			switch (type)
			{
				case FlexibleGameObject.Type.Constant:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Value"), label);
					break;
				case FlexibleGameObject.Type.Parameter:
					EditorGUI.PropertyField(EditorGUITools.SubtractPopupWidth(position), property.FindPropertyRelative("_Parameter"), label);
					break;
			}

			Rect popupRect = EditorGUITools.GetPopupRect(position);

			if (EditorGUITools.ButtonMouseDown(popupRect, GUIContent.none, FocusType.Passive, Styles.shurikenDropDown))
			{
				GenericMenu menu = new GenericMenu();
				foreach (FlexibleGameObject.Type t in System.Enum.GetValues(typeof(FlexibleGameObject.Type)))
				{
					menu.AddItem(new GUIContent(t.ToString()), t == type, SelectType, new KeyValuePair<SerializedProperty, FlexibleGameObject.Type>(typeProperty, t));
				}
				menu.DropDown(popupRect);
			}

			EditorGUI.EndProperty();
		}

		private static void SelectType(object obj)
		{
			KeyValuePair<SerializedProperty, FlexibleGameObject.Type> pair = (KeyValuePair<SerializedProperty, FlexibleGameObject.Type>)obj;
			SerializedProperty typeProperty = pair.Key;
			FlexibleGameObject.Type type = pair.Value;

			typeProperty.serializedObject.Update();

			typeProperty.enumValueIndex = (int)type;

			typeProperty.serializedObject.ApplyModifiedProperties();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight;

            SerializedProperty typeProperty = property.FindPropertyRelative("_Type");

			FlexibleGameObject.Type type = (FlexibleGameObject.Type)typeProperty.enumValueIndex;

			switch (type)
			{
				case FlexibleGameObject.Type.Constant:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Value"));
					break;
				case FlexibleGameObject.Type.Parameter:
					height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_Parameter"));
					break;
			}

			return height;
		}
	}
}
