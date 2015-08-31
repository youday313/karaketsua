/// @cond
using UnityEngine;
using System.Collections;

namespace Arbor
{
	public static class ComponentUtility
	{
		public delegate Component EditorAddComponent( GameObject gameObject,System.Type type );
		public delegate void EditorDestroyObjectImmediate( Object obj );
		public delegate void EditorRestoreBehaviour( State state,StateBehaviour behaviour );

		public static EditorAddComponent editorAddComponent;
		public static EditorDestroyObjectImmediate editorDestroyObjectImmediate;
		public static EditorRestoreBehaviour editorRestoreBehaviour;

		public static Component AddComponent( GameObject gameObject,System.Type type )
		{
			if( Application.isEditor && !Application.isPlaying && editorAddComponent != null )
			{
				return editorAddComponent( gameObject,type );
			}
			return gameObject.AddComponent( type );
		}

		public static Type AddComponent<Type>( GameObject gameObject ) where Type : Component
		{
			if( Application.isEditor && !Application.isPlaying && editorAddComponent != null )
			{
				return editorAddComponent( gameObject,typeof(Type) ) as Type;
			}
			return gameObject.AddComponent<Type>();
		}

		public static void Destroy( Object obj )
		{
			if( Application.isEditor && !Application.isPlaying && editorDestroyObjectImmediate != null )
			{
				editorDestroyObjectImmediate( obj );
				return;
			}
			Object.Destroy( obj );
		}

		public static void RestoreBehaviour( State state,StateBehaviour behaviour )
		{
			if( Application.isEditor && !Application.isPlaying && editorRestoreBehaviour != null )
			{
				editorRestoreBehaviour( state,behaviour );
				return;
			}
			
			throw new System.NotSupportedException();
		}
	}
}
/// @endcond