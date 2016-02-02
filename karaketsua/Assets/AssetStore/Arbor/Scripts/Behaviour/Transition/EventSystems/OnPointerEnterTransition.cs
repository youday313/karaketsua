using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/EventSystems/OnPointerEnterTransition")]
	[BuiltInBehaviour]
	public class OnPointerEnterTransition : StateBehaviour , IPointerEnterHandler
	{
		[SerializeField] private StateLink _NextState;

		public void OnPointerEnter(PointerEventData data)
		{
			Transition(_NextState);
		}
	}
}