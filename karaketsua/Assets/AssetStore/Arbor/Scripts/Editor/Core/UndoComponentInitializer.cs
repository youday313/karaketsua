using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;

namespace ArborEditor
{
	[InitializeOnLoad]
	class UndoComponentInitializer
	{
		static void RefreshBehaviours(ArborFSMInternal stateMachine)
		{
			bool cachedEnabled = ComponentUtility.enabled;
			ComponentUtility.enabled = false;

			stateMachine.RefreshBehaviours();

			ComponentUtility.enabled = cachedEnabled;
		}

		static UndoComponentInitializer()
		{
			ComponentUtility.editorAddComponent = Undo.AddComponent;
			ComponentUtility.editorDestroyObjectImmediate = Undo.DestroyObjectImmediate;
			ComponentUtility.editorRecordObject = Undo.RecordObject;
            ComponentUtility.editorMoveBehaviour = EditorGUITools.MoveBehaviour;
			ComponentUtility.editorRefreshBehaviours = RefreshBehaviours;
		}
	}
}
