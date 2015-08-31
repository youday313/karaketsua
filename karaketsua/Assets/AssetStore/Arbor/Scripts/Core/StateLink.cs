using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Stateの遷移先を格納するクラス。
	/// </summary>
#else	
	/// <summary>
	/// Class that contains a transition destination State.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class StateLink
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先StateのID。
		/// </summary>
#else		
		/// <summary>
		/// ID of transition destination State.
		/// </summary>
#endif
		public int stateID;

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public bool lineEnable;

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public Vector2 lineStart;

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public Vector2 lineStartTangent;

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public Vector2 lineEnd;

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public Vector2 lineEndTangent;

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public bool lineColorChanged;

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public Color lineColor = new Color( 1.0f,1.0f,1.0f,1.0f );
	}
}