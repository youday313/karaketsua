using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	internal class EditorGUITools
	{
		private static int s_BehaviourTitlebarHash = "s_BehaviourTitlebarHash".GetHashCode();
		private static int s_StateTitlebarHash = "s_StateTitlebarHash".GetHashCode();
		private static int s_StateLinkHash = "s_StateLinkHash".GetHashCode();
		
		private static GUIStyle s_BehaviourTitlebar = (GUIStyle)"IN Title";
		private static GUIStyle s_BehaviourTitlebarText = (GUIStyle)"IN TitleText";
		private static GUIStyle s_StateTitlebar = (GUIStyle)"IN BigTitle";

		private static GUIStyle s_GridBackground = UnityEditor.Graphs.Styles.graphBackground;

		private static GUIContent s_ContextPopupContent = new GUIContent( EditorGUIUtility.FindTexture ( "_Popup" ) );
		private static GUIContent s_HelpButtonContent = new GUIContent( EditorGUIUtility.FindTexture ( "_Help" ) );

		private static Texture2D s_lineTexture = (Texture2D)UnityEditor.Graphs.Styles.connectionTexture.image;

		private static Material s_HandleWireMaterial2D;

		private static Material handleWireMaterial2D
		{
			get
			{
				if (!(bool) ((Object) s_HandleWireMaterial2D))
				{
					s_HandleWireMaterial2D = (Material) EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
				}
				return s_HandleWireMaterial2D;
			}
		}

		private static MethodInfo _SearchField;
		private static MethodInfo _ToolbarSearchField;
		
		static EditorGUITools()
		{
			_SearchField = typeof(EditorGUI).GetMethod("SearchField", BindingFlags.Static | BindingFlags.NonPublic);
			_ToolbarSearchField = typeof(EditorGUI).GetMethod("ToolbarSearchField", BindingFlags.Static | BindingFlags.NonPublic,null,
				new System.Type[] {typeof(Rect),typeof(string[]),typeof(int).MakeByRefType(), typeof(string) },null);
		}

		public static string SearchField(Rect position, string text)
		{
			if (_SearchField == null)
			{
				return text;
			}
			var args = new object[] { position, text };
			return _SearchField.Invoke(null, args) as string;
		}

		public static string ToolbarSearchField(Rect position, string[] searchModes, ref int searchMode, string text)
		{
			if (_SearchField == null)
			{
				return text;
			}
			var args = new object[] { position, searchModes, searchMode, text };

			text = _ToolbarSearchField.Invoke(null, args) as string;

			searchMode = (int)args[2];

			return text;
		}

		private static Dictionary<string,GUIContent> _TextContents = new Dictionary<string, GUIContent>();
		public static GUIContent GetTextContent( string key )
		{
			string word = Localization.GetWord( key );

			GUIContent content = null;
			if( !_TextContents.TryGetValue( word,out content ) )
			{
				content = new GUIContent( word );
				_TextContents.Add( word,content );
			}

			return content;
		}

		public static void DrawArraw( Vector2 position,Vector2 direction,Color color,float width )
		{
			Vector2 cross = Vector3.Cross( direction,Vector3.forward ).normalized;
			
			Vector3[] vector3Array = new Vector3[4];
			vector3Array[0] = position;
			vector3Array[1] = position - direction * width + cross * width * 0.5f;
			vector3Array[2] = position - direction * width - cross * width * 0.5f;
			vector3Array[3] = vector3Array[0];
			
			Shader.SetGlobalColor("_HandleColor", color);
			handleWireMaterial2D.SetPass(0);
			GL.Begin(4);
			GL.Color(color);
			GL.Vertex(vector3Array[0]);
			GL.Vertex(vector3Array[1]);
			GL.Vertex(vector3Array[2]);
			GL.End();
		}

		public static void BezierArrow( Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width,float arrowWidth )
		{
			Vector2 v = (end-endTangent).normalized*arrowWidth;
			
			Handles.DrawBezier( start,end-v,startTangent,endTangent-v,color,s_lineTexture,width );

			DrawArraw( end,v.normalized,color,arrowWidth );
		}

		public static void DrawGridBackground( Rect position )
		{
			if (Event.current.type == EventType.Repaint)
			{
				s_GridBackground.Draw(position, false, false, false, false);
			}
		}

		static Material _HandleWireMaterial = null;

		public static Material handleWireMaterial
		{
			get
			{
				if( _HandleWireMaterial == null )
				{
					_HandleWireMaterial = (Material) EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
				}

				return _HandleWireMaterial;
			}
		}

		private static readonly Color _GridMinorColorDark = new Color(0.0f, 0.0f, 0.0f, 0.18f);
		private static readonly Color _GridMajorColorDark = new Color(0.0f, 0.0f, 0.0f, 0.28f);
		private static readonly Color _GridMinorColorLight = new Color(0.0f, 0.0f, 0.0f, 0.1f);
		private static readonly Color _GridMajorColorLight = new Color(0.0f, 0.0f, 0.0f, 0.15f);
		
		public static Color gridMinorColor
		{
			get
			{
				if (EditorGUIUtility.isProSkin)
					return _GridMinorColorDark;
				else
					return _GridMinorColorLight;
			}
		}

		public static Color gridMajorColor
		{
			get
			{
				if (EditorGUIUtility.isProSkin)
					return _GridMajorColorDark;
				else
					return _GridMajorColorLight;
			}
		}
		
		static void SetStartStateContextMenu( object obj )
		{
			State state = obj as State;
			
			ArborFSMInternal stateMachine = state.stateMachine;
			
			SerializedObject serializedObject = new SerializedObject( stateMachine );
			
			serializedObject.Update();
			
			SerializedProperty startStateIDPropery = serializedObject.FindProperty( "_StartStateID" );
			
			startStateIDPropery.intValue = state.stateID;
			
			serializedObject.ApplyModifiedProperties();
			
			serializedObject.Dispose();
		}

		static GameObject _Clipboard = null;
		static GameObject clipboard
		{
			get
			{
				if( _Clipboard == null )
				{
					_Clipboard = EditorUtility.CreateGameObjectWithHideFlags( "Clipboard",HideFlags.HideAndDontSave );
					GameObject.DontDestroyOnLoad( _Clipboard );
				}
				
				return _Clipboard;
			}
		}

		static ArborFSMInternal _NodeClipboard = null;
		static List<Node> _CopyNodes = new List<Node>();

		public static bool isCopyedNode
		{
			get
			{
				return _CopyNodes.Count != 0;
			}
		}

		public static void CopyNodes( Node[] nodes )
		{
			if(_NodeClipboard != null )
			{
				Object.DestroyImmediate(_NodeClipboard);
				_NodeClipboard = null;
			}
			_NodeClipboard = clipboard.AddComponent<ArborFSMInternal>();
			_NodeClipboard.hideFlags |= HideFlags.HideAndDontSave;

			_CopyNodes.Clear ();

			foreach( Node node in nodes)
			{
				if (node is State)
				{
					State state = node as State;
					State copyState = _NodeClipboard.CreateState(state.resident);

					copyState.name = state.name;
					copyState.position = state.position;

					foreach (StateBehaviour behaviour in state.behaviours)
					{
						StateBehaviour copyBehaviour = copyState.AddBehaviour(behaviour.GetType());

						copyBehaviour.hideFlags |= HideFlags.HideAndDontSave;

						EditorUtility.CopySerialized(behaviour, copyBehaviour);
					}

					_CopyNodes.Add(copyState);
				}
				else if (node is CommentNode)
				{
					CommentNode comment = node as CommentNode;
					CommentNode copyComment = _NodeClipboard.CreateComment();

					copyComment.position = comment.position;
					copyComment.comment = comment.comment;

					_CopyNodes.Add(copyComment);
				}
			}
		}

		public static Node[] DuplicateNodes( ArborFSMInternal stateMachine,Vector2 position,Node[] sourceNodes )
		{
			List<Node> duplicateNodes = new List<Node>();

			Vector2 minPosition = new Vector2( float.MaxValue,float.MaxValue );
			
			foreach(Node sourceNode in sourceNodes)
			{
				minPosition.x = Mathf.Min(sourceNode.position.x,minPosition.x );
				minPosition.y = Mathf.Min(sourceNode.position.y,minPosition.y );
			}

			position -= minPosition;
			
			foreach(Node sourceNode in sourceNodes)
			{
				if (sourceNode is State)
				{
					State sourceState = sourceNode as State;
					State state = stateMachine.CreateState(sourceState.resident);

					if (state != null)
					{
						state.name = sourceState.name;
						state.position = sourceState.position;
						state.position.x += position.x;
						state.position.y += position.y;

						ComponentUtility.EditorAddComponent cachedAddComponent = ComponentUtility.editorAddComponent;
						ComponentUtility.editorAddComponent = null;

						foreach (StateBehaviour sourceBehaviour in sourceState.behaviours)
						{
							StateBehaviour behaviour = state.AddBehaviour(sourceBehaviour.GetType());

							if (behaviour != null)
							{
								CopyBehaviour(sourceBehaviour, behaviour, true);
							}
						}

						ComponentUtility.editorAddComponent = cachedAddComponent;

						duplicateNodes.Add(state);
					}
				}
				else if (sourceNode is CommentNode)
				{
					CommentNode sourceComment = sourceNode as CommentNode;
					CommentNode comment = stateMachine.CreateComment();

					if (comment != null)
					{
						comment.comment = sourceComment.comment;
						comment.position = sourceComment.position;
						comment.position.x += position.x;
						comment.position.y += position.y;

						duplicateNodes.Add(comment);
					}
				}
			}
			
			return duplicateNodes.ToArray();
		}

		public static void RestoreBehaviour( State state,StateBehaviour sourceBehaviour )
		{
			ComponentUtility.EditorAddComponent cachedAddComponent = ComponentUtility.editorAddComponent;
			ComponentUtility.editorAddComponent = null;
			
			StateBehaviour destBehaviour = state.AddBehaviour( sourceBehaviour.GetType () );
			
			if( destBehaviour != null )
			{
				CopyBehaviour( sourceBehaviour,destBehaviour,false );
			}

			ComponentUtility.editorAddComponent = cachedAddComponent;
		}

		public static Node[] PasteNodes( ArborFSMInternal stateMachine,Vector2 position )
		{
			return DuplicateNodes( stateMachine,position,_CopyNodes.ToArray() );
		}

		static void PasteBehaviourToStateContextMenu( object obj )
		{
			State state = obj as State;
			
			Undo.IncrementCurrentGroup();
			
			ArborFSMInternal stateMachine = state.stateMachine;

			Undo.RecordObject( stateMachine,"Paste Behaviour" );

			StateBehaviour behaviour = state.AddBehaviour( _CopyBehaviour.GetType() );

			CopyBehaviour( _CopyBehaviour,behaviour,true );

			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );

			EditorUtility.SetDirty( stateMachine );
		}

		internal static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector2 = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector2.x;
			guiRect.y = vector2.y;
			return guiRect;
		}

		static void AddBehaviourToStateContextMenu(object obj)
		{
			KeyValuePair<State, Rect> pair = (KeyValuePair<State, Rect>)obj;
			State state = pair.Key;
			Rect position = pair.Value;

			BehaviourMenuWindow.instance.Init(state, position);
		}

		public static void StateTitlebar( Rect position,State state )
		{
			int controlId = GUIUtility.GetControlID(s_StateTitlebarHash,EditorGUIUtility.native, position);

			Event current = Event.current;
			
			EventType typeForControl = current.GetTypeForControl(controlId);

	//		position.x -= 4;
	//		position.width += 3+5;
			position.y -= 5;
			position.height += 5+3;
			
			Rect namePosition = s_StateTitlebar.padding.Remove(position);
			namePosition.height = 16;
			namePosition.width -= 16+8;
			
			Rect popupPosition = new Rect( namePosition.xMax+8 , namePosition.y , 16 , namePosition.height );

			if( current.type == EventType.Repaint )
			{
				s_StateTitlebar.Draw( position,GUIContent.none,controlId,false );
			}

			string name = EditorGUI.TextField( namePosition,state.name );
			if( name != state.name )
			{
				ArborFSMInternal stateMachine = state.stateMachine;
				
				Undo.RecordObject( stateMachine,"Rename State" );
				
				state.name = name;

				EditorUtility.SetDirty( stateMachine );
			}
			
			switch (typeForControl)
			{
			case EventType.MouseDown:
				if( popupPosition.Contains( current.mousePosition ) )
				{
					GenericMenu menu = new GenericMenu();
					
					SerializedObject serializedObject = new SerializedObject( state.stateMachine );
					
					SerializedProperty startStateIDPropery = serializedObject.FindProperty( "_StartStateID" );
					
					if( startStateIDPropery.intValue == state.stateID )
					{
						menu.AddDisabledItem( GetTextContent("Set Start State") );
					}
					else
					{
						menu.AddItem( GetTextContent("Set Start State"),false,SetStartStateContextMenu,state );
					}

					//BehaviourMenuUtility.AddMenu( state,menu );

					menu.AddItem(GetTextContent("Add Behaviour"), false, AddBehaviourToStateContextMenu, new KeyValuePair<State,Rect>(state, GUIToScreenRect( position) ) );

					if ( _CopyBehaviour != null )
					{
						menu.AddItem( GetTextContent("Paste Behaviour"),false,PasteBehaviourToStateContextMenu,state );
					}
					else
					{
						menu.AddDisabledItem( GetTextContent("Paste Behaviour") );
					}

					serializedObject.Dispose();
					
					menu.DropDown( popupPosition );
					
					current.Use();
				}
				break;
			case EventType.Repaint:
				s_BehaviourTitlebarText.Draw(popupPosition, s_ContextPopupContent, controlId, false);
				break;
			}
		}
		
		public static void StateTitlebar( State state )
		{
			Rect position = GUILayoutUtility.GetRect( 0.0f,20.0f );
			StateTitlebar( position,state );
		}

		public static void CommentField(Rect position, CommentNode comment)
		{
			RectOffset offset = new RectOffset(6, 6, 0, 6);
			position = offset.Remove(position);
			
			string commentText = EditorGUI.TextArea(position, comment.comment);
			if (commentText != comment.comment)
			{
				ArborFSMInternal stateMachine = comment.stateMachine;

				Undo.RecordObject(stateMachine, "Change Comment");

				comment.comment = commentText;

				EditorUtility.SetDirty(stateMachine);
			}
		}

		public static void CommentField(CommentNode comment)
		{
			Rect position = GUILayoutUtility.GetRect(0.0f,50.0f);
			CommentField(position, comment);
		}

		static void DeleteBehaviourContextMenu( object obj )
		{
			StateBehaviour behaviour = obj as StateBehaviour;
			ArborFSMInternal stateMachine = behaviour.stateMachine;

			Undo.IncrementCurrentGroup();
			
			Undo.RecordObject( stateMachine,"Delete Behaviour" );

			behaviour.Destroy();
			
			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );

			EditorUtility.SetDirty( stateMachine );
		}

		static void MoveUpBehaviourContextMenu( object obj )
		{
			KeyValuePair<State,int> pair = (KeyValuePair<State,int>)obj;

			State state = pair.Key;
			ArborFSMInternal stateMachine = state.stateMachine;

			Undo.IncrementCurrentGroup();
			
			Undo.RecordObject( stateMachine,"MoveUp Behaviour" );

			int index = pair.Value;

			state.SwapBehaviour( index,index-1 );

			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );

			EditorUtility.SetDirty( stateMachine );
		}

		static void MoveDownBehaviourContextMenu( object obj )
		{
			KeyValuePair<State,int> pair = (KeyValuePair<State,int>)obj;
			
			State state = pair.Key;
			ArborFSMInternal stateMachine = state.stateMachine;
			
			Undo.IncrementCurrentGroup();
			
			Undo.RecordObject( stateMachine,"MoveDown Behaviour" );
			
			int index = pair.Value;

			state.SwapBehaviour( index,index+1 );

			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );

			EditorUtility.SetDirty( stateMachine );
		}

		static StateBehaviour _CopyBehaviour = null;

		static void CopyBehaviourContextMenu( object obj )
		{
			StateBehaviour behaviour = obj as StateBehaviour;

			if( _CopyBehaviour != null )
			{
				Object.DestroyImmediate( _CopyBehaviour );
				_CopyBehaviour = null;
			}

			_CopyBehaviour = clipboard.AddComponent( behaviour.GetType () ) as StateBehaviour;

			EditorUtility.CopySerialized( behaviour,_CopyBehaviour );
		}

		static void CopyBehaviour( StateBehaviour source,StateBehaviour dest,bool checkStateLink )
		{
			if( dest == null )
			{
				return;
			}

			ArborFSMInternal stateMachine = dest.stateMachine;
			int stateID = dest.stateID;
			bool expanded = dest.expanded;
			
			EditorUtility.CopySerialized( source,dest );
			
			dest.expanded = expanded;
			
			SerializedObject serializedObject = new SerializedObject( dest );
			
			serializedObject.Update();
			
			SerializedProperty stateMachineProperty = serializedObject.FindProperty( "_StateMachine" );
			stateMachineProperty.objectReferenceValue = stateMachine;
			
			SerializedProperty stateIDProperty = serializedObject.FindProperty( "_StateID" );
			stateIDProperty.intValue = stateID;

			if( checkStateLink )
			{			
				SerializedProperty iterator = serializedObject.GetIterator();
				while( iterator.NextVisible(true) )
				{
					if( iterator.type == "StateLink" )
					{
						if( iterator.isArray )
						{
							for( int i=0;i<iterator.arraySize;i++ )
							{
								SerializedProperty stateLinkProperty = iterator.GetArrayElementAtIndex( i );
								
								SerializedProperty property = stateLinkProperty.FindPropertyRelative( "stateID" );
								if( property.intValue == stateID || stateMachine != source.stateMachine || stateMachine.GetStateFromID( property.intValue ) == null )
								{
									property.intValue = 0;
								}
							}
						}
						else
						{
							SerializedProperty property = iterator.FindPropertyRelative( "stateID" );
							if( property.intValue == stateID || stateMachine != source.stateMachine || stateMachine.GetStateFromID( property.intValue ) == null )
							{
								property.intValue = 0;
							}
						}
					}
				}
			}
			
			serializedObject.ApplyModifiedProperties();
			serializedObject.Dispose();
		}

		static void PasteBehaviourContextMenu( object obj )
		{
			StateBehaviour behaviour = obj as StateBehaviour;

			Undo.IncrementCurrentGroup();
			
			Undo.RecordObject( behaviour,"Paste Behaviour" );

			CopyBehaviour( _CopyBehaviour,behaviour,true );

			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
			
			EditorUtility.SetDirty( behaviour );
		}

		static void EditScriptBehaviourContextMenu( object obj )
		{
			MonoScript script = obj as MonoScript;

			AssetDatabase.OpenAsset( script );
		}

		public static bool BehaviourTitlebar( Rect position,bool foldout,StateBehaviour behaviour )
		{
			int controlId = GUIUtility.GetControlID(s_BehaviourTitlebarHash,EditorGUIUtility.native, position);

			Event current = Event.current;

			//foldout = EditorGUI.Foldout( position,foldout,GUIContent.none,s_BehaviourTitlebar );

			Rect iconPosition = new Rect(position.x + (float) s_BehaviourTitlebar.padding.left , position.y + (float)s_BehaviourTitlebar.padding.top, 16f, 16f );

			Rect checkPosition = new Rect( iconPosition.xMax,iconPosition.y,16f,16f );

			Rect popupPosition = new Rect( position.xMax - (float)s_BehaviourTitlebar.padding.right - 2.0f - 16.0f, iconPosition.y, 16f, 16f);

			Rect helpPosition = new Rect( popupPosition.x - 18.0f, iconPosition.y, 16f, 16f);

			Rect textPosition = new Rect( checkPosition.xMax + 4.0f, iconPosition.y, helpPosition.x - iconPosition.xMax - 4.0f  - 4.0f, iconPosition.height);

			string titleName = behaviour.GetType().Name;

			object[] objs = behaviour.GetType ().GetCustomAttributes( typeof(BehaviourTitle),false );

			if( objs!=null && objs.Length > 0 )
			{
				BehaviourTitle attr = (BehaviourTitle)objs[0];
				titleName = attr.titleName;
			}

			System.Type classType = behaviour.GetType();

			string siteURL = Localization.GetWord("SiteURL");

			string helpUrl = siteURL;

			string helpTooltip = "Open Arbor Document";

			object[] attributes = classType.GetCustomAttributes( typeof(BehaviourHelp),false );
			if (attributes != null && attributes.Length > 0)
			{
				BehaviourHelp help = attributes[0] as BehaviourHelp;
				helpUrl = help.url;

				helpTooltip = string.Format("Open Reference for {0}.", classType.Name);
			}
			else
			{
				attributes = classType.GetCustomAttributes(typeof(BuiltInBehaviour), false);
				if (attributes != null && attributes.Length > 0)
				{
					helpUrl = siteURL + "manual/behaviour-reference/"+ classType.Name.ToLower() + "/";
					
					helpTooltip = string.Format("Open Reference for {0}.", classType.Name);
				}
			}

			EditorGUI.BeginChangeCheck();
			bool behaviourEnabled = EditorGUI.Toggle( checkPosition,behaviour.behaviourEnabled );
			if( EditorGUI.EndChangeCheck() )
			{
				Undo.RecordObject( behaviour,( !behaviourEnabled? "Disable" : "Enable" ) + " Behaviour" );
				behaviour.behaviourEnabled = behaviourEnabled;
				EditorUtility.SetDirty( behaviour );
			}

			HelpButton( helpPosition,helpUrl,helpTooltip );

			EventType typeForControl = current.GetTypeForControl(controlId);
			switch (typeForControl)
			{
			case EventType.MouseDown:
				if( popupPosition.Contains( current.mousePosition ) )
				{
					GenericMenu menu = new GenericMenu();

					State state = behaviour.state;

					int index = -1;
					StateBehaviour[] behaviours = state.behaviours;
					for( int i=0;i<behaviours.Length;i++ )
					{
						if( behaviours[i] == behaviour )
						{
							index = i;
							break;
						}
					}

					if( index >= 1 )
					{
						menu.AddItem( GetTextContent("Move Up"),false,MoveUpBehaviourContextMenu,new KeyValuePair<State,int>(state,index) );
					}
					else
					{
						menu.AddDisabledItem( GetTextContent("Move Up") );
					}

					if( index < behaviours.Length -1 )
					{
						menu.AddItem( GetTextContent("Move Down"),false,MoveDownBehaviourContextMenu,new KeyValuePair<State,int>(state,index) );
					}
					else
					{
						menu.AddDisabledItem( GetTextContent("Move Down") );
					}

					menu.AddItem ( GetTextContent("Copy"),false,CopyBehaviourContextMenu,behaviour );

					if( _CopyBehaviour != null && _CopyBehaviour.GetType () == behaviour.GetType () )
					{
						menu.AddItem ( GetTextContent("Paste"),false,PasteBehaviourContextMenu,behaviour );
					}
					else
					{
						menu.AddDisabledItem( GetTextContent("Paste") );
					}
					
					menu.AddItem( GetTextContent("Delete"),false,DeleteBehaviourContextMenu,behaviour );

					menu.AddSeparator( "" );

					MonoScript script = MonoScript.FromMonoBehaviour( behaviour );

					if( script != null )
					{
						menu.AddItem( GetTextContent("Edit Script"),false,EditScriptBehaviourContextMenu,script );
					}
					else
					{
						menu.AddDisabledItem( GetTextContent("Edit Script") );
					}

					menu.DropDown( popupPosition );
					
					current.Use();
				}
				else if( checkPosition.Contains ( current.mousePosition ) )
				{
					if (current.button == 0 && (Application.platform != RuntimePlatform.OSXEditor || !current.control) )
					{

					}
				}
				else if( position.Contains( current.mousePosition ) )
				{
					if (current.button == 0 && (Application.platform != RuntimePlatform.OSXEditor || !current.control) )
					{
						GUIUtility.hotControl = GUIUtility.keyboardControl = controlId;
						current.Use();
					}
				}
				break;
			case EventType.MouseUp:
				if( GUIUtility.hotControl == controlId )
				{
					GUIUtility.hotControl = 0;

					if( position.Contains(current.mousePosition) )
					{
						foldout = !foldout;
					}
					current.Use();
				}
				break;
			case EventType.KeyDown:
				if (GUIUtility.keyboardControl == controlId)
				{
					if( current.keyCode == KeyCode.LeftArrow )
					{
						foldout = false;
						current.Use();
					}
					if( current.keyCode == KeyCode.RightArrow )
					{
						foldout = true;
						current.Use();
						break;
					}
				}
				break;
			case EventType.Repaint:
				s_BehaviourTitlebar.Draw ( position,GUIContent.none,controlId,foldout );
				GUIStyle.none.Draw( iconPosition,new GUIContent( AssetPreview.GetMiniThumbnail(behaviour) ),controlId,foldout );
				s_BehaviourTitlebarText.Draw( textPosition, new GUIContent( ObjectNames.NicifyVariableName( titleName ) ),controlId,foldout );
				s_BehaviourTitlebarText.Draw( popupPosition, s_ContextPopupContent,controlId,foldout );
				break;
			}

			return foldout;
		}

		public static bool BehaviourTitlebar( bool foldout,StateBehaviour behaviour )
		{
			Rect position = GUILayoutUtility.GetRect(GUIContent.none, s_BehaviourTitlebar );

			return BehaviourTitlebar( position,foldout,behaviour );
		}

		public class Pivot
		{
			public Vector2 position;
			public Vector2 normal;

			public Pivot( Vector2 position,Vector2 normal )
			{
				this.position = position;
				this.normal = normal;
			}
		}

		private static State GetStateFromPosition( ArborFSMInternal stateMachine,Vector2 position )
		{
			foreach( State state in stateMachine.states )
			{
				if( !state.resident && state.position.Contains( position ) )
				{
					return state;
				}
			}

			return null;
		}

		public struct Bezier
		{
			public Vector2 startPos;
			public Vector2 startTangent;
			public Vector2 endPos;
			public Vector2 endTangent;
		}

		static Bezier GetTargetBezier( State state,Vector2 targetPos,Vector2 leftPos,Vector2 rightPos )
		{
			Bezier bezier = new Bezier();

			bezier.endPos = targetPos;

			bool right = (targetPos-leftPos).magnitude > (targetPos-rightPos).magnitude;

			if( right )
			{
				bezier.startPos = rightPos;
				bezier.startTangent = rightPos + new Vector2( 50.0f,0.0f );
			}
			else
			{
				bezier.startPos = leftPos;
				bezier.startTangent = leftPos - new Vector2( 50.0f,0.0f );
			}

			bezier.endTangent = bezier.startTangent;

			return bezier;
		}

		static Bezier GetTargetBezier( State state,State target,Vector2 leftPos,Vector2 rightPos)
		{
			bool right = true;

			Bezier bezier = new Bezier();

			if( target != null )
			{
				Rect targetRect = target.position;
				targetRect.x -= state.position.x;
				targetRect.y -= state.position.y;

				Pivot findPivot = null;
				
				List<Pivot> pivots = new List<Pivot>();
				
				pivots.Add( new Pivot( new Vector2( targetRect.xMin,targetRect.yMin + 32.0f ),-Vector2.right ) );
				pivots.Add( new Pivot( new Vector2( targetRect.xMax,targetRect.yMin + 32.0f ),Vector2.right ) );
				
				if( targetRect.x == 0.0f )
				{
					if( targetRect.y > 0.0f )
					{
						findPivot = pivots[0];
						right = false;
					}
					else
					{
						findPivot = pivots[1];
						right = true;
					}
				}
				else
				{					
					float findDistance = 0.0f;
					
					foreach( Pivot pivot in pivots )
					{
						Vector2 vl = leftPos-pivot.position;
						Vector2 vr = rightPos-pivot.position;
						
						float leftDistance = vl.magnitude;
						float rightDistance = vr.magnitude;
						
						float distance = 0.0f;
						bool checkRight = false;
						
						if( leftDistance > rightDistance )
						{
							distance = rightDistance;
							checkRight = true;
						}
						else
						{
							distance = leftDistance;
							checkRight = false;
						}
						
						if( findPivot == null || distance < findDistance )
						{
							findPivot = pivot;
							findDistance = distance;
							right = checkRight;
						}
					}
				}
				
				if( right )
				{
					bezier.startPos = rightPos;
					bezier.startTangent = rightPos + new Vector2( 50.0f,0.0f );
				}
				else
				{
					bezier.startPos = leftPos;
					bezier.startTangent = leftPos - new Vector2( 50.0f,0.0f );
				}
				
				bezier.endPos = findPivot.position;
				bezier.endTangent = bezier.endPos + findPivot.normal * 50;
			}

			return bezier;
		}

		private static State _DragTargetState = null;

		public static void SingleStateLinkField( Rect position, GUIContent label, SerializedProperty property )
		{
			StateBehaviour behaviour = property.serializedObject.targetObject as StateBehaviour;
			
			if( behaviour == null || behaviour.stateID == 0 || behaviour.stateMachine == null || property.isArray )
			{
				EditorGUI.HelpBox( position,"This is Arbor Editor only.",MessageType.Error );
				
				return;
			}
			
			int controlID = EditorGUIUtility.GetControlID( s_StateLinkHash,EditorGUIUtility.native,position );
			
			Event currentEvent = Event.current;
			
			EventType eventType = currentEvent.GetTypeForControl( controlID );
			
			ArborFSMInternal stateMachine = behaviour.stateMachine;
			State state = stateMachine.GetStateFromID( behaviour.stateID );
			
			SerializedProperty stateIDProperty = property.FindPropertyRelative( "stateID" );
			SerializedProperty lineEnableProperty = property.FindPropertyRelative( "lineEnable" );
			SerializedProperty lineStartProperty = property.FindPropertyRelative( "lineStart" );
			SerializedProperty lineStartTangentProperty = property.FindPropertyRelative( "lineStartTangent" );
			SerializedProperty lineEndProperty = property.FindPropertyRelative( "lineEnd" );
			SerializedProperty lineEndTangentProperty = property.FindPropertyRelative( "lineEndTangent" );
			SerializedProperty lineColorProperty = property.FindPropertyRelative( "lineColor" );
			SerializedProperty lineColorChangedProperty = property.FindPropertyRelative( "lineColorChanged" );

			ArborEditorWindow window = ArborEditorWindow.GetCurrent();

			State linkState = stateMachine.GetStateFromID( stateIDProperty.intValue );

			bool dragging = ( GUIUtility.hotControl == controlID && currentEvent.button == 0 );

			State targetState = dragging? _DragTargetState : linkState;

			Vector2 nowPos = currentEvent.mousePosition;

			Vector2 leftPos = new Vector2( position.x+8,position.center.y );
			Vector2 rightPos = new Vector2( position.x+position.width-8,position.center.y );

			Bezier bezier = new Bezier();
			if( targetState != null )
			{
				bezier = GetTargetBezier( state,targetState,leftPos,rightPos );
			}
			else if( dragging )
			{
				bezier = GetTargetBezier( state,nowPos,leftPos,rightPos );
			}
			else
			{
				bezier.startPos = rightPos;
			}
			
			Rect boxRect = new Rect( bezier.startPos.x-8,position.y,16,position.height );

			Vector2 statePosition = new Vector2( state.position.x,state.position.y );

			bezier.startPos += statePosition;
			bezier.startTangent += statePosition;
			bezier.endPos += statePosition;
			bezier.endTangent += statePosition;

			Rect colorRect = position;
			
			colorRect.x += colorRect.width - 32 - 16;
			colorRect.width = 32;

			Color lineColor = Color.white;
			
			if( lineColorChangedProperty.boolValue )
			{
				lineColor = lineColorProperty.colorValue;
			}

			switch( eventType )
			{
			case EventType.MouseDown:
				if( position.Contains( nowPos ) && !colorRect.Contains( nowPos ) )
				{
					if (currentEvent.button == 0)
					{
						GUIUtility.hotControl = GUIUtility.keyboardControl = controlID;

						_DragTargetState = null;

						if (window != null)
						{
							window.DragBranchEnable(true);
							window.DragBranchBezie(bezier.startPos, bezier.startTangent, bezier.endPos, bezier.endTangent);
							window.DragBranchHoverStateID(0);
						}

						lineEnableProperty.boolValue = false;

						currentEvent.Use();
					}
				}
				break;
			case EventType.MouseDrag:
				if( GUIUtility.hotControl == controlID && currentEvent.button == 0 )
				{
					DragAndDrop.PrepareStartDrag();

					State nextState = GetStateFromPosition( stateMachine, nowPos+statePosition );
					
					if( nextState != null && nextState != state )
					{
						if( window )
						{
							window.DragBranchHoverStateID( nextState.stateID );
						}
						
						_DragTargetState = nextState;
					}
					else
					{
						if( window )
						{
							window.DragBranchHoverStateID( 0 );
						}
						_DragTargetState = null;
					}

					currentEvent.Use ();
				}
				break;
			case EventType.MouseUp:
				if( GUIUtility.hotControl == controlID )
				{
					if( currentEvent.button == 0 )
					{
						GUIUtility.hotControl = 0;

						if( _DragTargetState != linkState )
						{
							Undo.RecordObject( behaviour,"Link State" );

							if( _DragTargetState != null )
							{
								stateIDProperty.intValue = _DragTargetState.stateID;
							}
							else
							{
								stateIDProperty.intValue = 0;
							}

							lineEnableProperty.boolValue = stateIDProperty.intValue != 0;
							lineStartProperty.vector2Value = bezier.startPos;
							lineStartTangentProperty.vector2Value = bezier.startTangent;
							lineEndProperty.vector2Value = bezier.endPos;
							lineEndTangentProperty.vector2Value = bezier.endTangent;

							//EditorUtility.SetDirty( behaviour );
						}
						
						if( window != null )
						{
							window.DragBranchEnable(false);
							window.DragBranchHoverStateID( 0 );
							window.Repaint();
						}

						_DragTargetState = null;

						currentEvent.Use ();
					}
				}
				break;
			case EventType.Repaint:
				if( GUIUtility.hotControl == controlID && currentEvent.button == 0 )
				{
					if( window )
					{
						window.DragBranchBezie( bezier.startPos,bezier.startTangent,bezier.endPos,bezier.endTangent );
					}
				}
				else if( linkState != null )
				{
					lineEnableProperty.boolValue = true;
					lineStartProperty.vector2Value = bezier.startPos;
					lineStartTangentProperty.vector2Value = bezier.startTangent;
					lineEndProperty.vector2Value = bezier.endPos;
					lineEndTangentProperty.vector2Value = bezier.endTangent;
				}

				bool on = GUIUtility.hotControl == controlID && currentEvent.button == 0 || linkState != null;

				Color savedColor = GUI.backgroundColor;

				GUI.backgroundColor = new Color( lineColor.r,lineColor.g,lineColor.b );
				EditorStyles.miniButton.Draw( position,label,controlID,on );

				GUI.backgroundColor = savedColor;

				EditorStyles.radioButton.Draw( boxRect,GUIContent.none,controlID,on );
				break;
			}

			EditorGUI.BeginChangeCheck();
			lineColor = EditorGUI.ColorField( colorRect,lineColor );
			if( EditorGUI.EndChangeCheck() )
			{
				lineColorProperty.colorValue = new Color( lineColor.r,lineColor.g,lineColor.b );
				lineColorChangedProperty.boolValue = true;
			}
		}

		public static void SingleStateLinkField( GUIContent label, SerializedProperty property )
		{
			Rect position = GUILayoutUtility.GetRect( 0.0f,16.0f );

			SingleStateLinkField( position,label,property );
		}

		static bool HasVisibleChildFields(SerializedProperty property)
		{
			switch (property.propertyType)
			{
			case SerializedPropertyType.Vector2:
			case SerializedPropertyType.Vector3:
			case SerializedPropertyType.Rect:
			case SerializedPropertyType.Bounds:
				return false;
			default:
				return property.hasVisibleChildren;
			}
		}
		
		public static void StateLinkField( string label,SerializedProperty property )
		{
			bool isStateLink = property.type == "StateLink";

			if( property.isArray && HasVisibleChildFields(property) )
			{
				for( int index=0;index<property.arraySize;index++ )
				{
					SerializedProperty element = property.GetArrayElementAtIndex(index);
					string elementLabel = label + "["+index+"]";

					if( isStateLink )
					{
						SingleStateLinkField( new GUIContent( elementLabel ),element );
					}
					else
					{
						StateLinkField( elementLabel,element );
					}
				}
			}
			else if( isStateLink )
			{
				SingleStateLinkField( new GUIContent( label ),property );
			}
			else
			{
				SerializedProperty serializedProperty = property.Copy();
				SerializedProperty endProperty = serializedProperty.GetEndProperty();
				
				bool enterChildren = HasVisibleChildFields(serializedProperty);
				while( serializedProperty.NextVisible(enterChildren) && !SerializedProperty.EqualContents(serializedProperty, endProperty) )
				{
					StateLinkField( label+"/" + ObjectNames.NicifyVariableName( serializedProperty.name ),serializedProperty );
					enterChildren = false;
				}
			}
		}

		public static void HelpButton( Rect position,string url,string tooltip )
		{
			GUIContent content = new GUIContent(s_HelpButtonContent);
			content.tooltip = tooltip;

			if( GUI.Button( position,content,s_BehaviourTitlebarText ) )
			{
				Help.BrowseURL( url );
			}
		}

		public static void HelpButton( string url,string tooltip )
		{
			Rect position = GUILayoutUtility.GetRect( 0.0f,20.0f );

			position.x += position.width - 18.0f;
			position.width = 16.0f;

			HelpButton( position,url,tooltip );
		}

		static int s_ButtonMouseDownHash = "ButtonMouseDownHash".GetHashCode();

		public static bool ButtonMouseDown(Rect position, GUIContent content, GUIStyle style)
		{
			Event current = Event.current;

			int controlId = GUIUtility.GetControlID(s_ButtonMouseDownHash, FocusType.Passive, position);

			switch (current.type)
			{
				case EventType.MouseDown:
					if (position.Contains(current.mousePosition) && current.button == 0)
					{
						Event.current.Use();
						return true;
					}
					break;
				case EventType.Repaint:
					style.Draw(position, content, controlId, false);
					break;
			}
			return false;
		}
	}
}