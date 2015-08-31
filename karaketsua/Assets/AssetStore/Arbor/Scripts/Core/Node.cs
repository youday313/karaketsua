using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Arbor Editorにあるノードの基底クラス
	/// </summary>
#else
	/// <summary>
	/// Base class of a node in Arbor Editor
	/// </summary>
#endif
	[System.Serializable]
	public class Node
	{
		[SerializeField] protected ArborFSMInternal _StateMachine;

#if ARBOR_DOC_JA
		/// <summary>
		/// Arbor Editor上での位置。
		/// </summary>
#else
		/// <summary>
		/// Position on the Arbor Editor.
		/// </summary>
#endif
		public Rect position;

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
				return _StateMachine;
			}
		}

		public Node(ArborFSMInternal stateMachine)
		{
			_StateMachine = stateMachine;
		}
	}
}