using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/UI/UIToggleTransition")]
	public class UIToggleTransition : StateBehaviour
	{
		[SerializeField] private Toggle _Toggle;
		[SerializeField] private bool _ChangeTimingTransition;
		[SerializeField] private StateLink _OnState;
		[SerializeField] private StateLink _OffState;

		void Awake()
		{
			if (_Toggle == null)
			{
				_Toggle = GetComponent<Toggle>();
			}
		}

		void Transition(bool on)
		{
			if (on)
			{
				Transition(_OnState);
			}
			else
			{
				Transition(_OffState);
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Toggle != null)
			{
				if (!_ChangeTimingTransition)
				{
					Transition(_Toggle.isOn);
				}
				else
				{
					_Toggle.onValueChanged.AddListener(Transition);
				}
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			if (_Toggle != null)
			{
				if (_ChangeTimingTransition)
				{
					_Toggle.onValueChanged.RemoveListener(Transition);
				}
			}
		}
	}
}