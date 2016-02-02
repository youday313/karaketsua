using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/EventSystems/OnPointerUpTransition")]
	[BuiltInBehaviour]
	public class OnPointerUpTransition : StateBehaviour , IPointerUpHandler
	{
		[SerializeField] private bool _CheckButton;
		[SerializeField] private PointerEventData.InputButton _Button;
		[SerializeField] private StateLink _NextState;

		public void OnPointerUp(PointerEventData data)
		{
			if (!_CheckButton || data.button == _Button)
			{
				Transition(_NextState);
			}
		}
	}
}