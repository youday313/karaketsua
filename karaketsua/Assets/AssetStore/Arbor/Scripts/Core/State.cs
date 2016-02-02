using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ステートを表すクラス
	/// </summary>
#else	
	/// <summary>
	/// Class that represents the state
	/// </summary>
#endif
	[System.Serializable]
	public sealed class State : Node
	{
		[SerializeField] private int _StateID;
		[SerializeField] private bool _Resident = false;
		[SerializeField] private List<StateBehaviour> _Behaviours = new List<StateBehaviour>();
		
#if ARBOR_DOC_JA
		/// <summary>
		/// ステートの名前。
		/// </summary>
#else		
		/// <summary>
		/// The name of the state.
		/// </summary>
#endif
		public string name = "New State";
		
#if ARBOR_DOC_JA
		/// <summary>
		/// Behaviourの配列を取得。
		/// </summary>
#else		
		/// <summary>
		/// Gets the behaviours.
		/// </summary>
#endif
		public StateBehaviour[] behaviours
		{
			get
			{
				return _Behaviours.ToArray();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ステートIDを取得。
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
		/// 常駐する<see cref="State"/>かどうかを取得。
		/// </summary>
#else		
		/// <summary>
		/// Gets a value indicating whether this <see cref="State"/> is resident.
		/// </summary>
#endif
		public bool resident
		{
			get
			{
				return _Resident;
			}
		}
		
#if ARBOR_DOC_JA
		/// <summary>
		/// ステートの生成は<see cref="ArborFSMInternal.CreateState"/>を使用してください。
		/// </summary>
#else		
		/// <summary>
		/// Please use the <see cref = "ArborFSMInternal.CreateState" /> state creating.
		/// </summary>
#endif
		public State( ArborFSMInternal stateMachine,int stateID,bool resident ) : base(stateMachine)
		{
			_StateID = stateID;
			_Resident = resident;
		}

#if ARBOR_TRIAL
		bool IsTrialLimit()
		{
			int count = _Behaviours.Count;
			if( count >= 2 )
			{
				return true;
			}

			return false;
		}
#endif

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを追加。
		/// </summary>
		/// <param name="behaviour">追加するStateBehaviour</param>
#else
		/// <summary>
		/// Adds the behaviour.
		/// </summary>
		/// <param name="behaviour">Add StateBehaviour</param>
#endif
		public void AddBehaviour(StateBehaviour behaviour)
		{
			ComponentUtility.RecordObject(stateMachine, "Add Behaviour");

			_Behaviours.Add(behaviour);

			ComponentUtility.SetDirty(stateMachine);
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
#if ARBOR_TRIAL
			if( IsTrialLimit() )
			{
				Debug.LogError( "There is a limit to the number that can be assigned StateBehaviour in the trial version." );
				return null;
			}
#endif

			StateBehaviour behaviour = StateBehaviour.Create( _StateMachine,stateID,type );

			ComponentUtility.RecordObject(stateMachine, "Add Behaviour");

			_Behaviours.Add( behaviour );

			ComponentUtility.SetDirty(stateMachine);

			return behaviour;
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
#if ARBOR_TRIAL
			if( IsTrialLimit() )
			{
				Debug.LogError( "There is a limit to the number that can be assigned StateBehaviour in the trial version." );
				return null;
			}
#endif

			T behaviour = StateBehaviour.Create<T>( _StateMachine,stateID );

			ComponentUtility.RecordObject(stateMachine, "Add Behaviour");

			_Behaviours.Add( behaviour );

			ComponentUtility.SetDirty(stateMachine);

			return behaviour;
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
			foreach( StateBehaviour behaviour in _Behaviours )
			{
				System.Type classType = behaviour.GetType ();

				if( classType == type || classType.IsSubclassOf( type ) )
				{
					return behaviour;
				}
			}

			return null;
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
			System.Type type = typeof(T);

			foreach( StateBehaviour behaviour in _Behaviours )
			{
				System.Type classType = behaviour.GetType ();
				
				if( classType == type || classType.IsSubclassOf( type ) )
				{
					return behaviour as T;
				}
			}

			return null;
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
			List<StateBehaviour> list = new List<StateBehaviour>();

			foreach( StateBehaviour behaviour in _Behaviours )
			{
				System.Type classType = behaviour.GetType ();

				if( classType == type || classType.IsSubclassOf( type ) )
				{
					list.Add( behaviour );
				}
			}

			return list.ToArray();
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
			System.Type type = typeof(T);

			List<T> list = new List<T>();

			foreach( StateBehaviour behaviour in _Behaviours )
			{
				System.Type classType = behaviour.GetType ();
				
				if( classType == type || classType.IsSubclassOf( type ) )
				{
					list.Add( behaviour as T );
				}
			}

			return list.ToArray();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourが含まれているかどうか。
		/// </summary>
		/// <param name="behaviour">判定するStateBehaviour。</param>
		/// <returns>含まれているかどうか。</returns>
#else
		/// <summary>
		/// Whether StateBehaviour are included.
		/// </summary>
		/// <param name="behaviour">Judges StateBehaviour.</param>
		/// <returns>Whether included are.</returns>
#endif
		public bool Contains( StateBehaviour behaviour )
		{
			return _Behaviours.Contains( behaviour );
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourを削除する。インスタンスは削除されないため、<see cref="StateBehaviour.Destroy" />を使用すること。
		/// </summary>
		/// <param name="behaviour">削除するStateBehaviour。</param>
#else
		/// <summary>
		/// I want to remove the StateBehaviour. For instance is not deleted, that you use the <see cref = "StateBehaviour.Destroy" />.
		/// </summary>
		/// <param name="behaviour">StateBehaviour you want to remove.</param>
#endif
		public void RemoveBehaviour( StateBehaviour behaviour )
		{
			int index = _Behaviours.IndexOf( behaviour );
			if( index >= 0 )
			{
				ComponentUtility.RecordObject(stateMachine, "Add Behaviour");
				_Behaviours.RemoveAt( index );
				ComponentUtility.SetDirty(stateMachine);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StateBehaviourの順番を入れ替える。
		/// </summary>
		/// <param name="fromIndex">入れ替えたいインデックス。</param>
		/// <param name="toIndex">入れ替え先インデックス。</param>
#else
		/// <summary>
		/// Swap the order of StateBehaviour.
		/// </summary>
		/// <param name="fromIndex">The swapping want index.</param>
		/// <param name="toIndex">Exchange destination index.</param>
#endif
		public void SwapBehaviour( int fromIndex,int toIndex )
		{
			StateBehaviour behaviour = _Behaviours[toIndex];
			_Behaviours[toIndex] = _Behaviours[fromIndex];
			_Behaviours[fromIndex] = behaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public delegate Object InstanceIDToObject( int instanceID );

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public void ForceRebuild( InstanceIDToObject instanceIDToObject )
		{
			if( !Application.isEditor || Application.isPlaying )
			{
				throw new System.NotSupportedException();
			}

			for( int i=0;i<_Behaviours.Count;i++ )
			{
				_Behaviours[i] = (StateBehaviour)instanceIDToObject( _Behaviours[i].GetInstanceID() );
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public void Move( ArborFSMInternal stateMachine )
		{
			if( !Application.isEditor || Application.isPlaying )
			{
				throw new System.NotSupportedException();
			}

			_StateMachine = stateMachine;

			StateBehaviour[] sourceBehaviours = _Behaviours.ToArray();
			_Behaviours.Clear();

			foreach( StateBehaviour sourceBehaviour in sourceBehaviours )
			{
				ComponentUtility.MoveBehaviour( this, sourceBehaviour);
			}
		}

		private static readonly System.Reflection.BindingFlags _OnStateTriggerBindingAttr = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
		private static System.Type[] _OnStateTriggerTypes = new System.Type[]{ typeof(string) };

		private static Dictionary<System.Type,System.Reflection.MethodInfo> _OnStateTriggerMethods = new Dictionary<System.Type, System.Reflection.MethodInfo>();

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーメッセージを送信する。<see cref="StateBehaviour.OnStateTrigger"/>が呼び出される。
		/// </summary>
		/// <param name="message"></param>
#else
		/// <summary>
		/// Send a trigger message.<see cref = "StateBehaviour.OnStateTrigger" /> is called.
		/// </summary>
		/// <param name="message"></param>
#endif
		public void SendTrigger( string message )
		{
			if (stateMachine.gameObject.activeInHierarchy && stateMachine.enabled)
			{
				foreach (StateBehaviour behaviour in _Behaviours)
				{
					if( behaviour.enabled && behaviour.behaviourEnabled )
					{
						System.Type type = behaviour.GetType();
						System.Reflection.MethodInfo methodInfo = null;
						if (!_OnStateTriggerMethods.TryGetValue(type, out methodInfo))
						{
							methodInfo = type.GetMethod("OnStateTrigger", _OnStateTriggerBindingAttr, null, _OnStateTriggerTypes, null);
							_OnStateTriggerMethods.Add(type, methodInfo);
						}

						if (methodInfo != null)
						{
							methodInfo.Invoke(behaviour, new object[] { message });
						}
					}
				}
			}
		}
	}
}
