using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	public class ArborFSMInternalInspector : Editor
	{
		private static readonly GUIContent _NameContent = new GUIContent( "Name" );

		public override void OnInspectorGUI()
		{
			ArborFSMInternal stateMachine = target as ArborFSMInternal;

			serializedObject.Update ();

			EditorGUILayout.PropertyField( serializedObject.FindProperty ("fsmName"),_NameContent );

			serializedObject.ApplyModifiedProperties();
			
			if( GUILayout.Button ( "Open Editor" ) )
			{
				ArborEditorWindow.Open( stateMachine );
			}
#if false
			foreach( StateBehaviour behaviour in stateMachine.GetComponents<StateBehaviour>() )
			{
				State state = behaviour.state;
				if( state == null )
				{
					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.LabelField( "Missing Behaviour : " + behaviour.GetType () );
					if( GUILayout.Button( "Delete" ) )
					{
						DestroyImmediate( behaviour );
					}

					EditorGUILayout.EndHorizontal();
				}
			}
#endif
		}
		
		public void OnDestroy()
		{
			if( !target )
			{
				ArborFSMInternal stateMachine = (ArborFSMInternal)target;
				stateMachine.DestroySubComponents();
			}
		}
	}
}