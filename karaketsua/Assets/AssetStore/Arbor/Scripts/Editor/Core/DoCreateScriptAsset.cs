using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace ArborEditor
{
	internal sealed class DoCreateScriptAsset : EndNameEditAction
	{
		static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string template)
		{
			string fullPath = Path.GetFullPath(pathName);

			string input1 = template;

			string withoutExtension = Path.GetFileNameWithoutExtension(pathName);
			string str1 = Regex.Replace(withoutExtension, " ", string.Empty);
			string str2 = Regex.Replace(input1, "#SCRIPTNAME#", str1);
			
			UTF8Encoding utF8Encoding = new UTF8Encoding(true, false);
			
			bool append = false;
			StreamWriter streamWriter = new StreamWriter(fullPath, append, utF8Encoding );
			streamWriter.Write(str2);
			streamWriter.Close();
			
			AssetDatabase.ImportAsset(pathName);
			return AssetDatabase.LoadAssetAtPath(pathName, typeof (UnityEngine.Object));
		}
		
		public override void Action(int instanceId, string pathName, string resourceFile)
		{
			string template = "";
#if ARBOR_DLL
			Assembly assembly = Assembly.GetExecutingAssembly();
			
			string templatePath = "ArborEditor.ScriptTemplates." + resourceFile;
			
			Stream stream = assembly.GetManifestResourceStream( templatePath );
			
			using( StreamReader reader = new StreamReader( stream ) )
			{
				template = reader.ReadToEnd();
			}
#else
			MonoScript monoScript = MonoScript.FromScriptableObject( this );
			if( monoScript != null )
			{
				string directory = Path.GetDirectoryName( AssetDatabase.GetAssetPath( monoScript ) );
				string templatePath = directory + "/ScriptTemplates/" + resourceFile;

				TextAsset templateAsset = AssetDatabase.LoadAssetAtPath( templatePath,typeof(TextAsset) ) as TextAsset;

				template = templateAsset.text;
			}
#endif
			ProjectWindowUtil.ShowCreatedAsset( CreateScriptAssetFromTemplate(pathName, template) );
		}
	}
}
