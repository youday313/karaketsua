using UnityEngine;
using UnityEditor;
using System.Collections;

public class ListGUI
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
		GUILayout.BeginHorizontal(ObjectNames.NicifyVariableName(_Property.name), Styles.OLTitle);

		GUILayout.FlexibleSpace();
		if (GUILayout.Button(Styles.addIconContent, Styles.invisibleButton, GUILayout.Width(20f), GUILayout.Height(20)))
		{
			if (addButton != null)
			{
				addButton();
            }
		}
		GUILayout.EndHorizontal();

		if (_Property.arraySize > 0)
		{
			EditorGUILayout.BeginVertical(Styles.OLBox);

			for (int i = 0; i < _Property.arraySize; i++)
			{
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.BeginVertical();

				if (drawChild != null)
				{
					drawChild( _Property.GetArrayElementAtIndex(i) );
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
					GUILayout.Label(GUIContent.none, Styles.separator);
				}
			}

			EditorGUILayout.EndVertical();
		}
    }
}
