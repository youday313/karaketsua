using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// シーンをまたいでもアクセス可能なParameterContainerrを扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class dealing with the accessible ParameterContainer even across the scene.
	/// </summary>
#endif
	[AddComponentMenu("Arbor/GlobalParameterContainer")]
	public class GlobalParameterContainer : GlobalParameterContainerInternal
	{
	}
}
