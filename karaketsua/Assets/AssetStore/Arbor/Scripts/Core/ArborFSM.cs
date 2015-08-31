using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Arborのコア部分。
	/// GameObjectにアタッチして使用する。
	/// </summary>
#else
	/// <summary>
	/// Core part of Arbor.
	/// Is used by attaching to GameObject.
	/// </summary>
#endif
	[AddComponentMenu("Arbor/ArborFSM")]
	public sealed class ArborFSM : ArborFSMInternal
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内にあるArborFSMを名前で取得する。
		/// </summary>
		/// <param name="name">検索するArborFSMの名前。</param>
		/// <returns>見つかったArborFSM。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the ArborFSM that in the scene with the name.
		/// </summary>
		/// <param name="name">The name of the search ArborFSM</param>
		/// <returns>Found ArborFSM. Returns null if not found.</returns>
#endif
		public static ArborFSM FindFSM( string name )
		{
			foreach( ArborFSM fsm in ArborFSM.FindObjectsOfType<ArborFSM>() )
			{
				if( fsm.fsmName.Equals( name ) )
				{
					return fsm;
				}
			}
			
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内にある同一名のArborFSMを取得する。
		/// </summary>
		/// <param name="name">検索するArborFSMの名前。</param>
		/// <returns>見つかったArborFSMの配列。</returns>
#else
		/// <summary>
		/// Get the ArborFSM of the same name that is in the scene.
		/// </summary>
		/// <param name="name">The name of the search ArborFSM.</param>
		/// <returns>Array of found ArborFSM.</returns>
#endif
		public static ArborFSM[] FindFSMs( string name )
		{
			List<ArborFSM> fsms = new List<ArborFSM>();
			
			foreach( ArborFSM fsm in ArborFSM.FindObjectsOfType<ArborFSM>() )
			{
				if( fsm.fsmName.Equals( name ) )
				{
					fsms.Add( fsm );
				}
			}
			
			return fsms.ToArray();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectにアタッチされているArborFSMを名前で取得する。
		/// </summary>
		/// <param name="gameObject">検索したいGameObject。</param>
		/// <param name="name">検索するArborFSMの名前。</param>
		/// <returns>見つかったArborFSM。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get ArborFSM in the name that has been attached to the GameObject.
		/// </summary>
		/// <param name="gameObject">Want to search GameObject.</param>
		/// <param name="name">The name of the search ArborFSM.</param>
		/// <returns>Found ArborFSM. Returns null if not found.</returns>
#endif
		public static ArborFSM FindFSM( GameObject gameObject,string name )
		{
			foreach( ArborFSM fsm in gameObject.GetComponents<ArborFSM>() )
			{
				if( fsm.fsmName.Equals( name ) )
				{
					return fsm;
				}
			}
			
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectにアタッチされている同一名のArborFSMを取得する。
		/// </summary>
		/// <param name="gameObject">検索したいGameObject。</param>
		/// <param name="name">検索するArborFSMの名前。</param>
		/// <returns>見つかったArborFSMの配列。</returns>
#else
		/// <summary>
		/// Get the ArborFSM of the same name that is attached to a GameObject.
		/// </summary>
		/// <param name="gameObject">Want to search GameObject.</param>
		/// <param name="name">The name of the search ArborFSM.</param>
		/// <returns>Array of found ArborFSM.</returns>
#endif
		public static ArborFSM[] FindFSMs( GameObject gameObject,string name )
		{
			List<ArborFSM> fsms = new List<ArborFSM>();
			
			foreach( ArborFSM fsm in gameObject.GetComponents<ArborFSM>() )
			{
				if( fsm.fsmName.Equals( name ) )
				{
					fsms.Add( fsm );
				}
			}
			
			return fsms.ToArray();
		}
	}
}