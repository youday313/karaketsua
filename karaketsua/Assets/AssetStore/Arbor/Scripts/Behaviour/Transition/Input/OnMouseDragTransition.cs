using UnityEngine;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Input/OnMouseDragTransition")]
	[BuiltInBehaviour]
	public class OnMouseDragTransition : StateBehaviour
	{
		[SerializeField] private StateLink _NextState;

		void OnMouseDrag()
		{
			Transition(_NextState);
		}
	}
}