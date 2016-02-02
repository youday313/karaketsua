using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// パラメータコンテナ。
	/// GameObjectにアタッチして使用する。
	/// </summary>
#else
	/// <summary>
	/// ParameterContainer。
	/// Is used by attaching to GameObject.
	/// </summary>
#endif
	[AddComponentMenu("Arbor/ParameterContainer")]
	public class ParameterContainer : ParameterContainerInternal
	{
	}
}
