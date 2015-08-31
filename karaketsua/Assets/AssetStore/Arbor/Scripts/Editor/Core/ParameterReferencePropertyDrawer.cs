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
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty containerProperty = property.FindPropertyRelative("container");
			SerializedProperty idProperty = property.FindPropertyRelative("id");

			Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth*0.5f, 16f);
			EditorGUI.LabelField(labelRect, label);

			Rect containerRect = new Rect(labelRect.xMax, position.y, EditorGUIUtility.labelWidth * 0.5f, 16f);
			Rect parameterRect = new Rect(containerRect.xMax, position.y, Mathf.Max(0.0f, position.xMax- containerRect.xMax), 16f);

			EditorGUI.PropertyField(containerRect, containerProperty, GUIContent.none);

			ParameterContainer container = containerProperty.objectReferenceValue as Arbor.ParameterContainer;

			if (container != null)
			{
				Parameter[] parameters = container.parameters;

				if (parameters.Length > 0)
				{
					int id = idProperty.intValue;

					int selected = -1;

					List<string> names = new List<string>();
					List<int> ids = new List<int>();
					for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++)
					{
						names.Add(parameters[paramIndex].name);
						ids.Add(parameters[paramIndex].id);

						if (parameters[paramIndex].id == id)
						{
							selected = paramIndex;
						}
					}

					selected = EditorGUI.Popup(parameterRect, selected, names.ToArray());

					if (selected >= 0 && ids[selected] != idProperty.intValue)
					{
						idProperty.intValue = ids[selected];
					}
				}
			}
			else
			{
				GUI.enabled = false;

				EditorGUI.Popup(parameterRect, -1, new string[] { "" });

				GUI.enabled = true;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 16f;
		}
	}
}
