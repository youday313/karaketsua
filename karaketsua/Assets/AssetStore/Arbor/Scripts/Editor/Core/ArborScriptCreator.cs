using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ArborEditor
{
	class ArborScriptCreator
	{
		static readonly string _CSharpTemplatePath = @"C# Script-NewBehaviourScript.txt";
		static readonly Texture2D _CSharpIcon = EditorGUIUtility.FindTexture("cs Script Icon");

		static readonly string _JavaScriptTemplatePath = @"Javascript-NewBehaviourScript.txt";
		static readonly Texture2D _JavaScriptIcon = EditorGUIUtility.FindTexture("js Script Icon");

		static readonly string _BooScriptTemplatePath = @"Boo Script-NewBehaviourScript.txt";
		static readonly Texture2D _BooScriptIcon = EditorGUIUtility.FindTexture("boo Script Icon");

		[MenuItem("Assets/Create/Arbor/C# Script")]
		public static void CreateCSharpScript()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), "NewArborScript.cs", _CSharpIcon, _CSharpTemplatePath );
		}

		[MenuItem("Assets/Create/Arbor/JavaScript")]
		public static void CreateJavaScript()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), "NewArborScript.js", _JavaScriptIcon, _JavaScriptTemplatePath );
		}

		[MenuItem("Assets/Create/Arbor/Boo Script")]
		public static void CreateBooScript()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateScriptAsset>(), "NewArborScript.boo", _BooScriptIcon, _BooScriptTemplatePath );
		}
	}
}