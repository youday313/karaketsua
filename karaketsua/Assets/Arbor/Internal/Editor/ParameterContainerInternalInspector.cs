using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	public class ParameterContainerInternalInspector : Editor
	{
		ParameterContainerInternal _ParameterContainer;

		void OnEnable()
		{
			_ParameterContainer = target as ParameterContainerInternal;
		}
		
		void AddParameterMenu(object value)
		{
			Undo.RecordObject(_ParameterContainer, "Parameter Added");
			Parameter.Type type = (Parameter.Type)value;
			_ParameterContainer.AddParam("New " + type.ToString(), type);
		}

		void OnAddButton()
		{
			GenericMenu genericMenu = new GenericMenu();
			foreach (object userData in System.Enum.GetValues(typeof(Parameter.Type)))
			{
				genericMenu.AddItem(new GUIContent(userData.ToString()), false, AddParameterMenu, userData);
			}
			genericMenu.ShowAsContext();
		}

		void OnDrawChild(SerializedProperty property)
		{
			EditorGUILayout.BeginHorizontal();

			SerializedProperty nameProperty = property.FindPropertyRelative("name");
			
			EditorGUI.BeginChangeCheck();
			string name = EditorGUILayout.TextField(nameProperty.stringValue, GUILayout.ExpandWidth(true));
			if (EditorGUI.EndChangeCheck() && name != nameProperty.stringValue)
			{
				nameProperty.stringValue = _ParameterContainer.MakeUniqueName(name);
			}

			Parameter.Type type = (Parameter.Type)property.FindPropertyRelative("type").enumValueIndex;
			switch (type)
			{
				case Parameter.Type.Int:
					{
						SerializedProperty valueProperty = property.FindPropertyRelative("intValue");
                        EditorGUI.BeginChangeCheck();
						int intValue = EditorGUILayout.IntField(valueProperty.intValue, GUILayout.Width(120f));
						if (EditorGUI.EndChangeCheck())
						{
							valueProperty.intValue = intValue;
						}
					}
					break;
				case Parameter.Type.Float:
					{
						SerializedProperty valueProperty = property.FindPropertyRelative("floatValue");
						EditorGUI.BeginChangeCheck();
						float floatValue = EditorGUILayout.FloatField(valueProperty.floatValue, GUILayout.Width(120f));
						if (EditorGUI.EndChangeCheck())
						{
							valueProperty.floatValue = floatValue;
						}
					}
					break;
				case Parameter.Type.Bool:
					{
						SerializedProperty valueProperty = property.FindPropertyRelative("boolValue");
						EditorGUI.BeginChangeCheck();
						bool boolValue = GUILayout.Toggle(valueProperty.boolValue, string.Empty, GUILayout.Width(120f));
						if (EditorGUI.EndChangeCheck())
						{
							valueProperty.boolValue = boolValue;
						}
					}
					break;
				case Parameter.Type.GameObject:
					{
						SerializedProperty valueProperty = property.FindPropertyRelative("gameObjectValue");
						EditorGUI.BeginChangeCheck();
						GameObject gameObjectValue = EditorGUILayout.ObjectField(valueProperty.objectReferenceValue,typeof(GameObject),true,GUILayout.Width(120f)) as GameObject;
						if (EditorGUI.EndChangeCheck())
						{
							valueProperty.objectReferenceValue = gameObjectValue;
						}
					}
					break;
			}

			EditorGUILayout.EndHorizontal();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			ListGUI listGUI = new ListGUI(serializedObject.FindProperty("_Parameters") );

			listGUI.addButton = OnAddButton;
			listGUI.drawChild = OnDrawChild;

			listGUI.OnGUI();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
