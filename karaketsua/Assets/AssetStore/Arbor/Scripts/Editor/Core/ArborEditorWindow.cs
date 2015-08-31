using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	internal class ArborEditorWindow : EditorWindow
	{
		[MenuItem("Window/Arbor Editor")]
		public static void OpenFromMenu()
		{
			EditorWindow.GetWindow<ArborEditorWindow>("Arbor Editor");
		}

		public static void Open(ArborFSMInternal stateMachine)
		{
			ArborEditorWindow window = EditorWindow.GetWindow<ArborEditorWindow>("Arbor Editor");
			window.Initialize(stateMachine);
		}

		[SerializeField] private ArborFSMInternal _StateMachine;
		[SerializeField] private int _StateMachineInstanceID = 0;

		[SerializeField] private List<int> _Selection = new List<int>();

		private Node[] selection
		{
			get
			{
				List<Node> nodes = new List<Node>();
				
				foreach( int nodeID in _Selection )
				{
					Node node = _StateMachine.GetNodeFromID( nodeID );
					
					if(node != null )
					{
						nodes.Add(node);
					}
				}

				return nodes.ToArray();
			}
		}

		private Dictionary<State,StateEditor> _StateEditors = new Dictionary<State,StateEditor>();
		
		StateEditor GetStateEditor( State state )
		{
			StateEditor stateEditor = null;
			if( !_StateEditors.TryGetValue( state,out stateEditor ) )
			{
				stateEditor = new StateEditor( state );
				
				_StateEditors.Add( state,stateEditor );
			}

			return stateEditor;
		}

		void FinalizeStateEditor()
		{
			if( _StateEditors == null )
			{
				return;
			}
			foreach( StateEditor stateEditor in _StateEditors.Values )
			{
				stateEditor.FinalizeBehaviourEditor();
			}
			_StateEditors.Clear();
		}

		private Dictionary<CommentNode, CommentEditor> _CommentEditors = new Dictionary<CommentNode, CommentEditor>();

		CommentEditor GetCommentEditor(CommentNode comment)
		{
			CommentEditor commentEditor = null;
			if (!_CommentEditors.TryGetValue(comment, out commentEditor))
			{
				commentEditor = new CommentEditor(comment);

				_CommentEditors.Add(comment, commentEditor);
			}

			return commentEditor;
		}

		void FinalizeCommentEditor()
		{
			if (_CommentEditors == null)
			{
				return;
			}
			_CommentEditors.Clear();
		}

		static ArborEditorWindow _CurrentWindow;

		public bool _DragBranchEnable = false;
		public Vector2 _DragBranchStart;
		public Vector2 _DragBranchStartTangent;
		public Vector2 _DragBranchEnd;
		public Vector2 _DragBranchEndTangent;
		public int _DragBranchHoverStateID = 0;

		void OnEnable()
		{
			_CurrentWindow = this;

			EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
		}

		void OnDisable()
		{
			EditorApplication.playmodeStateChanged -= PlaymodeStateChanged;
		}

		void PlaymodeStateChanged()
		{
			Repaint();
		}

		void Initialize(ArborFSMInternal stateMachine)
		{
			Undo.RecordObject( this,"Select StateMachine" );

			_CurrentWindow = this;

			FinalizeStateEditor();

			_StateMachine = stateMachine;

			if( _StateMachine != null )
			{
				_StateMachineInstanceID = _StateMachine.GetInstanceID();
			}
			else
			{
				_StateMachineInstanceID = 0;
			}

			_Selection.Clear();

			Repaint();
		}

		public void DragBranchEnable( bool enable )
		{
			_DragBranchEnable = enable;
		}

		public void DragBranchBezie( Vector2 start,Vector2 startTangent,Vector2 end,Vector2 endTangent )
		{
			_DragBranchStart = start;
			_DragBranchStartTangent = startTangent;
			_DragBranchEnd = end;
			_DragBranchEndTangent = endTangent;
		}

		public void DragBranchHoverStateID( int stateID )
		{
			if( _DragBranchHoverStateID != stateID )
			{
				_DragBranchHoverStateID = stateID;
			}
		}

		public static ArborEditorWindow GetCurrent()
		{
			return _CurrentWindow;
		}

		bool MissingStateBehaviourGUI( State state,SerializedObject serializedObject )
		{
			serializedObject.Update();
			SerializedProperty property = serializedObject.FindProperty("m_Script");
			if( property == null )
			{
				return false;
			}

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField( property );

			MonoScript monoScript = property.objectReferenceValue as MonoScript;

			bool valid = ( monoScript != null && monoScript.GetClass() != null && monoScript.GetClass().IsSubclassOf( typeof(StateBehaviour) ) );

			if( !valid )
			{
				EditorGUILayout.HelpBox( "The associated script can not be loaded.\nPlease fix any compile errors\nand assign a valid script.", MessageType.Warning, true);
			}

			if( serializedObject.ApplyModifiedProperties() )
			{
				state.ForceRebuild( EditorUtility.InstanceIDToObject );
				FinalizeStateEditor();
			}

			return true;
		}

		bool IsMissingStateBehaviourTarget( Object target )
		{
			if (target.GetType() != typeof (MonoBehaviour))
				return target.GetType() == typeof (ScriptableObject);
			else
				return true;
		}

		void SelectAll()
		{
			Undo.IncrementCurrentGroup();
			
			Undo.RecordObject( this,"Selection State" );

			_Selection.Clear ();

			foreach ( State state in _StateMachine.states )
			{
				_Selection.Add( state.stateID );
			}
			foreach (CommentNode comment in _StateMachine.comments)
			{
				_Selection.Add(comment.commentID);
			}

			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
		}

		void SelectNode(Node node)
		{
			Event current = Event.current;
			if( current.type != EventType.MouseDown || current.button != 0 )
			{
				return;
			}

			int nodeID = 0;

			if (node is State)
			{
				State state = node as State;
				nodeID = state.stateID;
			}
			else if (node is CommentNode)
			{
				CommentNode comment = node as CommentNode;
				nodeID = comment.commentID;
			}

			if ( EditorGUI.actionKey || current.shift )
			{
				Undo.IncrementCurrentGroup();

				Undo.RecordObject( this,"Selection Node" );

				if (_Selection.Contains(nodeID))
				{
					_Selection.Remove(nodeID);
				}
				else
				{
					_Selection.Add(nodeID);
				}

				Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );

				current.Use();
			}
			else
			{
				if( !_Selection.Contains(nodeID) )
				{
					Undo.IncrementCurrentGroup();

					Undo.RecordObject( this,"Selection Node" );

					_Selection.Clear();
					_Selection.Add(nodeID);

					Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
				}
				HandleUtility.Repaint();
			}
			GUIUtility.keyboardControl = 0;
		}

		private static readonly int _DragSelectionControlID = "DragSelection".GetHashCode();
		private static readonly int _DragStatesControlID = "DragStates".GetHashCode();

		private Vector2 _DragBeginPos;
		private List<int> _OldSelection;
		public enum SelectionMode
		{
			None,
			Pick,
			Rect,
		};
		private SelectionMode _SelectionMode = SelectionMode.None;
		private bool _IsDragSelection = false;

		private Dictionary<Node,Rect> _DragNodePositions = new Dictionary<Node, Rect>();
		private Vector2 _LastMousePosition;
		private Vector2 _DragStateDistance;
		private bool _IsDragStates = false;

		static Rect FromToRect( Vector2 start, Vector2 end )
		{
			Rect rect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
			if( rect.width < 0.0f )
			{
				rect.x += rect.width;
				rect.width = -rect.width;
			}
			if( rect.height < 0.0f )
			{
				rect.y += rect.height;
				rect.height = -rect.height;
			}
			return rect;
		}

		void SelectNodesInRect( Rect rect )
		{
			Undo.RecordObject( this,"Selection State" );

			_Selection.Clear();
			foreach( State state in _StateMachine.states )
			{
				if( state.position.xMax >= rect.x && state.position.x <= rect.xMax && 
				   state.position.yMax >= rect.y && state.position.y <= rect.yMax )
				{
					_Selection.Add( state.stateID );
				}
			}
			foreach (CommentNode comment in _StateMachine.comments)
			{
				if (comment.position.xMax >= rect.x && comment.position.x <= rect.xMax &&
				   comment.position.yMax >= rect.y && comment.position.y <= rect.yMax)
				{
					_Selection.Add(comment.commentID);
				}
			}

			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
		}

		Rect SnapPositionToGrid(Rect position)
		{
			if( ArborSettings.showGrid && ArborSettings.snapGrid )
			{
				float gridSizeMinor = ArborSettings.gridSize/(float)ArborSettings.gridSplitNum;
				int num1 = Mathf.RoundToInt(position.x / gridSizeMinor);
				int num2 = Mathf.RoundToInt(position.y / gridSizeMinor);
				position.x = (float) num1 * gridSizeMinor;
				position.y = (float) num2 * gridSizeMinor;
			}
			return position;
		}
		
		void DragNodes()
		{
			Event current = Event.current;
			int controlId = GUIUtility.GetControlID(_DragStatesControlID, FocusType.Passive);

			switch( current.GetTypeForControl(controlId) )
			{
			case EventType.MouseDown:
				if( current.button == 0 )
				{
					Undo.IncrementCurrentGroup();

					Undo.RecordObject( _StateMachine,"Move State" );

					_LastMousePosition = EditorGUIUtility.GUIToScreenPoint(current.mousePosition);
					_DragStateDistance = Vector2.zero;
					foreach( Node node in selection )
					{
						_DragNodePositions[node] = node.position;
					}
					GUIUtility.hotControl = controlId;
					current.Use();
				}
				break;
			case EventType.MouseUp:
				if( GUIUtility.hotControl == controlId )
				{
					_DragNodePositions.Clear();
					GUIUtility.hotControl = 0;
					current.Use();
				}
				break;
			case EventType.MouseDrag:
				if( GUIUtility.hotControl == controlId )
				{
					_DragStateDistance += EditorGUIUtility.GUIToScreenPoint(current.mousePosition) - _LastMousePosition;
					_LastMousePosition = EditorGUIUtility.GUIToScreenPoint(current.mousePosition);
					foreach( Node node in selection )
					{
						Rect position = node.position;
						Rect rect = _DragNodePositions[node];
						position.x = rect.x + _DragStateDistance.x;
						position.y = rect.y + _DragStateDistance.y;

						position = SnapPositionToGrid( position );

						if( (position.x != node.position.x || position.y != node.position.y ) )
						{
							Undo.RecordObject( _StateMachine,"Move Node" );

							node.position = position;

							Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );

							EditorUtility.SetDirty( _StateMachine );
						}
					}

					current.Use();
				}
				break;
			case EventType.KeyDown:
				if( GUIUtility.hotControl == controlId && current.keyCode == KeyCode.Escape )
				{
					foreach( Node node in selection )
					{
						Rect position = _DragNodePositions[node];

						position = SnapPositionToGrid( position );

						if( (position.x != node.position.x || position.y != node.position.y ) )
						{
							Undo.RecordObject( _StateMachine,"Move Node" );

							node.position = position;

							Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
							
							EditorUtility.SetDirty( _StateMachine );
						}
					}
					GUIUtility.hotControl = 0;
					current.Use();

					Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
				}
				break;
			}

			_IsDragStates = GUIUtility.hotControl == controlId;
		}

		bool IsContainsState( Vector2 position )
		{
			foreach( State state in _StateMachine.states )
			{
				if( state.position.Contains( position ) )
				{
					return true;
				}
			}
			return false;
		}

		void DragSelection()
		{
			int controlId = GUIUtility.GetControlID(_DragSelectionControlID, FocusType.Passive);
			Event current = Event.current;

			switch( current.GetTypeForControl(controlId) )
			{
			case EventType.MouseDown:
				if( !IsContainsState(current.mousePosition) && current.button == 0 && !(current.clickCount == 2 || current.alt) )
				{
					Undo.IncrementCurrentGroup();
					
					Undo.RecordObject( this,"Selection State" );

					GUIUtility.hotControl = GUIUtility.keyboardControl = controlId;
					_DragBeginPos = current.mousePosition;
					_OldSelection = new List<int>( _Selection );
					if( !EditorGUI.actionKey && !current.shift )
					{
						_Selection.Clear();
					}
					_SelectionMode = SelectionMode.Pick;
					current.Use();
				}
				break;
			case EventType.MouseUp:
				if( GUIUtility.hotControl == controlId )
				{
					GUIUtility.hotControl = GUIUtility.keyboardControl = 0;
					_OldSelection.Clear();
					_SelectionMode = SelectionMode.None;
					current.Use();
				}
				break;
			case EventType.MouseDrag:
				if( GUIUtility.hotControl == controlId )
				{
					_SelectionMode = SelectionMode.Rect;
					SelectNodesInRect( FromToRect(_DragBeginPos, current.mousePosition) );
					current.Use();
				}
				break;
			case EventType.KeyDown:
				if( _SelectionMode != SelectionMode.None && current.keyCode == KeyCode.Escape )
				{
					Undo.RecordObject( this,"Selection State" );

					_Selection = _OldSelection;
					GUIUtility.hotControl = GUIUtility.keyboardControl = 0;

					Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );

					current.Use();
				}
				break;
			case EventType.Repaint:
				if( _SelectionMode == SelectionMode.Rect )
				{
					UnityEditor.Graphs.Styles.selectionRect.Draw( FromToRect(_DragBeginPos, current.mousePosition), false, false, false, false );
				}
				break;
			}

			_IsDragSelection = GUIUtility.hotControl == controlId;
		}
		
		void OnStateGUI( StateEditor stateEditor )
		{
			State state = stateEditor.state;
			SelectNode( state );

			EditorGUITools.StateTitlebar( state );

			foreach( StateBehaviour behaviour in state.behaviours )
			{
				//if( behaviour != null )
				{
					Editor editor = stateEditor.GetBehaviourEditor( behaviour );
					if( editor!=null )
					{
						bool expanded = EditorGUITools.BehaviourTitlebar( behaviour.expanded,behaviour );
						if( behaviour.expanded != expanded )
						{
							behaviour.expanded = expanded;

							EditorUtility.SetDirty( behaviour );
						}

						bool missing = IsMissingStateBehaviourTarget( behaviour );

						if( expanded )
						{
							if( missing )
							{
								missing = MissingStateBehaviourGUI( state,editor.serializedObject );
							}
							if( !missing )
							{
								GUIStyle marginStyle = (editor.UseDefaultMargins()) ? EditorStyles.inspectorDefaultMargins : EditorStyles.inspectorFullWidthMargins;
                                EditorGUILayout.BeginVertical(marginStyle);
								editor.OnInspectorGUI();
								EditorGUILayout.EndVertical();
							}
						}

						if( !missing )
						{
							editor.serializedObject.Update();
							
							SerializedProperty iterator = editor.serializedObject.GetIterator();

							for( bool enterChildren=true;iterator.NextVisible(enterChildren);enterChildren=false )
							{
								EditorGUITools.StateLinkField( ObjectNames.NicifyVariableName( iterator.name ),iterator );
							}
							
							editor.serializedObject.ApplyModifiedProperties();
						}
					}
				}
			}

			Event current = Event.current;
			
			switch( current.type )
			{
			case EventType.DragUpdated:
			case EventType.DragPerform:
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				
				if( current.type == EventType.DragPerform )
				{
					bool findBehaviour = false;
					
					foreach( Object draggedObject in DragAndDrop.objectReferences )
					{
						MonoScript script = draggedObject as MonoScript;
						if( script != null )
						{
							System.Type classType = script.GetClass();
							
							if( classType.IsSubclassOf(typeof(StateBehaviour)) )
							{
								if( !findBehaviour )
								{
									Undo.RecordObject( state.stateMachine,"Add State Behaviour" );
									
									state.AddBehaviour( classType );
									findBehaviour = true;
									
									EditorUtility.SetDirty( state.stateMachine );
									
									DragAndDrop.AcceptDrag();
									DragAndDrop.activeControlID = 0;
									
									current.Use();
								}
							}
							else
							{
								Debug.LogError ( classType.Name + " is not support State Script." );
							}
						}
					}
				}
				break;
			case EventType.Repaint:
				Rect lastRect = GUILayoutUtility.GetLastRect();
				
				float height = lastRect.y + lastRect.height;
				
				if( height != state.position.height )
				{			
					state.position.height = height;
					
					EditorUtility.SetDirty( state.stateMachine );
					
					Repaint();
				}
				break;
			}

			DragNodes();
			//GUI.DragWindow();
		}

		void OnCommentGUI(CommentEditor commentEditor)
		{
			CommentNode comment = commentEditor.comment;
			SelectNode(comment);

			EditorGUITools.CommentField(comment);

			Event current = Event.current;

			switch (current.type)
			{
				case EventType.Repaint:
					Rect lastRect = GUILayoutUtility.GetLastRect();

					float height = lastRect.y + lastRect.height;

					if (height != comment.position.height)
					{
						comment.position.height = height;

						EditorUtility.SetDirty(comment.stateMachine);

						Repaint();
					}
					break;
			}

			DragNodes();
			//GUI.DragWindow();
		}

		void CreateState( Vector2 position,bool resident )
		{
			Undo.IncrementCurrentGroup();
			
			Undo.RecordObject( _StateMachine,"Created State" );
			
			State state = _StateMachine.CreateState( resident );
			
			if( state != null )
			{
				state.position = new Rect( position.x,position.y,300,100 );
			}
			
			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
			
			if( state != null )
			{
				EditorUtility.SetDirty( _StateMachine );
			}
		}

		void CreateComment(Vector2 position)
		{
			Undo.IncrementCurrentGroup();

			Undo.RecordObject(_StateMachine, "Created State");

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			CommentNode comment = _StateMachine.CreateComment();

			if (comment != null)
			{
				comment.position = new Rect(position.x, position.y, 300, 100);
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			if (comment != null)
			{
				EditorUtility.SetDirty(_StateMachine);
			}
		}
		
		void CreateState( object obj )
		{
			Vector2 position = (Vector2)obj;

			CreateState( position,false );
		}

		void CreateResidentState( object obj )
		{
			Vector2 position = (Vector2)obj;

			CreateState( position,true );
		}

		void CreateComment(object obj)
        {
			Vector2 position = (Vector2)obj;

			CreateComment(position);
		}

		void CopyNodes()
		{
			EditorGUITools.CopyNodes( selection );
		}

		void DuplicateNodes( object obj )
		{
			Vector2 position = (Vector2)obj;
			
			Undo.IncrementCurrentGroup();

			Undo.RecordObjects(new Object[] { _StateMachine, this }, "Duplicate Nodes");
			
			Node[] nodes = EditorGUITools.DuplicateNodes( _StateMachine,position,selection );
			
			if(nodes != null )
			{
				_Selection.Clear();
				
				EditorUtility.SetDirty( _StateMachine );
				
				foreach( Node node in nodes )
				{
					if (node is State)
					{
						State state = node as State;
						_Selection.Add(state.stateID);

						foreach (StateBehaviour behaviour in state.behaviours)
						{
							EditorUtility.SetDirty(behaviour);
						}
					}
					else if (node is CommentNode)
					{
						CommentNode comment = node as CommentNode;
						_Selection.Add(comment.commentID);
					}
				}
				
				EditorUtility.SetDirty( this );
			}
			
			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
		}

		void PasteNodes( object obj )
		{
			Vector2 position = (Vector2)obj;

			Undo.IncrementCurrentGroup();

			Undo.RecordObjects(new Object[] { _StateMachine, this }, "Paste Nodes");
			
			Node[] nodes = EditorGUITools.PasteNodes( _StateMachine,position );

			if(nodes != null )
			{
				_Selection.Clear();

				EditorUtility.SetDirty( _StateMachine );

				foreach( Node node in nodes)
				{
					if (node is State)
					{
						State state = node as State;
						_Selection.Add(state.stateID);

						foreach (StateBehaviour behaviour in state.behaviours)
						{
							EditorUtility.SetDirty(behaviour);
						}
					}
					else if (node is CommentNode)
					{
						CommentNode comment = node as CommentNode;
						_Selection.Add(comment.commentID);
					}
				}

				EditorUtility.SetDirty( this );
			}

			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
		}

		void DeleteNodes()
		{
			Undo.IncrementCurrentGroup();

			List<Object> objects = new List<Object>();
			objects.Add( _StateMachine );

			foreach( State otherState in _StateMachine.states )
			{
				if( !_Selection.Contains( otherState.stateID ) )
				{
					foreach( StateBehaviour behaviour in otherState.behaviours )
					{
						objects.Add( behaviour );
					}
				}
			}

			objects.Add( this );
			
			Undo.RecordObjects( objects.ToArray(),"Delete Nodes" );

			foreach( Node deleteNode in selection )
			{
				_StateMachine.DeleteNode(deleteNode);
			}

			_Selection.Clear();
			
			Undo.CollapseUndoOperations( Undo.GetCurrentGroup() );
			
			foreach( Object target in objects )
			{
				EditorUtility.SetDirty( target );
			}
		}

		private static readonly int s_DragScrollHash = "DragScroll".GetHashCode();
		private Vector2 _ScrollPos = Vector2.zero;
		private bool _ScrollDragging = false;
		private int _CachedHotControl=0;

		static GUIStyle GetStateStyle( UnityEditor.Graphs.Styles.Color color, bool on)
		{
			return UnityEditor.Graphs.Styles.GetNodeStyle( "node",color,on );
		}

		void HandleContextMenu()
		{
			Event current = Event.current;

			if( !_DragBranchEnable && current.type == EventType.ContextClick )
			{
				GenericMenu menu = new GenericMenu();
				
				menu.AddItem( EditorGUITools.GetTextContent( "Create State"),false,CreateState,current.mousePosition );
				menu.AddItem( EditorGUITools.GetTextContent( "Create Resident State"),false,CreateResidentState,current.mousePosition );

				menu.AddSeparator("");

				menu.AddItem(EditorGUITools.GetTextContent("Create Comment"), false, CreateComment, current.mousePosition);

				menu.AddSeparator("");

				if( _Selection.Count > 0 )
				{
					menu.AddItem( EditorGUITools.GetTextContent("Copy"),false,CopyNodes);
				}
				else
				{
					menu.AddDisabledItem( EditorGUITools.GetTextContent("Copy") );
				}
				
				if( EditorGUITools.isCopyedNode )
				{
					menu.AddItem( EditorGUITools.GetTextContent("Paste"),false,PasteNodes,current.mousePosition );
				}
				else
				{
					menu.AddDisabledItem( EditorGUITools.GetTextContent("Paste") );
				}

				menu.AddSeparator("");

				if( _Selection.Count > 0 )
				{
					menu.AddItem( EditorGUITools.GetTextContent("Duplicate"),false,DuplicateNodes,current.mousePosition );
					menu.AddItem( EditorGUITools.GetTextContent("Delete"),false,DeleteNodes );
				}
				else
				{
					menu.AddDisabledItem( EditorGUITools.GetTextContent("Duplicate") );
					menu.AddDisabledItem( EditorGUITools.GetTextContent("Delete") );
				}
				
				menu.ShowAsContext();
				
				current.Use ();
			}
		}

		bool _IsFrameSelected = false;
		Vector2 _FrameSelectTarget = Vector2.zero;

		void FrameSelected()
		{
			_FrameSelectTarget = Vector2.zero;
			foreach( Node node in selection )
			{
				_FrameSelectTarget += node.position.center;
			}
			_FrameSelectTarget /= (float)selection.Length;

			_FrameSelectTarget.x -= _GraphArea.width * 0.5f;
			_FrameSelectTarget.y -= _GraphArea.height * 0.5f;

			_FrameSelectTarget.x -= _GraphExtents.x;
			_FrameSelectTarget.y -= _GraphExtents.y;

			_IsFrameSelected = true;

			Repaint();
		}

		void HandleCommand( Vector2 position )
		{
			Event current = Event.current;

			EventType eventType = current.type;

			switch( eventType )
			{
			case EventType.ValidateCommand:
				switch( current.commandName )
				{
				case "Copy":
				case "Duplicate":
				case "Delete":
				case "SoftDelete":
				case "FrameSelected":
					if( _Selection.Count > 0 )
					{
						current.Use();
					}
					break;
				case "Paste":
					if( EditorGUITools.isCopyedNode )
					{
						current.Use();
					}
					break;
				case "SelectAll":
					if( _StateMachine.states.Length > 0 )
					{
						current.Use();
					}
					break;
				}
				break;
			case EventType.ExecuteCommand:
				switch( current.commandName )
				{
				case "Copy":
					CopyNodes();
					break;
				case "Paste":
					PasteNodes( position );
					break;
				case "Duplicate":
					DuplicateNodes( position );
					break;
				case "FrameSelected":
					FrameSelected();
					break;
				case "Delete":
				case "SoftDelete":
					DeleteNodes();
					break;
				case "SelectAll":
					SelectAll();
					break;
				}
				break;
			}
		}

		void DrawToolbar()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar );
			
			if( _StateMachine == null && _StateMachineInstanceID != 0 )
			{
				_StateMachine = EditorUtility.InstanceIDToObject( _StateMachineInstanceID ) as ArborFSMInternal;
				_Selection.Clear();
			}
			
			EditorGUI.BeginChangeCheck();
			
			ArborFSMInternal stateMachine = EditorGUILayout.ObjectField( _StateMachine,typeof(ArborFSMInternal),true,GUILayout.Width ( 200 ) ) as ArborFSMInternal;
			
			if( EditorGUI.EndChangeCheck() )
			{
				Initialize( stateMachine );
			}

			Rect stateListButtonPosition = GUILayoutUtility.GetRect(100.0f, 20.0f);

			ArborSettings.openStateList = GUI.Toggle(stateListButtonPosition, ArborSettings.openStateList, Localization.GetWord("State List"), EditorStyles.toolbarButton);


			GUILayout.FlexibleSpace();

			List<string> languageLabel = new List<string>();
			languageLabel.Add(Localization.GetWord("Auto") + "(" + Localization.GetWord(ArborSettings.GetAutoLanguage().ToString()) + ")");
			int selectIndex = 0;
			SystemLanguage[] languages = Localization.GetLanguages();
			if (languages != null)
			{
				for (int i = 0; i < languages.Length; i++)
				{
					SystemLanguage language = languages[i];
					languageLabel.Add(Localization.GetWord(language.ToString()));
					if (!ArborSettings.autoLanguage && language == ArborSettings.language)
					{
						selectIndex = i + 1;
					}
				}
			}

			EditorGUI.BeginChangeCheck();
			selectIndex = EditorGUILayout.Popup(selectIndex, languageLabel.ToArray(), EditorStyles.toolbarPopup,GUILayout.Width(100.0f) );
			if (EditorGUI.EndChangeCheck())
			{
				if (selectIndex == 0)
				{
					ArborSettings.autoLanguage = true;
				}
				else
				{
					ArborSettings.autoLanguage = false;
					ArborSettings.language = languages[selectIndex - 1];
				}
			}

			Rect gridButtonPosition = GUILayoutUtility.GetRect(80.0f, 20.0f);
			if (GUI.Button(gridButtonPosition, Localization.GetWord("Grid"), EditorStyles.toolbarDropDown))
			{
				GridSettingsWindow.instance.Init(gridButtonPosition);
			}

			Rect helpButtonPosition = GUILayoutUtility.GetRect(22.0f, 20.0f);
			helpButtonPosition.x += 5.0f;
			helpButtonPosition.width -= 10.0f;

            helpButtonPosition.y += 1.0f;
			helpButtonPosition.height -= 2.0f;

			string siteURL = Localization.GetWord("SiteURL");
			EditorGUITools.HelpButton( helpButtonPosition, siteURL, "Open Reference" );
			
			EditorGUILayout.EndHorizontal();
		}

		private static int s_DrawBranchHash = "s_DrawBranchHash".GetHashCode();

		void DrawBranchStateLink(StateBehaviour behaviour, SerializedProperty property)
		{
			bool lineEnable = property.FindPropertyRelative("lineEnable").boolValue;
			int stateID = property.FindPropertyRelative("stateID").intValue;

			Vector2 lineStart = property.FindPropertyRelative("lineStart").vector2Value;
			Vector2 lineStartTangent = property.FindPropertyRelative("lineStartTangent").vector2Value;
			Vector2 lineEnd = property.FindPropertyRelative("lineEnd").vector2Value;
			Vector2 lineEndTangent = property.FindPropertyRelative("lineEndTangent").vector2Value;
			bool lineColorChanged = property.FindPropertyRelative("lineColorChanged").boolValue;
			Color lineColor = Color.white;
			if (lineColorChanged)
			{
				lineColor = property.FindPropertyRelative("lineColor").colorValue;
			}

			if (lineEnable && stateID != 0)
			{
				Vector2 shadowPos = Vector2.one * 3;

				EditorGUITools.BezierArrow(lineStart + shadowPos, lineStartTangent + shadowPos, lineEnd + shadowPos, lineEndTangent + shadowPos, new Color(0, 0, 0, 1.0f), 5.0f, 16.0f);
				EditorGUITools.BezierArrow(lineStart, lineStartTangent, lineEnd, lineEndTangent, lineColor, 5.0f, 16.0f);

				int controlID = EditorGUIUtility.GetControlID(s_DrawBranchHash, EditorGUIUtility.native);

				Event currentEvent = Event.current;

				EventType eventType = currentEvent.GetTypeForControl(controlID);

				float distance = HandleUtility.DistancePointBezier(currentEvent.mousePosition, lineStart, lineEnd, lineStartTangent, lineEndTangent);

				switch (eventType)
				{
					case EventType.MouseDown:
						if (distance <= 15.0f)
						{
							if (currentEvent.button == 1)
							{
								State prevState = behaviour.state;
								State nextState = behaviour.stateMachine.GetStateFromID(stateID);

                                GenericMenu menu = new GenericMenu();
								menu.AddItem(new GUIContent(Localization.GetWord("Go to Previous State") + " : " + prevState.name), false, () => {
                                    _Selection.Clear();
									_Selection.Add(prevState.stateID);
									_FrameSelected = true;
								});
								menu.AddItem(new GUIContent(Localization.GetWord("Go to Next State") + " : " + nextState.name), false, () => {
									_Selection.Clear();
									_Selection.Add(nextState.stateID);
									_FrameSelected = true;
								});

								menu.ShowAsContext();
								currentEvent.Use();
							}
						}
						break;
				}
			}
		}

		public void DrawBehaviourBranches(StateBehaviour behaviour, SerializedProperty property)
		{
			if (property.type == "StateLink")
			{
				if (property.isArray)
				{
					for (int i = 0; i < property.arraySize; i++)
					{
						SerializedProperty stateLinkProperty = property.GetArrayElementAtIndex(i);

						DrawBranchStateLink(behaviour, stateLinkProperty);
					}
				}
				else
				{
					DrawBranchStateLink(behaviour, property);
				}
			}
		}

		public void DrawBehaviourBranches(StateEditor stateEditor)
		{
			foreach (StateBehaviour behaviour in stateEditor.state.behaviours)
			{
				if (behaviour != null)
				{
					Editor editor = stateEditor.GetBehaviourEditor(behaviour);

					editor.serializedObject.Update();

					SerializedProperty iterator = editor.serializedObject.GetIterator();
					while (iterator.NextVisible(true))
					{
						DrawBehaviourBranches(behaviour, iterator);
					}

					editor.serializedObject.ApplyModifiedProperties();
				}
			}
		}

		public class StateEditor
		{
			private State _State;
			public State state
			{
				get
				{
					return _State;
				}
			}

			public StateEditor( State state )
			{
				_State = state;
			}

			private Dictionary<StateBehaviour,Editor> _BehaviourEditors = new Dictionary<StateBehaviour, Editor>();

			public Editor GetBehaviourEditor( StateBehaviour behaviour )
			{
				Editor editor = null;
				if( !_BehaviourEditors.TryGetValue( behaviour,out editor ) )
				{
					editor = Editor.CreateEditor( behaviour );
					
					_BehaviourEditors.Add( behaviour,editor );
				}
				
				return editor;
			}

			public void FinalizeBehaviourEditor()
			{
				if( _BehaviourEditors == null )
				{
					return;
				}
				foreach( Editor editor in _BehaviourEditors.Values )
				{
					Object.DestroyImmediate( editor );
				}
				_BehaviourEditors.Clear();
			}
		}

		public class CommentEditor
		{
			private CommentNode _Comment;
			public CommentNode comment
			{
				get
				{
					return _Comment;
				}
			}

			public CommentEditor(CommentNode comment)
			{
				_Comment = comment;
			}
		}

		Vector2 _StateListScrollPos = Vector2.zero;

		enum SearchMode
		{
			All,
			Name,
			Type
		};

		SearchMode _SearchMode = SearchMode.All;
		string _SearchText;

		bool _FrameSelected = false;

		private class Styles
		{
			public static GUIStyle background;
			
			static Styles()
			{
				background = (GUIStyle)"grey_border";
			}
		}

		void DrawStateList()
		{
			EditorGUILayout.BeginVertical(Styles.background,GUILayout.Width(200.0f));

			State[] states = null;

			if (_StateMachine != null)
			{
				states = _StateMachine.states;
			}
			else
			{
				GUI.enabled = false;
			}

			Rect searchRect = GUILayoutUtility.GetRect(10.0f, 20.0f);
			searchRect.y += 4f;
            searchRect.x += 8f;
			searchRect.width -= 16f;

			string[] names = System.Enum.GetNames(typeof(SearchMode));
			int searchMode = (int)_SearchMode;
			_SearchText = EditorGUITools.ToolbarSearchField(searchRect, names, ref searchMode, _SearchText);
			_SearchMode = (SearchMode)searchMode;

			if (states != null)
			{
				_StateListScrollPos = EditorGUILayout.BeginScrollView(_StateListScrollPos);

				GUILayout.Space(3.0f);

				List<State> viewStates = new List<State>();
				if (!string.IsNullOrEmpty(_SearchText))
				{
					foreach (State state in states)
					{
						switch (_SearchMode)
						{
							case SearchMode.All:
								if (state.name.IndexOf(_SearchText, System.StringComparison.OrdinalIgnoreCase) >= 0)
								{
									viewStates.Add(state);
								}
								else
								{
									foreach (StateBehaviour behaviour in state.behaviours)
									{
										if (behaviour.GetType().Name.Equals(_SearchText, System.StringComparison.OrdinalIgnoreCase) )
										{
											viewStates.Add(state);
											break;
										}
									}
								}
								break;
							case SearchMode.Name:
								if (state.name.IndexOf(_SearchText, System.StringComparison.OrdinalIgnoreCase) >= 0)
								{
									viewStates.Add(state);
								}
								break;
							case SearchMode.Type:
								foreach (StateBehaviour behaviour in state.behaviours)
								{
									if (behaviour.GetType().Name.Equals(_SearchText, System.StringComparison.OrdinalIgnoreCase) )
									{
										viewStates.Add(state);
										break;
									}
								}
								break;
						}
					}
				}
				else
				{
					viewStates.AddRange(states);
				}

				viewStates.Sort((a, b) => {
					return a.name.CompareTo(b.name);
				});

				foreach (State state in viewStates)
				{
					UnityEditor.Graphs.Styles.Color color = UnityEditor.Graphs.Styles.Color.Gray;

					if (_StateMachine.currentState == state)
					{
						color = UnityEditor.Graphs.Styles.Color.Orange;
					}
					else if (_StateMachine.startStateID == state.stateID)
					{
						color = UnityEditor.Graphs.Styles.Color.Aqua;
					}
					else if (state.resident)
					{
						color = UnityEditor.Graphs.Styles.Color.Green;
					}

					bool on = _Selection.Contains(state.stateID);
					GUIStyle nodeStyle = GetStateStyle(color, on);
					
					EditorGUILayout.BeginHorizontal();

					GUILayout.FlexibleSpace();

					Rect rect = GUILayoutUtility.GetRect(100.0f, 190.0f,30.0f,30.0f);

					if ( EditorGUITools.ButtonMouseDown(rect,new GUIContent(state.name), nodeStyle) )
					{
						_Selection.Clear();
						_Selection.Add(state.stateID);
						_FrameSelected = true;
                    }

					GUILayout.FlexibleSpace();

					EditorGUILayout.EndHorizontal();

					GUILayout.Space(5);
				}

				EditorGUILayout.EndScrollView();
			}

			if (_StateMachine == null)
			{
				GUI.enabled = true;
			}

			EditorGUILayout.EndVertical();
		}

		Rect _GraphArea;
		Rect _GraphViewArea;
		Rect _GraphExtents;
		Rect _LastGraphExtents;

		void UpdateGraphExtents()
		{
			if (_StateMachine.states.Length > 0 || _StateMachine.comments.Length > 0)
			{
				Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
				Vector2 max = new Vector2(float.MinValue, float.MinValue);

				foreach (State state in _StateMachine.states)
				{
					min.x = Mathf.Min(min.x, state.position.xMin);
					min.y = Mathf.Min(min.y, state.position.yMin);

					max.x = Mathf.Max(max.x, state.position.xMax);
					max.y = Mathf.Max(max.y, state.position.yMax);
				}

				foreach (CommentNode comment in _StateMachine.comments)
				{
					min.x = Mathf.Min(min.x, comment.position.xMin);
					min.y = Mathf.Min(min.y, comment.position.yMin);

					max.x = Mathf.Max(max.x, comment.position.xMax);
					max.y = Mathf.Max(max.y, comment.position.yMax);
				}

				_GraphExtents = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
			}
			else
			{
				_GraphExtents = new Rect();
			}

			_GraphExtents.xMin -= _GraphArea.width * 0.6f;
			_GraphExtents.xMax += _GraphArea.width * 0.6f;
			_GraphExtents.yMin -= _GraphArea.height * 0.6f;
			_GraphExtents.yMax += _GraphArea.height * 0.6f;
		}

		void UpdateScrollPosition()
		{
			_ScrollPos.x += _LastGraphExtents.x - _GraphExtents.x;
			_ScrollPos.y += _LastGraphExtents.y - _GraphExtents.y;

			_LastGraphExtents = _GraphExtents;
		}

		void UpdateGraphViewArea()
		{
			Vector2 scrollPos = _ScrollPos + new Vector2(_GraphExtents.x, _GraphExtents.y);

			_GraphViewArea = new Rect(scrollPos.x, scrollPos.y, _GraphArea.width, _GraphArea.height);
		}

		private void DrawGrid()
		{
			if (Event.current.type != EventType.Repaint)
				return;
			Profiler.BeginSample("DrawGrid");
			EditorGUITools.handleWireMaterial.SetPass(0);
			GL.PushMatrix();
			GL.Begin(1);

			if (ArborSettings.gridSplitNum > 1)
			{
				float gridSize = ArborSettings.gridSize / (float)ArborSettings.gridSplitNum;
				DrawGridLines(gridSize, EditorGUITools.gridMinorColor);
			}
			DrawGridLines(ArborSettings.gridSize, EditorGUITools.gridMajorColor);

			GL.End();
			GL.PopMatrix();
			Profiler.EndSample();
		}

		private void DrawGridLines(float gridSize, UnityEngine.Color gridColor)
		{
			GL.Color(gridColor);
			float x = _GraphViewArea.xMin - _GraphViewArea.xMin % gridSize;
			while (x < _GraphViewArea.xMax)
			{
				DrawLine(new Vector2(x, _GraphViewArea.yMin), new Vector2(x, _GraphViewArea.yMax));
				x += gridSize;
			}
			GL.Color(gridColor);
			float y = _GraphViewArea.yMin - _GraphViewArea.yMin % gridSize;
			while (y < _GraphViewArea.yMax)
			{
				DrawLine(new Vector2(_GraphViewArea.xMin, y), new Vector2(_GraphViewArea.xMax, y));
				y += gridSize;
			}
		}

		private void DrawLine(Vector2 p1, Vector2 p2)
		{
			GL.Vertex((Vector3)p1);
			GL.Vertex((Vector3)p2);
		}

		void DragGrid()
		{
			int controlID = GUIUtility.GetControlID(s_DragScrollHash, FocusType.Passive);

			Event current = Event.current;

			bool dragButton = current.button == 2 || current.alt;

			if (current.alt || _ScrollDragging)
			{
				EditorGUIUtility.AddCursorRect(_GraphArea, MouseCursor.Pan, controlID);
			}

			if (_DragBranchEnable || _IsDragSelection || _IsDragStates)
			{
				Vector2 offset = Vector2.zero;

				Vector2 mousePosition = current.mousePosition;

				if (mousePosition.x < _GraphViewArea.xMin)
				{
					offset.x = mousePosition.x - _GraphViewArea.xMin;
				}
				else if (_GraphViewArea.xMax < mousePosition.x)
				{
					offset.x = mousePosition.x - _GraphViewArea.xMax;
				}

				if (mousePosition.y < _GraphViewArea.yMin)
				{
					offset.y = mousePosition.y - _GraphViewArea.yMin;
				}
				else if (_GraphViewArea.yMax < mousePosition.y)
				{
					offset.y = mousePosition.y - _GraphViewArea.yMax;
				}

				if (offset.sqrMagnitude > 0.0f)
				{
					_IsFrameSelected = false;
				}

				_ScrollPos += offset;
			}

			switch (current.GetTypeForControl(controlID))
			{
				case EventType.MouseDown:
					if (_GraphViewArea.Contains(current.mousePosition) && dragButton )
					{
						_CachedHotControl = GUIUtility.hotControl;
						GUIUtility.hotControl = controlID;
						current.Use();
						EditorGUIUtility.SetWantsMouseJumping(1);

						_ScrollDragging = true;
					}
					break;
				case EventType.MouseUp:
					if (GUIUtility.hotControl == controlID)
					{
						GUIUtility.hotControl = _CachedHotControl;
						current.Use();
						EditorGUIUtility.SetWantsMouseJumping(0);

						_ScrollDragging = false;
					}
					break;
				case EventType.MouseMove:
				case EventType.MouseDrag:
					if (GUIUtility.hotControl == controlID)
					{
						_ScrollPos -= current.delta;

						_IsFrameSelected = false;

						current.Use();
					}
					break;
			}
		}

		void BeginGraphGUI(Rect position)
		{
			_GraphArea = position;
			
			EditorGUITools.DrawGridBackground(_GraphArea);

			if (_IsFrameSelected)
			{
				Vector2 dir = _FrameSelectTarget - _ScrollPos;
				_ScrollPos += dir * 0.1f;
				if (dir.magnitude <= 0.1f)
				{
					_ScrollPos = _FrameSelectTarget;
					_IsFrameSelected = false;
				}
				Repaint();
			}

			EditorGUI.BeginChangeCheck();
			_ScrollPos = GUI.BeginScrollView(_GraphArea, _ScrollPos, _GraphExtents );
			if (EditorGUI.EndChangeCheck())
			{
				_IsFrameSelected = false;
			}

			if (ArborSettings.showGrid)
			{
				DrawGrid();
            }
		}

		void OnGraphGUI()
		{
			// TODO : Zoom out

			if (_DragBranchEnable && Event.current.type == EventType.MouseDown && (Event.current.button == 1 || Application.platform == RuntimePlatform.OSXEditor && Event.current.control))
			{
				Event.current.Use();
			}

			BeginWindows();

			foreach (State state in _StateMachine.states)
			{
				StateEditor stateEditor = GetStateEditor(state);

				DrawBehaviourBranches(stateEditor);

				Rect position = state.position;

				position.width = Mathf.Max(position.width, 300);

				string name = Localization.GetWord("State");

				if (_StateMachine.startStateID == state.stateID)
				{
					name = Localization.GetWord("Start State");
				}
				else if (state.resident)
				{
					name = Localization.GetWord("Resident State");
				}

				UnityEditor.Graphs.Styles.Color color = UnityEditor.Graphs.Styles.Color.Gray;

				if (_DragBranchEnable && _DragBranchHoverStateID == state.stateID)
				{
					color = UnityEditor.Graphs.Styles.Color.Red;
				}
				else if (_StateMachine.currentState == state)
				{
					color = UnityEditor.Graphs.Styles.Color.Orange;
				}
				else if (_StateMachine.startStateID == state.stateID)
				{
					color = UnityEditor.Graphs.Styles.Color.Aqua;
				}
				else if (state.resident)
				{
					color = UnityEditor.Graphs.Styles.Color.Green;
				}

				StateEditor currentStateEditor = stateEditor;
				GUI.WindowFunction func = (int id) => {
					OnStateGUI(currentStateEditor);
				};

				bool on = _Selection.Contains(state.stateID);
				GUIStyle nodeStyle = GetStateStyle(color, on);

				GUILayout.Window(state.stateID, position, func, name, nodeStyle);
			}

			foreach (CommentNode comment in _StateMachine.comments)
			{
				CommentEditor commentEditor = GetCommentEditor(comment);

				Rect position = comment.position;

				position.width = Mathf.Max(position.width, 300);

				string name = Localization.GetWord("Comment");

				UnityEditor.Graphs.Styles.Color color = UnityEditor.Graphs.Styles.Color.Yellow;

				CommentEditor currentCommentEditor = commentEditor;
				GUI.WindowFunction func = (int id) => {
					OnCommentGUI(currentCommentEditor);
				};

				bool on = _Selection.Contains(comment.commentID);
				GUIStyle nodeStyle = GetStateStyle(color, on);

				GUILayout.Window(comment.commentID, position, func, name, nodeStyle);
			}

			EndWindows();

			if (_DragBranchEnable)
			{
				Vector2 shadowPos = Vector2.one * 3;

				EditorGUITools.BezierArrow(_DragBranchStart + shadowPos, _DragBranchStartTangent + shadowPos, _DragBranchEnd + shadowPos, _DragBranchEndTangent + shadowPos, new Color(0, 0, 0, 1.0f), 5.0f, 16.0f);

				EditorGUITools.BezierArrow(_DragBranchStart, _DragBranchStartTangent, _DragBranchEnd, _DragBranchEndTangent, new Color(1.0f, 0.8f, 0.8f, 1.0f), 5.0f, 16.0f);
			}

			HandleContextMenu();

			Vector2 handlePosition = Event.current.mousePosition;
			HandleCommand(handlePosition);

			if (_FrameSelected)
			{
				FrameSelected();
				_FrameSelected = false;
			}

			DragSelection();
		}

		void EndGraphGUI()
		{
			UpdateGraphExtents();
			UpdateScrollPosition();
			UpdateGraphViewArea();
            DragGrid();

			GUI.EndScrollView(false);

			if (Event.current.type == EventType.ScrollWheel && _GraphArea.Contains(Event.current.mousePosition) )
			{
				_ScrollPos += Event.current.delta * 20.0f;
				Event.current.Use();
			}
		}

		void OnGUI()
		{
			DrawToolbar();

			EditorGUILayout.BeginHorizontal();

			if (ArborSettings.openStateList)
			{
				DrawStateList();
			}

			if (_StateMachine == null)
			{
				EditorGUILayout.EndHorizontal();
				return;
			}

			Event current = Event.current;

			if (current.type == EventType.ValidateCommand && current.commandName == "UndoRedoPerformed")
			{
				Repaint();
				HandleUtility.Repaint();
			}

			Rect rect = GUILayoutUtility.GetRect(0.0f,0.0f);

			Rect windowArea = new Rect( rect.x,rect.y,this.position.width-rect.x,this.position.height-rect.y );

			BeginGraphGUI(windowArea);

			OnGraphGUI();

			EndGraphGUI();

			EditorGUILayout.EndHorizontal();

			if ( EditorApplication.isPlaying )
			{
				Repaint ();
			}
		}
	}
}