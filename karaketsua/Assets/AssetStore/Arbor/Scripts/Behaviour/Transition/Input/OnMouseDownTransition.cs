using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/OnMouseDownTransition")]
	[BuiltInBehaviour]
	public class OnMouseDownTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;

		void OnMouseDown()
		{
			Transition(_NextState);
		}
	}
}