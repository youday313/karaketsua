using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="Arbor.ArborFSM" />の内部クラス。
	/// 実際にGameObjectにアタッチするには<see cref="Arbor.ArborFSM" />を使用する。
	/// </summary>
#else
	/// <summary>
	/// Internal class of <see cref="Arbor.ArborFSM" />.
	/// To actually attach to GameObject is to use the <see cref = "Arbor.ArborFSM" />.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public class ArborFSMInternal : MonoBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FSMの名前。
		/// </summary>
#else		
		/// <summary>
		/// The FSM name.
		/// </summary>
#endif
		public string fsmName;

		[SerializeField] private int _StartStateID;
		
		[SerializeField] private List<State> _States = new List<State>();

		[SerializeField] private List<CommentNode> _Comments = new List<CommentNode>();

		[System.NonSerialized] private State _CurrentState = null;
		[System.NonSerialized] private State _NextState = null;

		private bool _IsStarted = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始ステートのIDを取得する。
		/// </summary>
		/// <value>
		/// 開始ステートID。
		/// </value>
#else		
		/// <summary>
		/// Gets the start state identifier.
		/// </summary>
		/// <value>
		/// The start state identifier.
		/// </value>
#endif
		public int startStateID
		{
			get
			{
				return _StartStateID;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在の<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <value>
		/// 現在の<see cref="Arbor.State" />。
		/// </value>
#else
		/// <summary>
		/// Gets <see cref="Arbor.State" /> of the current.
		/// </summary>
		/// <value>
		/// <see cref="Arbor.State" /> of the current.
		/// </value>
#endif
		public State currentState
		{
			get
			{
				return _CurrentState;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 全ての<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <value>
		/// <see cref="Arbor.State" />の配列。
		/// </value>
#else
		/// <summary>
		/// Gets all of <see cref="Arbor.State" />.
		/// </summary>
		/// <value>
		/// Array of <see cref="Arbor.State" />.
		/// </value>
#endif
		public State[] states
		{
			get
			{
				return _States.ToArray();
			}
		}

		private Dictionary<int, State> _DicStates;

		private Dictionary<int, State> dicStates
		{
			get
			{
				if (_DicStates == null)
				{
					_DicStates = new Dictionary<int, State>();

					foreach (State state in _States)
					{
						_DicStates.Add(state.stateID, state);
					}
				}

				return _DicStates;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 全ての<see cref="Arbor.CommentNode" />を取得する。
		/// </summary>
#else
		/// <summary>
		/// Gets all of <see cref = "Arbor.CommentNode" />.
		/// </summary>
#endif
		public CommentNode[] comments
		{
			get
			{
				return _Comments.ToArray();
			}
		}
		
		private Dictionary<int, CommentNode> _DicComments;

		private Dictionary<int, CommentNode> dicComments
		{
			get
			{
				if (_DicComments == null)
				{
					_DicComments = new Dictionary<int, CommentNode>();

					foreach (CommentNode comment in _Comments)
					{
						_DicComments.Add(comment.commentID, comment);
					}
				}

				return _DicComments;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートIDを指定して<see cref="Arbor.State" />を取得する。
		/// </summary>
		/// <param name="stateID">ステートID</param>
		/// <returns>見つかった<see cref="Arbor.State" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.State" /> from the state identifier.
		/// </summary>
		/// <param name="stateID">The state identifier.</param>
		/// <returns>Found <see cref = "Arbor.State" />. Returns null if not found.</returns>
#endif
		public State GetStateFromID( int stateID )
		{
			State result = null;
			if( dicStates.TryGetValue( stateID,out result ) )
			{
				if( result.stateID == stateID )
				{
					return result;
				}
			}
			
			foreach( State state in _States )
			{
				if( state.stateID == stateID )
				{
					dicStates.Add( state.stateID,state );
					return state;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードIDを指定して<see cref="Arbor.Node" />を取得する。
		/// </summary>
		/// <param name="id">ノードID</param>
		/// <returns>見つかった<see cref="Arbor.Node" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.State" /> from the state identifier.
		/// </summary>
		/// <param name="stateID">The state identifier.</param>
		/// <returns>Found <see cref = "Arbor.State" />. Returns null if not found.</returns>
#endif
		public Node GetNodeFromID(int id)
		{
			State resultState = GetStateFromID(id);
			if (resultState != null)
			{
				return resultState;
            }

			CommentNode resultComment = GetCommentFromID(id);
			if (resultComment != null)
			{
				return resultComment;
			}
			return null;
		}

		int GetUniqueNodeID()
		{
			int count = _States.Count+_Comments.Count;

			System.Random random = new System.Random(count);
			
			while( true )
			{
				int nodeID = random.Next();
				
				if(nodeID != 0 && GetStateFromID(nodeID) == null && GetCommentFromID(nodeID) == null )
				{
					return nodeID;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コメントIDを指定して<see cref="Arbor.CommentNode" />を取得する。
		/// </summary>
		/// <param name="commentID">コメントID</param>
		/// <returns>見つかった<see cref="Arbor.CommentNode" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.CommentNode" /> from the comment identifier.
		/// </summary>
		/// <param name="commentID">The comment identifier.</param>
		/// <returns>Found <see cref = "Arbor.CommentNode" />. Returns null if not found.</returns>
#endif
		public CommentNode GetCommentFromID(int commentID)
		{
			CommentNode result = null;
			if (dicComments.TryGetValue(commentID, out result))
			{
				if (result.commentID == commentID)
				{
					return result;
				}
			}

			foreach (CommentNode comment in _Comments)
			{
				if (comment.commentID == commentID)
				{
					dicComments.Add(comment.commentID, comment);
					return comment;
				}
			}

			return null;
		}

#if ARBOR_TRIAL
		static readonly int _LimitStateNum = 3;
#endif

#if ARBOR_TRIAL
		bool IsTrialLimit()
		{
			int count = _States.Count;
			if( count >= _LimitStateNum )
			{
				return true;
			}
			
			return false;
		}
#endif

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを生成。
		/// </summary>
		/// <param name="resident">常駐するかどうかのフラグ。</param>
		/// <returns>生成したステート。</returns>
#else
		/// <summary>
		/// Create state.
		/// </summary>
		/// <param name="resident">Resident whether flags.</param>
		/// <returns>The created state.</returns>
#endif
		public State CreateState( bool resident )
		{
#if ARBOR_TRIAL
			if( IsTrialLimit() )
			{
				Debug.LogError( "There is a limit to the number that can be created State in the trial version." );
				return null;
			}
#endif
			State state = new State( this,GetUniqueNodeID(),resident );

			ComponentUtility.RecordObject(this, "Created State");

			_States.Add( state );
			
			if( !resident && _StartStateID == 0 )
			{
				_StartStateID = state.stateID;
			}

			ComponentUtility.SetDirty(this);

			return state;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを生成。
		/// </summary>
		/// <returns>生成したステート。</returns>
#else
		/// <summary>
		/// Create state.
		/// </summary>
		/// <returns>The created state.</returns>
#endif
		public State CreateState()
		{
			return CreateState( false );
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コメントを生成。
		/// </summary>
		/// <returns>生成したコメント。</returns>
#else
		/// <summary>
		/// Create comment.
		/// </summary>
		/// <returns>The created comment.</returns>
#endif
		public CommentNode CreateComment()
		{
			CommentNode comment = new CommentNode(this, GetUniqueNodeID());

			ComponentUtility.RecordObject(this, "Created Comment");

			_Comments.Add(comment);

			ComponentUtility.SetDirty(this);

			return comment;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを名前で検索。
		/// </summary>
		/// <param name="stateName">検索するステートの名前。</param>
		/// <returns>見つかったステート。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Search state by name.
		/// </summary>
		/// <param name="stateName">The name of the search state.</param>
		/// <returns>Found state. Return null if not.</returns>
#endif
		public State FindState( string stateName )
		{
			foreach( State state in _States )
			{
				if( state.name == stateName )
				{
					return state;
				}
			}

			return null;
		}
		
#if ARBOR_DOC_JA
		/// <summary>
		/// ステートを名前で検索。
		/// </summary>
		/// <param name="stateName">検索するステートの名前。</param>
		/// <returns>見つかったステートの配列。</returns>
#else
		/// <summary>
		/// Search state by name.
		/// </summary>
		/// <param name="stateName">The name of the search state.</param>
		/// <returns>Array of found state.</returns>
#endif
		public State[] FindStates( string stateName )
		{
			List<State> list = new List<State>();
			foreach( State state in _States )
			{
				if( state.name == stateName )
				{
					list.Add( state );
				}
			}

			return list.ToArray();
		}
		
#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourが属しているステートの取得。
		/// </summary>
		/// <param name="behaviour">StateBehaviour</param>
		/// <returns>StateBehaviourが属しているステート。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Acquisition of states StateBehaviour belongs.
		/// </summary>
		/// <param name="behaviour">StateBehaviour</param>
		/// <returns>States StateBehaviour belongs. Return null if not.</returns>
#endif
		public State FindStateContainsBehaviour( StateBehaviour behaviour )
		{
			foreach( State state in _States )
			{
				if( state.Contains( behaviour ) )
				{
					return state;
				}
			}

			return null;
		}
		
		static void DisconectStateObject( object obj,int stateID )
		{
			if( obj == null )
			{
				return;
			}
			
			System.Type type = obj.GetType();
			
			if( type == typeof( StateLink ) )
			{
				StateLink stateLink = (StateLink)obj;
				
				if( stateLink.stateID == stateID )
				{
					stateLink.stateID = 0;
				}
			}
			else if( typeof(IList).IsAssignableFrom(type) )
			{
				DisconectStateArray( (IList)obj,stateID );
			}
			else
			{
				object[] attributes = type.GetCustomAttributes( typeof( System.SerializableAttribute),false );
				if( attributes!=null && attributes.Length>0 )
				{
					DisconectState( obj,type,stateID );
				}
			}
		}
		
		static void DisconectStateArray( IList list,int stateID )
		{
			if( list != null )
			{
				foreach( var item in list )
				{
					DisconectStateObject( item,stateID );
				}
			}
		}
		
		static void DisconectState( object obj,System.Type type,int stateID )
		{
			System.Reflection.FieldInfo[] fields = type.GetFields( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public );
			
			foreach( System.Reflection.FieldInfo field in fields )
			{
				bool serializeField = false;
				
				if( field.IsPublic )
				{
					serializeField = true;
				}
				else
				{
					object[] attributes = field.GetCustomAttributes( typeof(SerializeField),false );
					if( attributes!=null && attributes.Length>0 )
					{
						serializeField = true;
					}
				}
				
				if( serializeField )
				{
					DisconectStateObject( field.GetValue(obj),stateID );
				}
			}
		}
		
#if ARBOR_DOC_JA
		/// <summary>
		/// ステートの削除。
		/// </summary>
		/// <param name="state">削除するステート。</param>
#else
		/// <summary>
		/// Delete state.
		/// </summary>
		/// <param name="state">State that you want to delete.</param>
#endif
		public void DeleteState( State state )
		{
			int stateID = state.stateID;

			ComponentUtility.RecordObject(this, "Delete Nodes");
			
			_States.Remove( state );
			
			if( _DicStates != null )
			{
				_DicStates.Remove ( state.stateID );
			}
			
			if( _StartStateID == stateID )
			{
				_StartStateID = 0;
			}

			foreach (State otherState in _States)
			{
				if (otherState != state)
				{
					foreach (StateBehaviour behaviour in otherState.behaviours)
					{
						System.Type type = behaviour.GetType();

						ComponentUtility.RecordObject(behaviour, "Delete Nodes");

						DisconectState(behaviour, type, state.stateID);
					}
				}
			}

			foreach (StateBehaviour behaviour in state.behaviours)
			{
				behaviour.Destroy();
			}

			ComponentUtility.SetDirty(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コメントの削除。
		/// </summary>
		/// <param name="comment">削除するコメント。</param>
#else
		/// <summary>
		/// Delete comment.
		/// </summary>
		/// <param name="comment">Comment that you want to delete.</param>
#endif
		public void DeleteComment(CommentNode comment)
		{
			ComponentUtility.RecordObject(this, "Delete Nodes");

			_Comments.Remove(comment);

			if (_DicComments != null)
			{
				_DicComments.Remove(comment.commentID);
			}

			ComponentUtility.SetDirty(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの削除。
		/// </summary>
		/// <param name="node">削除するノード。</param>
#else
		/// <summary>
		/// Delete node.
		/// </summary>
		/// <param name="node">Node that you want to delete.</param>
#endif
		public void DeleteNode(Node node)
		{
			if (node is State)
			{
				DeleteState(node as State);
			}
			else if (node is CommentNode)
			{
				DeleteComment(node as CommentNode);
			}
		}

		public void RefreshBehaviours()
		{
			foreach (StateBehaviour behaviour in GetComponents<StateBehaviour>())
			{
				State state = behaviour.state;
				if (behaviour.stateMachine == this && state != null && !state.Contains(behaviour))
				{
					state.AddBehaviour(behaviour);
                }
			}

			foreach (State state in _States)
			{
				foreach (StateBehaviour behaviour in state.behaviours)
				{
					if (behaviour != null)
					{
						behaviour.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
					}
				}
			}
		}
		
		void OnValidate()
		{
			if( _DicStates != null )
			{
				_DicStates.Clear ();
			}

			bool moved = false;

			foreach( State state in _States )
			{
				if( state.stateMachine != this )
				{
					moved = true;
				}
				else
				{
					foreach( StateBehaviour behaviour in state.behaviours )
					{
						if(behaviour != null && behaviour.stateMachine != this )
						{
							moved = true;
						}
					}
				}

				if( moved )
				{
					break;
				}
			}

			if( moved )
			{
				foreach ( State state in _States )
				{
					state.Move( this );
				}
			}

			ComponentUtility.RefreshBehaviours(this);
		}
		
		void Start()
		{
			RefreshBehaviours();

			foreach( State state in _States )
			{
				if( state.resident )
				{
					EnableState( state,true,true );
				}
			}

			State nextState = GetStateFromID( _StartStateID );
			ChangeState( nextState );

			_IsStarted = true;
		}

		void OnEnable()
		{
			if( !_IsStarted )
			{
				return;
			}

			foreach( State state in _States )
			{
				if( state.resident )
				{
					EnableState( state,true,false );
				}
			}

			if( _CurrentState != null )
			{
				EnableState( _CurrentState,true,false );
			}
		}

		void OnDisable()
		{
			if( !_IsStarted )
			{
				return;
			}

			if( _CurrentState != null )
			{
				EnableState( _CurrentState,false,false );
			}

			foreach( State state in _States )
			{
				if( state.resident )
				{
					EnableState( state,false,false );
				}
			}
		}

		void LateUpdate()
		{
			if( _NextState != null )
			{
				State nextState = _NextState;
				_NextState = null;

				ChangeState( nextState );
			}
		}

		void ChangeState( State nextState )
		{
			if( _CurrentState != null )
			{
				EnableState( _CurrentState,false,true );
			}
			
			_CurrentState = nextState;
			
			if( _CurrentState != null )
			{
				EnableState( _CurrentState,true,true );
			}
		}
		
		void EnableState( State state,bool enable,bool changeState )
		{
			foreach( StateBehaviour behaviour in state.behaviours )
			{
				if( behaviour != null && behaviour.behaviourEnabled )
				{
					if( enable )
					{
						if (!behaviour.enabled)
						{
							behaviour.enabled = true;
							if (changeState)
							{
								State nextState = _NextState;
                                behaviour.OnStateBegin();
								if (_CurrentState != null && _CurrentState.stateID != state.stateID || 
									_NextState != nextState )
								{
									return;
								}
							}
						}
					}
					else
					{
						if (behaviour.enabled)
						{
							if (changeState)
							{
								behaviour.OnStateEnd();
							}
							behaviour.enabled = false;
						}
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextState">遷移先のステート。</param>
		/// <param name="immediateTransition">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextState">Destination state.</param>
		/// <param name="immediateTransition">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(State nextState, bool immediateTransition)
		{
			if (nextState != null && nextState.stateMachine == this && !nextState.resident)
			{
				if (immediateTransition)
				{
					ChangeState(nextState);
				}
				else
				{
					_NextState = nextState;
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移する。実際に遷移するタイミングは現在フレームの最後(LateUpdate時)。
		/// </summary>
		/// <param name="nextState">遷移先のステート。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition. Timing to actually transition current frame last (when LateUpdate).
		/// </summary>
		/// <param name="nextState">Destination state.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(State nextState)
		{
			return Transition(nextState, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextStateID">遷移先のステートID。</param>
		/// <param name="immediateTransition">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextState">State ID for the transition destination.</param>
		/// <param name="immediateTransition">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(int nextStateID, bool immediateTransition)
		{
			State nextState = GetStateFromID(nextStateID);
			return Transition(nextState, immediateTransition);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移する。実際に遷移するタイミングは現在フレームの最後(LateUpdate時)。
		/// </summary>
		/// <param name="nextStateID">遷移先のステートID。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition. Timing to actually transition current frame last (when LateUpdate).
		/// </summary>
		/// <param name="nextStateID">State ID for the transition destination.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(int nextStateID)
		{
			return Transition(nextStateID, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextStateLink">遷移の接続先。</param>
		/// <param name="immediateTransition">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextStateLink">The destination of transition.</param>
		/// <param name="immediateTransition">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(StateLink nextStateLink, bool immediateTransition)
		{
			if (nextStateLink.stateID != 0)
			{
				return Transition(nextStateLink.stateID, immediateTransition);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移する。実際に遷移するタイミングは現在フレームの最後(LateUpdate時)。
		/// </summary>
		/// <param name="nextStateLink">遷移の接続先。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition. Timing to actually transition current frame last (when LateUpdate).
		/// </summary>
		/// <param name="nextStateLink">The destination of transition.</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(StateLink nextStateLink)
		{
			return Transition(nextStateLink, nextStateLink.immediateTransition);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーの送信
		/// </summary>
		/// <param name="message">送信するメッセージ</param>
#else
		/// <summary>
		/// Sending of trigger
		/// </summary>
		/// <param name="message">Message to be sent</param>
#endif
		public void SendTrigger( string message )
		{
			if( !gameObject.activeInHierarchy || !enabled )
			{
				return;
			}

			foreach( State state in _States )
			{
				if( state.resident )
				{
					state.SendTrigger( message );
				}
			}

			if( _CurrentState != null )
			{
				_CurrentState.SendTrigger( message );
			}
		}
		
		void OnDestroy()
		{
			DestroySubComponents();
		}
		
#if ARBOR_DOC_JA
		/// <summary>
		/// 内部的に使用するメソッド。特に呼び出す必要はありません。
		/// </summary>
#else
		/// <summary>
		/// Method to be used internally. In particular there is no need to call.
		/// </summary>
#endif
		public void DestroySubComponents()
		{
			foreach( State state in _States )
			{
				foreach( StateBehaviour behaviour in state.behaviours )
				{
					if( behaviour != null )
					{
						if( Application.isPlaying )
						{
							Destroy( behaviour );
						}
						else
						{
							DestroyImmediate( behaviour );
						}
					}
				}
			}
		}
	}
}
