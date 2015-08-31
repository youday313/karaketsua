using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/OnMouseUpAsButtonTransition")]
	[BuiltInBehaviour]
	public class OnMouseUpAsButtonTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;

		void OnMouseUpAdButton()
		{
			Transition(_NextState);
		}
	}
}