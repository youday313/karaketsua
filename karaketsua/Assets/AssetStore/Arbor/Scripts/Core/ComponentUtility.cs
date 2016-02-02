/// @cond
using UnityEngine;
using System.Collections;

namespace Arbor
{
	public static class ComponentUtility
	{
		public delegate Component EditorAddComponent( GameObject gameObject,System.Type type );
		public delegate void EditorDestroyObjectImmediate( Object obj );
		public delegate void EditorRecordObject(Object obj, string name);
		public delegate void EditorSetDirty(Object obj);
		public delegate void EditorMoveBehaviour( State state,StateBehaviour behaviour );
		public delegate void EditorRefreshBehaviours( ArborFSMInternal stateMachine );

		public static EditorAddComponent editorAddComponent;
		public static EditorDestroyObjectImmediate editorDestroyObjectImmediate;
		public static EditorRecordObject editorRecordObject;
		public static EditorSetDirty editorSetDirty;
		public static EditorMoveBehaviour editorMoveBehaviour;
		public static EditorRefreshBehaviours editorRefreshBehaviours;

		public static bool enabled = true;

		public static Component AddComponent( GameObject gameObject,System.Type type )
		{
			if(enabled && Application.isEditor && !Application.isPlaying && editorAddComponent != null )
			{
				return editorAddComponent( gameObject,type );
			}
			return gameObject.AddComponent( type );
		}

		public static Type AddComponent<Type>( GameObject gameObject ) where Type : Component
		{
			if(enabled && Application.isEditor && !Application.isPlaying && editorAddComponent != null )
			{
				return editorAddComponent( gameObject,typeof(Type) ) as Type;
			}
			return gameObject.AddComponent<Type>();
		}

		public static void Destroy( Object obj )
		{
			if(enabled && Application.isEditor && !Application.isPlaying && editorDestroyObjectImmediate != null )
			{
				editorDestroyObjectImmediate( obj );
				return;
			}
			Object.Destroy( obj );
		}

		public static void RecordObject(Object obj, string name)
		{
			if (enabled && Application.isEditor && !Application.isPlaying && editorRecordObject != null)
			{
				editorRecordObject(obj,name);
				return;
			}
		}

		public static void SetDirty(Object obj)
		{
			if (enabled && Application.isEditor && !Application.isPlaying && editorSetDirty != null)
			{
				editorSetDirty(obj);
				return;
			}
		}

		public static void MoveBehaviour( State state,StateBehaviour behaviour )
		{
			if(enabled && Application.isEditor && !Application.isPlaying && editorMoveBehaviour != null && state != null )
			{
				editorMoveBehaviour( state,behaviour );
				return;
			}
			
			throw new System.NotSupportedException();
		}

		public static void RefreshBehaviours(ArborFSMInternal stateMachine)
		{
			if (enabled && Application.isEditor && !Application.isPlaying && editorRefreshBehaviours != null && stateMachine != null)
			{
				editorRefreshBehaviours(stateMachine);
				return;
			}
		}
	}
}
/// @endcond
