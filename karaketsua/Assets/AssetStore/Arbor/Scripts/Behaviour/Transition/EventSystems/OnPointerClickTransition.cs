using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/EventSystems/OnPointerClickTransition")]
	[BuiltInBehaviour]
	public class OnPointerClickTransition : StateBehaviour , IPointerClickHandler
	{
		[SerializeField] private bool _CheckButton;
		[SerializeField] private PointerEventData.InputButton _Button;
		[SerializeField] private StateLink _NextState;

		public void OnPointerClick(PointerEventData data)
		{
			if (!_CheckButton || data.button == _Button)
			{
				Transition(_NextState);
			}
		}
	}
}