using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/OnMouseOverTransition")]
	[BuiltInBehaviour]
	public class OnMouseOverTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;

		void OnMouseOver()
		{
			Transition(_NextState);
		}
	}
}