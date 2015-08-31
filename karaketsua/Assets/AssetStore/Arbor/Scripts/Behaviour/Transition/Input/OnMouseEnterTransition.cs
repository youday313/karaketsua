using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/OnMouseEnterTransition")]
	[BuiltInBehaviour]
	public class OnMouseEnterTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;

		void OnMouseEnter()
		{
			Transition(_NextState);
		}
	}
}