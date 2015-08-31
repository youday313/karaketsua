using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/OnMouseUpTransition")]
	[BuiltInBehaviour]
	public class OnMouseUpTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;

		void OnMouseUp()
		{
			Transition(_NextState);
		}
	}
}