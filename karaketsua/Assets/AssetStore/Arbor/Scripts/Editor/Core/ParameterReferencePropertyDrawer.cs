using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(ParameterReference))]
	public class ParameterReferencePropertyDrawer : PropertyDrawer
	{
		protected virtual bool CheckType(Parameter.Type type)
		{
			return true;
		}

		int GetSelectParameter(int id,Parameter[] parameters, List<string> names, List<int> ids)
		{
			int selected = -1;

			if (parameters.Length > 0)
			{
				for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++)
				{
					Parameter parameter = parameters[paramIndex];

					if (!CheckType(parameter.type))
					{
						continue;
					}

					if (parameter.id == id)
					{
						selected = names.Count;
					}

					names.Add(parameter.name);
					ids.Add(parameter.id);
				}
			}

			return selected;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			SerializedProperty containerProperty = property.FindPropertyRelative("container");
			SerializedProperty idProperty = property.FindPropertyRelative("id");

			Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			EditorGUI.LabelField(labelRect, label );

			EditorGUI.indentLevel++;

			Rect containerRect = new Rect(position.x, labelRect.yMax, position.width, EditorGUIUtility.singleLineHeight);
			Rect parameterRect = new Rect(position.x, containerRect.yMax, position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField(containerRect, containerProperty);

			ParameterContainerBase containerBase = containerProperty.objectReferenceValue as ParameterContainerBase;
			ParameterContainerInternal container = null;
			if (containerBase != null)
			{
				container = containerBase.defaultContainer;
			}

			Parameter[] parameters = null;

            List<string> names = new List<string>();
			List<int> ids = new List<int>();

			int selected = -1;

			if (container != null)
			{
				parameters = container.parameters;

				selected = GetSelectParameter(idProperty.intValue, parameters, names, ids);
			}

			if (names.Count > 0)
			{
				selected = EditorGUI.Popup(parameterRect, "Parameter",selected, names.ToArray());

				if (selected >= 0 && ids[selected] != idProperty.intValue)
				{
					idProperty.intValue = ids[selected];
				}
			}
			else
			{
				EditorGUI.BeginDisabledGroup(true);
				
				EditorGUI.Popup(parameterRect, "Parameter", -1, new string[] { "" });

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
