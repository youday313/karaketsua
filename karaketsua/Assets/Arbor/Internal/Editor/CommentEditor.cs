using UnityEngine;
using UnityEditor;
using System.Collections;

using Arbor;
namespace ArborEditor
{
	internal sealed class CommentEditor
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
}
