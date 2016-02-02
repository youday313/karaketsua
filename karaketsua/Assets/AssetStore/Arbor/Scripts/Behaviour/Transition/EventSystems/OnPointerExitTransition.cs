using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/EventSystems/OnPointerExitTransition")]
	[BuiltInBehaviour]
	public class OnPointerExitTransition : StateBehaviour , IPointerExitHandler
	{
		[SerializeField] private StateLink _NextState;

		public void OnPointerExit(PointerEventData data)
		{
			Transition(_NextState);
		}
	}
}