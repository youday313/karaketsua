using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// コメントを表すクラス
	/// </summary>
#else
	/// <summary>
	/// Class that represents a comment
	/// </summary>
#endif
	[System.Serializable]
	public class CommentNode : Node
	{
		[SerializeField]
		private int _CommentID;

#if ARBOR_DOC_JA
		/// <summary>
		/// コメントIDを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the comment identifier.
		/// </summary>
#endif
		public int commentID
		{
			get
			{
				return _CommentID;
			}
		}

		public string comment;

		public CommentNode(ArborFSMInternal stateMachine, int commentID) : base(stateMachine)
		{
			_CommentID = commentID;
        }
	}
}