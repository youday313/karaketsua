using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	internal sealed class Styles
	{
		public static GUIContent addIconContent;
		public static GUIContent removeIconContent;
		public static Texture2D connectionTexture;
		public static Texture2D selectedConnectionTexture;
		public static GUIStyle invisibleButton;
		public static GUIStyle RLHeader;
		public static GUIStyle RLBackground;
		public static GUIStyle separator;
		public static GUIStyle background;
		public static GUIStyle header;
		public static GUIStyle componentButton;
		public static GUIStyle groupButton;
		public static GUIStyle previewBackground;
		public static GUIStyle previewHeader;
		public static GUIStyle previewText;
		public static GUIStyle rightArrow;
		public static GUIStyle leftArrow;
		public static GUIStyle shurikenDropDown;
		public static GUIStyle titlebar;
		public static GUIStyle titlebarText;
		public static GUIStyle hostview;
		public static GUIStyle graphBackground;
		public static GUIStyle selectionRect;
		private static readonly Dictionary<string, GUIStyle> _NodeStyleCache;

		static Styles()
		{
			addIconContent = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Plus"));
			removeIconContent = new GUIContent(EditorGUIUtility.FindTexture("Toolbar Minus"));
			connectionTexture = FindTexture("flow connection texture.png");
			selectedConnectionTexture = FindTexture("flow selected connection texture.png");
			invisibleButton = (GUIStyle)"InvisibleButton";
			RLHeader = (GUIStyle)"RL Header";
			RLBackground = new GUIStyle((GUIStyle)"RL Background");
			separator = (GUIStyle)"sv_iconselector_sep";
			background = (GUIStyle)"grey_border";
			header = new GUIStyle((GUIStyle)"IN BigTitle");
			componentButton = new GUIStyle((GUIStyle)"PR Label");
			previewBackground = (GUIStyle)"PopupCurveSwatchBackground";
			previewHeader = new GUIStyle(EditorStyles.label);
			previewText = new GUIStyle(EditorStyles.wordWrappedLabel);
			rightArrow = (GUIStyle)"AC RightArrow";
			leftArrow = (GUIStyle)"AC LeftArrow";
			shurikenDropDown = (GUIStyle)"ShurikenDropDown";
            titlebar = (GUIStyle)"IN Title";
			titlebarText = (GUIStyle)"IN TitleText";
			hostview = new GUIStyle((GUIStyle)"hostview");
			graphBackground = (GUIStyle)"flow background";
			selectionRect = (GUIStyle)"SelectionRect";
            header.font = EditorStyles.boldLabel.font;
			RLBackground.stretchHeight = false;
            componentButton.alignment = TextAnchor.MiddleLeft;
			componentButton.padding.left -= 15;
			componentButton.fixedHeight = 20f;
			groupButton = new GUIStyle(componentButton);
			groupButton.padding.left += 17;
			previewText.padding.left += 3;
			previewText.padding.right += 3;
			++previewHeader.padding.left;
			previewHeader.padding.right += 3;
			previewHeader.padding.top += 3;
			previewHeader.padding.bottom += 2;
			hostview.stretchHeight = false;
			_NodeStyleCache = new Dictionary<string, GUIStyle>();
        }

		private static Texture2D FindTexture(string contentName)
		{
			Texture2D image = null;
			if (EditorGUIUtility.isProSkin)
			{
#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
				image = EditorGUIUtility.Load("Builtin Skins/DarkSkin/images/" + contentName) as Texture2D;
#else
				image = EditorGUIUtility.Load("Graph/Dark/" + contentName) as Texture2D;
#endif
			}
			if (image == null)
			{
#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
				image = EditorGUIUtility.Load("Builtin Skins/LightSkin/images/" + contentName) as Texture2D;
#else
				image = EditorGUIUtility.Load("Graph/Light/" + contentName) as Texture2D;
#endif
			}
			if (image != null)
			{
				return image;
			}
			Debug.LogError("Unable to load " + contentName);
			return null;
		}

		public static GUIStyle GetNodeStyle(string styleName, Styles.Color color, bool on)
		{
			string key = string.Format("flow {0} {1}{2}", styleName, (int)color, (!on)? string.Empty :" on");
			GUIStyle style = null;
			if (!_NodeStyleCache.TryGetValue(key,out style))
			{
				style = (GUIStyle)key;
				_NodeStyleCache.Add(key, style);
			}
			return style;
		}

		public enum Color
		{
			Gray = 0,
			Grey = 0,
			Blue = 1,
			Aqua = 2,
			Green = 3,
			Yellow = 4,
			Orange = 5,
			Red = 6,
		}
	}
}
