using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/EventSystems/OnPointerDownTransition")]
	[BuiltInBehaviour]
	public class OnPointerDownTransition : StateBehaviour , IPointerDownHandler
	{
		[SerializeField]
		private bool _CheckButton;
		[SerializeField]
		private PointerEventData.InputButton _Button;
		[SerializeField]
		private StateLink _NextState;

		public void OnPointerDown(PointerEventData data)
		{
			if (!_CheckButton || data.button == _Button)
			{
				Transition(_NextState);
			}
		}
	}
}