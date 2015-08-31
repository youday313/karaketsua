using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ArborEditor
{
	internal sealed class Localization : ScriptableObject
	{
		private static Localization _Instance = null;
		private static Localization instance
		{
			get
			{
				if( _Instance == null )
				{
					_Instance = ScriptableObject.CreateInstance<Localization>();
					_Instance.hideFlags = HideFlags.HideAndDontSave;
					_Instance.LoadAll();
				}
				return _Instance;
			}
		}

		public class WordDictionary : Dictionary<string,string>
		{
		}

		private SortedDictionary<SystemLanguage,WordDictionary> _Languages = new SortedDictionary<SystemLanguage, WordDictionary>();

		void Load( SystemLanguage language,string text )
		{
			WordDictionary wordDic = null;
			if( _Languages.TryGetValue( language,out wordDic ) )
			{
				wordDic.Clear();
			}
			else
			{
				wordDic = new WordDictionary();

				_Languages.Add( language,wordDic );
			}

			foreach( string line in text.Split('\n') )
			{
				if( line.StartsWith("//") )
				{
					continue;
				}

				int firstColonIndex = line.IndexOf(':');
				if (firstColonIndex < 0)
				{
					continue;
				}
                
				string key = line.Substring(0,firstColonIndex);
				string word = line.Substring(firstColonIndex+1).Trim().Replace("\\n", "\n");

				wordDic.Add( key,word );
			}
		}

		void LoadAll()
		{
#if ARBOR_DLL
			Assembly assembly = Assembly.GetExecutingAssembly();
			
			string languageDirctory = "ArborEditor.Languages.";

			foreach( SystemLanguage language in System.Enum.GetValues( typeof(SystemLanguage) ) )
			{
				string languagePath = languageDirctory+language.ToString()+".txt";

				Stream stream = assembly.GetManifestResourceStream( languagePath );

				if( stream != null )
				{
					using( StreamReader reader = new StreamReader( stream ) )
					{
						Load( language,reader.ReadToEnd() );
					}
				}
			}
#else
			MonoScript monoScript = MonoScript.FromScriptableObject( this );
			if( monoScript != null )
			{
				string directory = System.IO.Path.GetDirectoryName( AssetDatabase.GetAssetPath( monoScript ) );
				string languageDirctory = directory + "/Languages/";

				foreach( SystemLanguage language in System.Enum.GetValues( typeof(SystemLanguage) ) )
				{
					string languagePath = languageDirctory+language.ToString()+".txt";

					TextAsset languageAsset = AssetDatabase.LoadAssetAtPath( languagePath,typeof(TextAsset) ) as TextAsset;
					if( languageAsset != null )
					{
						Load( language,languageAsset.text );
					}
				}
			}
#endif
		}

		public static string GetWord( string key )
		{
			WordDictionary wordDic=null;
			if( instance._Languages.TryGetValue( ArborSettings.currnentLanguage,out wordDic ) )
			{
				string word;
				if( wordDic.TryGetValue( key,out word ) )
				{
					return word;
				}
			}

			return key;
		}

		public static SystemLanguage[] GetLanguages()
		{
			if( instance._Languages.Keys.Count == 0 )
			{
				return null;
			}
			SystemLanguage[] languages = new SystemLanguage[instance._Languages.Keys.Count];
			instance._Languages.Keys.CopyTo( languages,0 );
			return languages;
		}

		public static bool ContainsLanguage( SystemLanguage language )
		{
			return instance._Languages.ContainsKey( language );
		}
	}
}