using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/UI/UISliderTransition")]
	public class UISliderTransition : StateBehaviour
	{
		[SerializeField] private Slider _Slider;
		[SerializeField] private bool _ChangeTimingTransition;
		[SerializeField] private float _Threshold;
		[SerializeField] private StateLink _LessState;
		[SerializeField] private StateLink _GreaterState;

		void Awake()
		{
			if (_Slider == null)
			{
				_Slider = GetComponent<Slider>();
			}
		}

		void Transition(float value)
		{
			if (value <= _Threshold)
			{
				Transition(_LessState);
			}
			else
			{
				Transition(_GreaterState);
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Slider != null)
			{
				if (!_ChangeTimingTransition)
				{
					Transition(_Slider.value);
				}
				else
				{
					_Slider.onValueChanged.AddListener(Transition);
				}
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			if (_Slider != null)
			{
				if (_ChangeTimingTransition)
				{
					_Slider.onValueChanged.RemoveListener(Transition);
				}
			}
		}
	}
}