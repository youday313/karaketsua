using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetSliderFromParameter")]
	[BuiltInBehaviour]
	public class UISetSliderFromParameter : StateBehaviour
	{
		[SerializeField] private Slider _Slider;
		[SerializeField] private FloatParameterReference _Parameter;
		[SerializeField] private bool _ChangeTimingUpdate;

		void Awake()
		{
			if (_Slider == null)
			{
				_Slider = GetComponent<Slider>();
			}
		}

		void UpdateSlider()
		{
			if (_Slider != null && _Parameter.parameter != null)
			{
				_Slider.value = _Parameter.parameter.floatValue;
			}
		}

		void OnChangedParameter(Parameter parameter)
		{
			UpdateSlider();
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			UpdateSlider();

			if (_Parameter.parameter != null && _ChangeTimingUpdate)
			{
				_Parameter.parameter.onChanged += OnChangedParameter;
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			if (_Parameter.parameter != null && _ChangeTimingUpdate)
			{
				_Parameter.parameter.onChanged -= OnChangedParameter;
			}
		}
	}
}