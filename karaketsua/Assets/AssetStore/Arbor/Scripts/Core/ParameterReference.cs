using UnityEngine;
using System.Collections;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference parameters.
	/// </summary>
#endif
	[System.Serializable]
	public class ParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 格納しているコンテナ。
		/// </summary>
#else
		/// <summary>
		/// Is stored to that container.
		/// </summary>
#endif
		public ParameterContainerBase container;

#if ARBOR_DOC_JA
		/// <summary>
		/// ID。
		/// </summary>
#else
		/// <summary>
		/// ID.
		/// </summary>
#endif
		public int id;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータを取得する。存在しない場合はnull。
		/// </summary>
#else
		/// <summary>
		/// Get the parameters. null if it does not exist.
		/// </summary>
#endif
		public Parameter parameter
		{
			get
			{
				if (container == null)
				{
					return null;
				}
				ParameterContainerInternal parameterContainer = container.container;

				if (parameterContainer == null)
				{
					return null;
				}

				return parameterContainer.GetParam(id);
			}
		}
	}
}
