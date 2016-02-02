using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace ArborEditor
{
	internal sealed class ArborSettings : ScriptableObject
	{
		private static ArborSettings _Instance = null;

		private static ArborSettings instance
		{
			get
			{
				if( _Instance == null )
				{
					Load();
				}

				return _Instance;
			}
		}

		ArborSettings()
		{
			_Instance = this;
		}

		private static readonly string _OldFilePath = "Library/ArborSettings.asset";
		private static readonly string _FilePath = UnityEditorInternal.InternalEditorUtility.unityPreferencesFolder + "/ArborSettings.asset";

		private static readonly bool _DefaultShowGrid = true;
		private static readonly bool _DefaultSnapGrid = true;
		private static readonly float _DefaultGridSize = 120.0f;
		private static readonly int _DefaultGridSplitNum = 10;
		private static readonly bool _DefaultOpenStateList = true;
		private static readonly float _DefaultStateListWidth = 200.0f;

		[SerializeField]private bool _ShowGrid = _DefaultShowGrid;
		[SerializeField]private bool _SnapGrid = _DefaultSnapGrid;
		[SerializeField]private float _GridSize = _DefaultGridSize;
		[SerializeField]private int _GridSplitNum = _DefaultGridSplitNum;
		
		public static bool showGrid
		{
			get
			{
				return instance._ShowGrid;
			}
			set
			{
				if( instance._ShowGrid != value )
				{
					instance._ShowGrid = value;

					Save();
				}
			}
		}

		public static bool snapGrid
		{
			get
			{
				return instance._SnapGrid;
			}
			set
			{
				if( instance._SnapGrid != value )
				{
					instance._SnapGrid = value;

					Save();
				}
			}
		}

		public static float gridSize
		{
			get
			{
				return instance._GridSize;
			}
			set
			{
				value = Mathf.Clamp( value,1.0f,1000.0f );
				if( instance._GridSize != value )
				{
					instance._GridSize = value;

					Save();
				}
			}
		}

		public static int gridSplitNum
		{
			get
			{
				return instance._GridSplitNum;
			}
			set
			{
				value = Mathf.Clamp ( value,1,100 );
				if( instance._GridSplitNum != value )
				{
					instance._GridSplitNum = value;

					Save();
				}
			}
		}

		private static readonly bool _DefaultAutoLanguage = true;
		private static readonly SystemLanguage _DefaultLanguage = SystemLanguage.English;

		[SerializeField]private bool _AutoLanguage = _DefaultAutoLanguage;
		[SerializeField]private SystemLanguage _Language = _DefaultLanguage;

		public static bool autoLanguage
		{
			get
			{
				return instance._AutoLanguage;
			}
			set
			{
				if( instance._AutoLanguage != value )
				{
					instance._AutoLanguage = value;

					Save();
				}
			}
		}

		public static SystemLanguage language
		{
			get
			{
				return instance._Language;
			}
			set
			{
				if( instance._Language != value )
				{
					instance._Language = value;
					
					Save();
				}
			}
		}

		public static SystemLanguage GetAutoLanguage()
		{
			SystemLanguage language = Application.systemLanguage;
			if( !Localization.ContainsLanguage( language ) )
			{
				language = SystemLanguage.English;
			}

			return language;
		}

		public static SystemLanguage currnentLanguage
		{
			get
			{
				if( instance._AutoLanguage )
				{
					return GetAutoLanguage();
				}

				return instance._Language;
			}
		}

		public static void ResetGrid()
		{
			instance._ShowGrid = _DefaultShowGrid;
			instance._SnapGrid = _DefaultSnapGrid;
			instance._GridSize = _DefaultGridSize;
			instance._GridSplitNum = _DefaultGridSplitNum;

			Save();
		}

		[SerializeField]
		private bool _OpenStateList = _DefaultOpenStateList;
		public static bool openStateList
		{
			get
			{
				return instance._OpenStateList;
			}
			set
			{
				if (instance._OpenStateList != value)
				{
					instance._OpenStateList = value;

					Save();
				}
			}
		}

		[SerializeField]
		private float _StateListWidth = _DefaultStateListWidth;
		public static float stateListWidth
		{
			get
			{
				return instance._StateListWidth;
			}
			set
			{
				if (instance._StateListWidth != value)
				{
					instance._StateListWidth = value;

					Save();
				}
			}
		}

		[SerializeField]
		private string _BehaviourSearch = string.Empty;
		public static string behaviourSearch
		{
			get
			{
				return instance._BehaviourSearch;
			}
			set
			{
				if (instance._BehaviourSearch != value)
				{
					instance._BehaviourSearch = value;

					Save();
				}
			}
		}

		static void Load()
		{
			UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget( _FilePath );
			if (_Instance != null)
			{
				return;
			}

			UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget(_OldFilePath);
			if (_Instance != null)
			{
				return;
			}

			ArborSettings instance = ScriptableObject.CreateInstance<ArborSettings>();
			instance.hideFlags = HideFlags.HideAndDontSave;
		}

		static void Save()
		{
			string directoryName = Path.GetDirectoryName(_FilePath);
			if( !Directory.Exists(directoryName) )
			{
				Directory.CreateDirectory(directoryName);
			}
			UnityEditorInternal.InternalEditorUtility.SaveToSerializedFileAndForget( new Object[]{ _Instance }, _FilePath, false );
		}
	}
}
