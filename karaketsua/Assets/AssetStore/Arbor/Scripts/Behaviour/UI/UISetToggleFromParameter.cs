using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetToggleFromParameter")]
	[BuiltInBehaviour]
	public class UISetToggleFromParameter : StateBehaviour
	{
		[SerializeField] private Toggle _Toggle;
		[SerializeField] private BoolParameterReference _Parameter;
		[SerializeField] private bool _ChangeTimingUpdate;

		void Awake()
		{
			if (_Toggle == null)
			{
				_Toggle = GetComponent<Toggle>();
			}
		}

		void UpdateToggle()
		{
			if (_Toggle != null && _Parameter.parameter != null)
			{
				_Toggle.isOn = _Parameter.parameter.boolValue;
			}
		}

		void OnChangedParameter(Parameter parameter)
		{
			UpdateToggle();
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			UpdateToggle();

			if (_Parameter.parameter != null && _ChangeTimingUpdate)
			{
				_Parameter.parameter.onChanged += OnChangedParameter;
            }
		}

		public override void OnStateEnd()
		{
			if (_Parameter.parameter != null && _ChangeTimingUpdate)
			{
				_Parameter.parameter.onChanged -= OnChangedParameter;
			}
		}
	}
}