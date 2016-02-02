using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	public sealed class ListGUI
	{
		private string _Title;
		private SerializedProperty _Property;

		public ListGUI(SerializedProperty property)
		{
			_Property = property;
		}

		public delegate void DelegateAddButton();

		public DelegateAddButton addButton;

		public delegate void DelegateDrawChild(SerializedProperty property);

		public DelegateDrawChild drawChild;

		public void OnGUI()
		{
			GUILayout.BeginHorizontal(Styles.RLHeader);

			GUILayout.Label(ObjectNames.NicifyVariableName(_Property.name));

			GUILayout.FlexibleSpace();
			if (GUILayout.Button(Styles.addIconContent, Styles.invisibleButton, GUILayout.Width(20f), GUILayout.Height(20)))
			{
				if (addButton != null)
				{
					addButton();
				}
				else
				{
					_Property.arraySize++;
				}
			}
			GUILayout.EndHorizontal();

			if (_Property.arraySize > 0)
			{
				EditorGUILayout.BeginVertical(Styles.RLBackground);

				for (int i = 0; i < _Property.arraySize; i++)
				{
					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.BeginVertical();

					if (drawChild != null)
					{
						drawChild(_Property.GetArrayElementAtIndex(i));
					}

					EditorGUILayout.EndVertical();

					if (GUILayout.Button(Styles.removeIconContent, Styles.invisibleButton, GUILayout.Width(20f), GUILayout.Height(20)))
					{
						_Property.DeleteArrayElementAtIndex(i);
						break;
					}

					EditorGUILayout.EndHorizontal();

					if (i < _Property.arraySize - 1)
					{
						EditorGUITools.DrawSeparator();
					}
				}

				GUILayout.Space(10);

				EditorGUILayout.EndVertical();
			}
		}
	}
}
