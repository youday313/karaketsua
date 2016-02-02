using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Parameter/SetFloatParameterFromUISlider")]
	[BuiltInBehaviour]
	public class SetFloatParameterFromUISlider : StateBehaviour
	{
		[SerializeField] private FloatParameterReference _Parameter;
		[SerializeField] private Slider _Slider;
		[SerializeField] private bool _ChangeTimingUpdate;

		void UpdateParameter(float value)
		{
			if ( _Parameter.parameter != null && _Parameter.parameter.type == Parameter.Type.Float)
			{
				_Parameter.parameter.floatValue = value;
				_Parameter.parameter.OnChanged();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Slider != null )
			{
				UpdateParameter(_Slider.value);

				if (_ChangeTimingUpdate)
				{
					_Slider.onValueChanged.AddListener(UpdateParameter);
				}
			}
		}

		public override void OnStateEnd()
		{
			if (_ChangeTimingUpdate)
			{
				_Slider.onValueChanged.RemoveListener(UpdateParameter);
			}
		}
	}
}