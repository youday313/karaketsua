using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Stateの挙動を定義するクラス。継承して利用する。
	/// </summary>
#else
	/// <summary>
	/// Class that defines the behavior of the State. Inherited and to use.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public class StateBehaviour : MonoBehaviour
	{
		[HideInInspector][SerializeField] private ArborFSMInternal _StateMachine;
		[HideInInspector][SerializeField] private int _StateID;
		[HideInInspector] public bool expanded = true;
		[HideInInspector][SerializeField] private bool _BehaviourEnabled = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// FSMを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the state machine.
		/// </summary>
#endif
		public ArborFSMInternal stateMachine
		{
			get
			{
				if( _StateMachine == null )
				{
					ArborFSMInternal[] stateMachines = gameObject.GetComponents<ArborFSMInternal>();
					foreach( ArborFSMInternal fsm in stateMachines )
					{
						State s = fsm.FindStateContainsBehaviour( this );
						if( s != null )
						{
							_StateMachine = fsm;
							_StateID = s.stateID;
							break;
						}
					}
				}
				return _StateMachine;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateを取得。
		/// </summary>
#else
		/// <summary>
		/// Get the State.
		/// </summary>
#endif
		public State state
		{
			get
			{
				ArborFSMInternal fsm = stateMachine;
				if( fsm != null )
				{
					return fsm.GetStateFromID( _StateID );
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateIDを取得。
		/// </summary>
#else		
		/// <summary>
		/// Gets the state identifier.
		/// </summary>
#endif
		public int stateID
		{
			get
			{
				return _StateID;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourの有効状態を取得/設定。
		/// </summary>
		/// <value>
		///   <c>true</c> 有効; その他、 <c>false</c>.
		/// </value>
#else
		/// <summary>
		/// Gets or sets a value indicating whether [behaviour enabled].
		/// </summary>
		/// <value>
		///   <c>true</c> if [behaviour enabled]; otherwise, <c>false</c>.
		/// </value>
#endif
		public bool behaviourEnabled
		{
			get
			{
				return _BehaviourEnabled;
			}
			set
			{
				if( _BehaviourEnabled != value )
				{
					_BehaviourEnabled = value;
					if( stateMachine.currentState == state )
					{
						enabled = _BehaviourEnabled;
					}
				}
			}
		}

		internal static StateBehaviour Create( ArborFSMInternal stateMachine,int stateID,System.Type type )
		{
			System.Type classType = typeof(StateBehaviour);
			if( type != classType && !type.IsSubclassOf( classType ) )
			{
				throw new System.ArgumentException( "The type `" + type.Name + "' must be convertible to `StateBehaviour' in order to use it as parameter `type'","type" );
			}

			StateBehaviour behaviour = ComponentUtility.AddComponent( stateMachine.gameObject,type ) as StateBehaviour;

			behaviour.enabled = false;
			behaviour.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
			
			behaviour._StateMachine = stateMachine;
			behaviour._StateID = stateID;

			return behaviour;
		}

		internal static Type Create<Type>( ArborFSMInternal stateMachine,int stateID ) where Type : StateBehaviour
		{
			Type behaviour = ComponentUtility.AddComponent<Type>( stateMachine.gameObject );

			behaviour.enabled = false;
			behaviour.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
			
			behaviour._StateMachine = stateMachine;
			behaviour._StateID = stateID;

			return behaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateに入った際に呼ばれる。
		/// </summary>
#else		
		/// <summary>
		/// Called when [state enter].
		/// </summary>
#endif
		public virtual void OnStateBegin()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Stateから出る際に呼ばれる。
		/// </summary>
#else		
		/// <summary>
		/// Called when [state exit].
		/// </summary>
#endif
		public virtual void OnStateEnd()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextStateID">遷移先のステートID。</param>
		/// <param name="force">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextState">State ID for the transition destination.</param>
		/// <param name="force">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(int nextStateID, bool force)
		{
			if (!enabled)
			{
				return false;
			}

			if (stateMachine != null)
			{
				return stateMachine.Transition(stateID,force);
			}

			return false;
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
		public bool Transition( int stateID )
		{
			if( !enabled )
			{
				return false;
			}
			
			if( stateMachine != null )
			{
				return stateMachine.Transition( stateID );
			}
			
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextState">遷移先のステート。</param>
		/// <param name="force">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextState">Destination state.</param>
		/// <param name="force">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(State nextState, bool force)
		{
			if (!enabled)
			{
				return false;
			}

			if (stateMachine != null)
			{
				return stateMachine.Transition(nextState,force);
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
		public bool Transition( State nextState )
		{
			if( !enabled )
			{
				return false;
			}

			if( stateMachine != null )
			{
				return stateMachine.Transition( nextState );
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態遷移
		/// </summary>
		/// <param name="nextStateLink">遷移の接続先。</param>
		/// <param name="force">すぐに遷移するかどうか。falseの場合は現在フレームの最後(LateUpdate時)に遷移する。</param>
		/// <returns>遷移できたかどうか</returns>
#else
		/// <summary>
		/// State transition
		/// </summary>
		/// <param name="nextStateLink">The destination of transition.</param>
		/// <param name="force">Whether or not to transition immediately. If false I will transition to the end of the current frame (when LateUpdate).</param>
		/// <returns>Whether or not the transition</returns>
#endif
		public bool Transition(StateLink nextStateLink, bool force)
		{
			if (!enabled)
			{
				return false;
			}

			if (stateMachine != null)
			{
				return stateMachine.Transition(nextStateLink, force);
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
		public bool Transition( StateLink nextStateLink )
		{
			if( !enabled )
			{
				return false;
			}

			if( stateMachine != null )
			{
				return stateMachine.Transition( nextStateLink );
			}
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <param name="type">追加するStateBehaviourの型</param>
		/// <returns>追加したStateBehaviour</returns>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <param name="type">Type of add StateBehaviour</param>
		/// <returns>Added StateBehaviour</returns>
#endif
		public StateBehaviour AddBehaviour( System.Type type )
		{
			return state.AddBehaviour( type );
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <typeparam name="T">追加するStateBehaviourの型</typeparam>
		/// <returns>追加したStateBehaviour</returns>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <typeparam name="T">Type of add StateBehaviour</typeparam>
		/// <returns>Added StateBehaviour</returns>
#endif
		public T AddBehaviour<T>() where T : StateBehaviour
		{
			return state.AddBehaviour<T>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <param name="type">取得したいStateBehaviourの型。</param>
		/// <returns>見つかったStateBehaviour。ない場合はnull。</returns>
#else
		/// <summary>
		/// Gets the behaviour.
		/// </summary>
		/// <param name="type">Type of you want to get StateBehaviour.</param>
		/// <returns>Found StateBehaviour. Or null if it is not.</returns>
#endif
		public StateBehaviour GetBehaviour( System.Type type )
		{
			return state.GetBehaviour( type );
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <typeparam name="T">取得したいStateBehaviourの型。</typeparam>
		/// <returns>見つかったStateBehaviour。ない場合はnull。</returns>
#else
		/// <summary>
		/// Gets the behaviour.
		/// </summary>
		/// <typeparam name="T">Type of you want to get StateBehaviour.</typeparam>
		/// <returns>Found StateBehaviour. Or null if it is not.</returns>
#endif
		public T GetBehaviour<T>() where T : StateBehaviour
		{
			return state.GetBehaviour<T>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <param name="type">取得したいStateBehaviourの型。</param>
		/// <returns>見つかったStateBehaviourの配列。</returns>
#else
		/// <summary>
		/// Gets the behaviours.
		/// </summary>
		/// <param name="type">Type of you want to get StateBehaviour.</param>
		/// <returns>Array of found StateBehaviour.</returns>
#endif
		public StateBehaviour[] GetBehaviours( System.Type type )
		{
			return state.GetBehaviours( type );
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを取得。
		/// </summary>
		/// <typeparam name="T">取得したいStateBehaviourの型。</typeparam>
		/// <returns>見つかったStateBehaviourの配列。</returns>
#else
		/// <summary>
		/// Gets the behaviours.
		/// </summary>
		/// <typeparam name="T">Type of you want to get StateBehaviour.</typeparam>
		/// <returns>Array of found StateBehaviour.</returns>
#endif
		public T[] GetBehaviours<T>() where T : StateBehaviour
		{
			return state.GetBehaviours<T>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// インスタンスを削除する。
		/// </summary>
#else		
		/// <summary>
		/// Destroys this instance.
		/// </summary>
#endif
		public void Destroy()
		{
			State s = state;

			ComponentUtility.Destroy( this );

			if( s != null )
			{
				s.RemoveBehaviour( this );
			}
		}
	}
}