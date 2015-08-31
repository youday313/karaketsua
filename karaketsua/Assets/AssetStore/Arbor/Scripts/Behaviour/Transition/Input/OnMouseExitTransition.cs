using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/OnMouseExitTransition")]
	[BuiltInBehaviour]
	public class OnMouseExitTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;

		void OnMouseExit()
		{
			Transition(_NextState);
		}
	}
}