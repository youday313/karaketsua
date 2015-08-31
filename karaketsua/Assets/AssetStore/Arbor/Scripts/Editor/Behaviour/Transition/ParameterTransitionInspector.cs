using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ArborEditor
{
	[CustomEditor(typeof(Arbor.ParameterTransition))]
	public class ParameterTransitionInspector : Editor
	{
		public class Styles
		{
			public static GUIContent addIconContent;
			public static GUIContent removeIconContent;
			public static GUIStyle invisibleButton;
			public static GUIStyle OLTitle;
			public static GUIStyle OLBox;
			public static GUIStyle separator;

			static Styles()
			{
				addIconContent = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"));
				removeIconContent = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"));
				invisibleButton = (GUIStyle)"InvisibleButton";
				OLTitle = (GUIStyle)"OL Title";
				OLBox = (GUIStyle)"OL box NoExpand";
				separator = (GUIStyle)"sv_iconselector_sep";
			}
		}

		private void CondisionGUI(Arbor.ParameterContainer container, SerializedProperty referenceProperty, SerializedProperty condisionProperty)
		{
			EditorGUILayout.BeginHorizontal();

			SerializedProperty idProperty = referenceProperty.FindPropertyRelative("id");

			Arbor.Parameter parameter = container.GetParam(idProperty.intValue);
			
			if (parameter != null)
			{
				SerializedProperty typeProperty = condisionProperty.FindPropertyRelative("type");

				switch (parameter.type)
				{
					case Arbor.Parameter.Type.Int:
						{
							Rect position = GUILayoutUtility.GetRect(0.0f, 18f);
							Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
							EditorGUI.LabelField(labelRect, "Value");

							Rect functionRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
							Rect valueRect = new Rect(functionRect.xMax, position.y, Mathf.Max(0.0f, position.xMax - functionRect.xMax), 16f);

							EditorGUI.PropertyField(functionRect, typeProperty, GUIContent.none);

							EditorGUI.PropertyField(valueRect, condisionProperty.FindPropertyRelative("intValue"), GUIContent.none);
						}
						break;
					case Arbor.Parameter.Type.Float:
						{
							Rect position = GUILayoutUtility.GetRect(0.0f, 18f);
							Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
							EditorGUI.LabelField(labelRect, "Value");

							Rect functionRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
							Rect valueRect = new Rect(functionRect.xMax, position.y, Mathf.Max(0.0f, position.xMax - functionRect.xMax), 16f);

							EditorGUI.PropertyField(functionRect, typeProperty, GUIContent.none);

							EditorGUI.PropertyField(valueRect, condisionProperty.FindPropertyRelative("floatValue"), GUIContent.none);
						}
						break;
					case Arbor.Parameter.Type.Bool:
						{
							Rect position = GUILayoutUtility.GetRect(0.0f, 18f);

							EditorGUI.PropertyField(position, condisionProperty.FindPropertyRelative("boolValue"), new GUIContent("Value"));
						}
						break;
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		void OnAddButton()
		{
			serializedObject.FindProperty("_Condisions").arraySize++;
        }

		void OnDrawChild(SerializedProperty property)
		{
			SerializedProperty referenceProperty = property.FindPropertyRelative("reference");

			EditorGUILayout.PropertyField(referenceProperty);

			SerializedProperty containerProperty = referenceProperty.FindPropertyRelative("container");

			Arbor.ParameterContainer container = containerProperty.objectReferenceValue as Arbor.ParameterContainer;

			if (container != null)
			{
				CondisionGUI(container, referenceProperty, property);
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			ListGUI listGUI = new ListGUI(serializedObject.FindProperty("_Condisions"));

			listGUI.addButton = OnAddButton;
			listGUI.drawChild = OnDrawChild;

			listGUI.OnGUI();
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}
