using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	internal class GridSettingsWindow : EditorWindow
	{
		private static GridSettingsWindow _Instance = null;

		public static GridSettingsWindow instance
		{
			get
			{
				if( _Instance == null )
				{
					_Instance = ScriptableObject.CreateInstance<GridSettingsWindow>();
				}

				return _Instance;
			}
		}

		internal static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector2 = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector2.x;
			guiRect.y = vector2.y;
			return guiRect;
		}

		public void Init( Rect buttonRect )
		{
			buttonRect = GUIToScreenRect( buttonRect );

			Vector2 windowSize = new Vector2(300f, Mathf.Min(2f + 110.0f, 900f));
			ShowAsDropDown( buttonRect, windowSize );
		}

		void OnGUI()
		{
			EditorGUI.BeginChangeCheck();

			ArborSettings.showGrid = EditorGUILayout.ToggleLeft( Localization.GetWord("Show Grid"),ArborSettings.showGrid );

			GUI.enabled = ArborSettings.showGrid;

			ArborSettings.snapGrid = EditorGUILayout.ToggleLeft( Localization.GetWord("Snap Grid"),ArborSettings.snapGrid );

			EditorGUILayout.Space();

			ArborSettings.gridSize = EditorGUILayout.Slider( Localization.GetWord("Grid Size"),ArborSettings.gridSize,1.0f,1000.0f );
			ArborSettings.gridSplitNum = EditorGUILayout.IntSlider( Localization.GetWord("Grid Split Num"),ArborSettings.gridSplitNum,1,100 );

			GUI.enabled = true;

			if( GUILayout.Button( Localization.GetWord("Reset") ) )
			{
				ArborSettings.ResetGrid();
			}

			if( EditorGUI.EndChangeCheck() )
			{
				ArborEditorWindow.GetCurrent().Repaint();
			}
		}
	}
}