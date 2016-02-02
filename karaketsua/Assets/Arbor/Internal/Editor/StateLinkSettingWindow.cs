using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class StateLinkSettingWindow : EditorWindow
	{
		private class Styles
		{
			public static GUIStyle background;
			
			static Styles()
			{
				background = (GUIStyle)"grey_border";
			}
		}

		private static StateLinkSettingWindow _Instance;

		public static StateLinkSettingWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<StateLinkSettingWindow>();
				}
				return _Instance;
			}
		}

		private SerializedObject _SerializedObject;
		private string _StateLinkPath;

		internal static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector2 = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector2.x;
			guiRect.y = vector2.y;
			return guiRect;
		}

		public void Init(SerializedProperty stateLinkProperty, Rect buttonRect)
		{
			buttonRect = GUIToScreenRect(buttonRect);
			ShowAsDropDown(buttonRect, new Vector2(300f, 60f));
			Focus();

			_SerializedObject = stateLinkProperty.serializedObject;
			_StateLinkPath = stateLinkProperty.propertyPath;
		}

		void DrawBackground()
		{
			if (Event.current.type == EventType.Repaint)
			{
				Styles.background.Draw(position, false, false, false, false);
			}
		}

		void OnGUI()
		{
			_SerializedObject.Update();

			DrawBackground();

			SerializedProperty stateLinkProperty = _SerializedObject.FindProperty(_StateLinkPath);

			System.Type type = null;
			System.Reflection.FieldInfo stateLinkFieldInfo = EditorGUITools.GetFieldInfoFromProperty(stateLinkProperty, out type);

			FixedImmediateTransition fixedImmediateTransition = null;

			object[] attributes = stateLinkFieldInfo.GetCustomAttributes(typeof(FixedImmediateTransition), false);
			if (attributes != null && attributes.Length > 0)
			{
				fixedImmediateTransition = (FixedImmediateTransition)attributes[0];
			}

			SerializedProperty nameProperty = stateLinkProperty.FindPropertyRelative("name");
			SerializedProperty immediateTransitionProperty = stateLinkProperty.FindPropertyRelative("immediateTransition");
			SerializedProperty lineColorProperty = stateLinkProperty.FindPropertyRelative("lineColor");
			SerializedProperty lineColorChangedProperty = stateLinkProperty.FindPropertyRelative("lineColorChanged");

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(nameProperty);
			if (EditorGUI.EndChangeCheck())
			{
				ArborEditorWindow.GetCurrent().Repaint();
			}

			bool tempImmediateTransition = immediateTransitionProperty.boolValue;
			if (fixedImmediateTransition != null)
			{
				immediateTransitionProperty.boolValue = fixedImmediateTransition.immediate;
				GUI.enabled = false;
			}

			EditorGUILayout.PropertyField(immediateTransitionProperty);

			if (fixedImmediateTransition != null)
			{
				immediateTransitionProperty.boolValue = tempImmediateTransition;
				GUI.enabled = true;
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(lineColorProperty);
			if (EditorGUI.EndChangeCheck())
			{
				lineColorChangedProperty.boolValue = true;
				ArborEditorWindow.GetCurrent().Repaint();
			}

			_SerializedObject.ApplyModifiedProperties();
		}
	}
}
