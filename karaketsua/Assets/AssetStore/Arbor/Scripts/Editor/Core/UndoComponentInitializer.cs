using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[InitializeOnLoad]
	class UndoComponentInitializer
	{
		static UndoComponentInitializer()
		{
			ComponentUtility.editorAddComponent = Undo.AddComponent;
			ComponentUtility.editorDestroyObjectImmediate = Undo.DestroyObjectImmediate;
			ComponentUtility.editorRestoreBehaviour = EditorGUITools.RestoreBehaviour;
		}
	}
}
