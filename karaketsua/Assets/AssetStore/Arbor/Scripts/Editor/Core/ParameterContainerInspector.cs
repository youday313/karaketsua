using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(ParameterContainer))]
	public class ParameterContainerInspector : Editor
	{
		public class Styles
		{
			public static GUIContent addIconContent;
			public static GUIContent removeIconContent;
			public static GUIStyle invisibleButton;
			public static GUIStyle OLTitle;
			public static GUIStyle OLBox;

			static Styles()
			{
				addIconContent = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"));
				removeIconContent = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"));
				invisibleButton = (GUIStyle)"InvisibleButton";
				OLTitle = (GUIStyle)"OL Title";
				OLBox = (GUIStyle)"OL box NoExpand";
			}
		}
		
		ParameterContainer _ParameterContainer;

		void OnEnable()
		{
			_ParameterContainer = target as ParameterContainer;
		}
		
		void AddParameterMenu(object value)
		{
			Undo.RegisterCompleteObjectUndo(_ParameterContainer, "Parameter Added");
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
				Undo.RegisterCompleteObjectUndo(_ParameterContainer, "Parameter renamed");
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
							Undo.RegisterCompleteObjectUndo(_ParameterContainer, "Parameter value changed");
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
							Undo.RegisterCompleteObjectUndo(_ParameterContainer, "Parameter value changed");
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
							Undo.RegisterCompleteObjectUndo(_ParameterContainer, "Parameter value changed");
							valueProperty.boolValue = boolValue;
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