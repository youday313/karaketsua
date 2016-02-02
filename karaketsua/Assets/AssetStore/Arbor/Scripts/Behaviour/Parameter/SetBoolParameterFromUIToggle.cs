using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Arbor
{
	[AddComponentMenu("")]
	[AddBehaviourMenu("Parameter/SetBoolParameterFromUIToggle")]
	[BuiltInBehaviour]
	public class SetBoolParameterFromUIToggle : StateBehaviour
	{
		[SerializeField] private BoolParameterReference _Parameter;
		[SerializeField] private Toggle _Toggle;
		[SerializeField] private bool _ChangeTimingUpdate;

		void UpdateParameter(bool value)
		{
			if ( _Parameter.parameter != null && _Parameter.parameter.type == Parameter.Type.Bool)
			{
				_Parameter.parameter.boolValue = value;
				_Parameter.parameter.OnChanged();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			if (_Toggle != null )
			{
				UpdateParameter(_Toggle.isOn);

				if (_ChangeTimingUpdate)
				{
					_Toggle.onValueChanged.AddListener(UpdateParameter);
				}
			}
		}

		public override void OnStateEnd()
		{
			if (_ChangeTimingUpdate)
			{
				_Toggle.onValueChanged.RemoveListener(UpdateParameter);
			}
		}
	}
}